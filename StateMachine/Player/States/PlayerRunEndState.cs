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
            //_playerStateMachine.Animator.Play("RunEnd");
            _playerStateMachine.Animator.CrossFadeInFixedTime("RunEnd", 3f);
        }

        public override void Tick(float deltaTime)
        {
            _animationDuration -= deltaTime;

            PlayerMove(deltaTime);
            Fall(deltaTime);
            DoJump();
            MeleeAttacks();
            DoDash();
            DoSlide();

            if (_animationDuration <= 0f)
            {
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
            }
        }

        public override void Exit()
        {
        }
    }
}
