using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
using static Assets.Scripts.StateMachine.Player.PlayerStateMachine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerWallSlideState : PlayerBaseState
    {
        private const float slideSpeed = -3.5f;
        private const float jumpHorizontalForce = 5f;

        public PlayerWallSlideState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("WallJump_Loop", 0.1f);
            _playerStateMachine.ForceReceiver.ResetForces();
        }

        public override void Tick(float deltaTime)
        {
            var wallSide = _playerStateMachine.GetWallContact();

            _playerStateMachine.ForceReceiver.verticalVelocity = Mathf.Max(_playerStateMachine.ForceReceiver.verticalVelocity, slideSpeed);

            if (_playerStateMachine.InputManager.JumpInput())
            {
                Vector3 jumpDir = Vector3.up;
                if (wallSide == PlayerStateMachine.WallSide.Left)
                    jumpDir += Vector3.right;
                else if (wallSide == PlayerStateMachine.WallSide.Right)
                    jumpDir += Vector3.left;

                jumpDir.Normalize();

                _playerStateMachine.ForceReceiver.ResetForces();
                _playerStateMachine.ForceReceiver.JumpTo(_playerStateMachine.PlayerStats.JumpForce, jumpDir * jumpHorizontalForce);

                _playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine));
                return;
            }

            if (!_playerStateMachine.IsTouchingWall || _playerStateMachine.CharacterController.isGrounded)
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                return;
            }

            Move(_playerStateMachine.ForceReceiver.Movement * deltaTime, deltaTime);
        }

        public override void Exit() { }
    }

}
