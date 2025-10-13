using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("DoubleBlades_Idle", 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            Vector2 input = GetFilteredMovementInput();

            if (input.x != 0 || _playerStateMachine.ForceReceiver.verticalVelocity < -10f)
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            Move(deltaTime);
            //Fall(deltaTime);
            DoJump();
            MeleeAttacks();
            DoDash();
            DoSlide();
        }

        public override void Exit() { }
    }
}
