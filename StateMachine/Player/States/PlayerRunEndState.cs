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
            _playerStateMachine.Animator.CrossFadeInFixedTime("DualBlades_RunEnd", 0.4f);
        }

        public override void Tick(float deltaTime)
        {
            _animationDuration -= deltaTime;

            // Keep moving logic in case the player slides
            PlayerMove(deltaTime);
            Fall(deltaTime);
            DoJump();
            MeleeAttacks();
            DoDash();
            DoSlide();

            // After run-end animation finishes
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
