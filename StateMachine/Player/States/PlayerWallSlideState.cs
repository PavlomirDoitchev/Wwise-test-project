using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
using static Assets.Scripts.StateMachine.Player.PlayerStateMachine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerWallSlideState : PlayerBaseState
    {
        private const float jumpHorizontalForce = 25f;

        public PlayerWallSlideState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            if (_playerStateMachine.GetWallContact() == PlayerStateMachine.WallSide.Left)
                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.left);
            else
                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.right);

            _playerStateMachine.Animator.CrossFadeInFixedTime("WallJump_Loop", 0.1f);
        }

        public override void Tick(float deltaTime)
        {
            var wallSide = _playerStateMachine.GetWallContact();
            DoWallDash();
            _playerStateMachine.ForceReceiver.ApplyWallSlideGravity(.85f, -12f);
            if (_playerStateMachine.InputManager.JumpInput())
            {
                Vector3 jumpDir = Vector3.up;
                if (wallSide == PlayerStateMachine.WallSide.Left)
                    jumpDir += Vector3.right * 1.2f; 
                else if (wallSide == PlayerStateMachine.WallSide.Right)
                    jumpDir += Vector3.left * 1.2f;

                jumpDir.Normalize();

                _playerStateMachine.ForceReceiver.ResetForces();
                _playerStateMachine.ForceReceiver.JumpTo(
                    _playerStateMachine.PlayerStats.JumpForce * 0.1f,
                    jumpDir * jumpHorizontalForce * 0.7f 
                );

                if (wallSide == PlayerStateMachine.WallSide.Left)
                    _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.right);
                else
                    _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.left);

                _playerStateMachine.ChangeState(new PlayerWallJumpState(_playerStateMachine, jumpDir * jumpHorizontalForce));
                return;
            }

            bool validWall = false;
            if (wallSide == PlayerStateMachine.WallSide.Left)
                validWall = _playerStateMachine.IsFullyTouchingWall(Vector3.left);
            else if (wallSide == PlayerStateMachine.WallSide.Right)
                validWall = _playerStateMachine.IsFullyTouchingWall(Vector3.right);

            if (!validWall || _playerStateMachine.CharacterController.isGrounded)
            {
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
                return;
            }

            Move(_playerStateMachine.ForceReceiver.Movement * deltaTime, deltaTime);
        }

        public override void Exit() { }
    }

}
