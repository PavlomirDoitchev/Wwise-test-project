using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerWallJumpState : PlayerBaseState
    {
        private const float airControlLockTime = 0.25f;
        private const float airControlRestoreTime = 0.25f;

        private Vector3 jumpDirection;
        private float elapsed;
        private bool controlRestored;

        public PlayerWallJumpState(PlayerStateMachine stateMachine, Vector3 direction)
            : base(stateMachine)
        {
            jumpDirection = direction.normalized;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Jump", 0.1f);

            _playerStateMachine.ForceReceiver.ResetForces();
            _playerStateMachine.ForceReceiver.JumpTo(
                _playerStateMachine.PlayerStats.JumpForce,
                jumpDirection * _playerStateMachine.PlayerStats.BaseSpeed * 1.1f
            );

            if (jumpDirection.x > 0)
                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.right);
            else if (jumpDirection.x < 0)
                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.left);

            elapsed = 0f;
            controlRestored = false;
        }

        public override void Tick(float deltaTime)
        {
            elapsed += deltaTime;

            if (elapsed < airControlLockTime)
            {
                Move(_playerStateMachine.ForceReceiver.Movement * deltaTime, deltaTime);
                return;
            }

            float t = Mathf.Clamp01((elapsed - airControlLockTime) / airControlRestoreTime);
            float controlFactor = Mathf.SmoothStep(0f, 1f, t);

            Vector2 input = _playerStateMachine.InputManager.MovementInput();
            Vector3 moveInput = new Vector3(input.x, 0, input.y).normalized * controlFactor;

            Vector3 totalMove = (moveInput * _playerStateMachine.PlayerStats.BaseSpeed)
                                + _playerStateMachine.ForceReceiver.Movement;

            Move(totalMove * deltaTime, deltaTime);

            if (_playerStateMachine.CharacterController.velocity.y <= 0)
            {
                Vector3 momentum = GetHorizontalMomentum();
                _playerStateMachine.ForceReceiver.ResetVertical();
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, momentum));
            }
        }

        public override void Exit() { }
    }
}
