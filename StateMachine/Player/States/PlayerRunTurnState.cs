using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerTurnState : PlayerBaseState
    {
        private float _animationDuration;

        public PlayerTurnState(PlayerStateMachine stateMachine, float animationDuration = 0.3f)
            : base(stateMachine)
        {
            _animationDuration = animationDuration;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("ChaseDown_End", 0.1f);

            float horizontalInput = _playerStateMachine.InputManager.MovementInput().x;
            float yRotation = horizontalInput > 0 ? 90f : -90f;
            _playerStateMachine.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }

        public override void Tick(float deltaTime)
        {
            _animationDuration -= deltaTime;

            if (_animationDuration <= 0f)
            {
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
            }

            PlayerMove(deltaTime);
            Fall(deltaTime);
            DoJump();
            MeleeAttacks();
            DoDash();
            DoSlide();
        }

        public override void Exit()
        {
        }
    }
}
