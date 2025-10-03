using Assets.Scripts.StateMachine.Player;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerJumpState : PlayerBaseState
    {
        private Vector3 momentum;
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Jump", .1f);
            _playerStateMachine.ForceReceiver.Jump(_playerStateMachine.PlayerStats.JumpForce);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }
        public override void Tick(float deltaTime)
        {
            PlayerMoveAirborne(deltaTime);
            DoAirborneAttack();
            if (_playerStateMachine.CharacterController.velocity.y <= 0)
            {
                _playerStateMachine.ForceReceiver.ResetForces();
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;
            }
        }
        public override void Exit()
        {
        }

    }
}