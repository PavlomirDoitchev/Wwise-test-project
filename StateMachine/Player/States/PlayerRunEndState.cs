using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerRunEndState : PlayerBaseState
    {
        private float _animationDuration;

        public PlayerRunEndState(PlayerStateMachine stateMachine, float animationDuration = 0.3f)
            : base(stateMachine)
        {
            _animationDuration = animationDuration;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("DualBlades_RunEnd", 0.2f);
        }

        public override void Tick(float deltaTime)
        {
            _animationDuration -= deltaTime;
            if (_animationDuration <= 0f)
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
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
