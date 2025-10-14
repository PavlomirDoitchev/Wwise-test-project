//using Assets.Scripts.State_Machine.Player_State_Machine;
//using UnityEngine;

//namespace Assets.Scripts.StateMachine.Player.States
//{
//    public class PlayerWallJumpState : PlayerBaseState
//    {
//        private const float airControlLockTime = 0.25f;
//        private const float airControlRestoreTime = 0.25f;

//        private Vector3 jumpDirection;
//        private float elapsed;
//        private bool controlRestored;

//        private float jumpTime;
//        private float maxJumpTime = 0.3f;
//        private bool isJumping;
//        private Vector3 hangPoint, standPoint;

//        public PlayerWallJumpState(PlayerStateMachine stateMachine, Vector3 direction)
//            : base(stateMachine)
//        {
//            jumpDirection = direction.normalized;
//        }

//        public override void Enter()
//        {
//            _playerStateMachine.Animator.CrossFadeInFixedTime("Jump", 0.1f);

//            _playerStateMachine.ForceReceiver.ResetForces();
//            _playerStateMachine.ForceReceiver.JumpTo(
//                _playerStateMachine.PlayerStats.JumpForce * .1f,
//                jumpDirection * _playerStateMachine.PlayerStats.BaseSpeed * 3f
//            );

//            if (jumpDirection.x > 0)
//                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.right);
//            else if (jumpDirection.x < 0)
//                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.left);

//            elapsed = 0f;
//            controlRestored = false;
//            jumpTime = 0f;
//            isJumping = true;
//        }

//        public override void Tick(float deltaTime)
//        {
//            elapsed += deltaTime;
//            Move(deltaTime);
//            if (isJumping && _playerStateMachine.InputManager.JumpHeld() && jumpTime < maxJumpTime)
//            {
//                float jumpForcePerFrame = _playerStateMachine.PlayerStats.JumpForce * deltaTime / maxJumpTime;
//                _playerStateMachine.ForceReceiver.JumpTo(jumpForcePerFrame * .2f, jumpDirection * .1f);
//                jumpTime += deltaTime;
//            }
//            else
//            {
//                isJumping = false;
//            }

//            if (CheckLedge(out hangPoint, out standPoint))
//            {
//                _playerStateMachine.ChangeState(new PlayerMantleState(_playerStateMachine, standPoint));
//                return;
//            }

//            if (elapsed < airControlLockTime)
//            {
//                Move(_playerStateMachine.ForceReceiver.Movement * deltaTime, deltaTime);
//                return;
//            }

//            float t = Mathf.Clamp01((elapsed - airControlLockTime) / airControlRestoreTime);
//            float controlFactor = Mathf.SmoothStep(0f, 1f, t);

//            Vector2 input = _playerStateMachine.InputManager.MovementInput();
//            Vector3 moveInput = new Vector3(input.x, 0, input.y).normalized * controlFactor;

//            Vector3 totalMove = (moveInput * _playerStateMachine.PlayerStats.BaseSpeed)
//                                + _playerStateMachine.ForceReceiver.Movement;

//            Move(totalMove * deltaTime, deltaTime);

//            if (_playerStateMachine.CharacterController.velocity.y <= 0)
//            {
//                Vector3 currentMomentum = GetHorizontalMomentum();

//                _playerStateMachine.ForceReceiver.ResetVertical();

//                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, currentMomentum));
//                return;
//            }
//        }

//        public override void Exit() { }
//    }
//}




using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerWallJumpState : PlayerBaseState
    {
        private const float airControlLockTime = 0.1f;
        private const float airControlRestoreTime = 0.2f;

        private Vector3 jumpDirection;
        private float elapsed;

        private float jumpTime;
        private float maxJumpTime = 0.2f;
        private bool isJumping;

        private Vector3 hangPoint, standPoint;

        private float initialHorizontalPush = 16f;
        private float initialVerticalPush = 15f;
        private float holdHorizontalFactor = 2f;
        private float holdVerticalFactor = 0.5f;

        public PlayerWallJumpState(PlayerStateMachine stateMachine, Vector3 direction)
            : base(stateMachine)
        {
            jumpDirection = direction.normalized;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Jump", 0.1f);

            _playerStateMachine.ForceReceiver.ResetForces();

            Vector3 horizontalImpulse = new Vector3(jumpDirection.x, 0f, jumpDirection.z) * initialHorizontalPush;
            _playerStateMachine.ForceReceiver.JumpTo(initialVerticalPush, horizontalImpulse);

            if (jumpDirection.x > 0)
                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.right);
            else if (jumpDirection.x < 0)
                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.left);

            elapsed = 0f;
            jumpTime = 0f;
            isJumping = true;
        }

        public override void Tick(float deltaTime)
        {
            elapsed += deltaTime;

            if (isJumping && _playerStateMachine.InputManager.JumpHeld() && jumpTime < maxJumpTime)
            {
                float extraVertical = initialVerticalPush * holdVerticalFactor * deltaTime / maxJumpTime;
                float extraHorizontal = initialHorizontalPush * holdHorizontalFactor * deltaTime / maxJumpTime;

                Vector3 horizontalDir = new Vector3(jumpDirection.x, 0f, jumpDirection.z).normalized;
                _playerStateMachine.ForceReceiver.Jump(extraVertical);
                _playerStateMachine.ForceReceiver.AddForce(horizontalDir * extraHorizontal);

                jumpTime += deltaTime;
            }
            else
            {
                isJumping = false;
            }

            if (CheckLedge(out hangPoint, out standPoint))
            {
                _playerStateMachine.ChangeState(new PlayerMantleState(_playerStateMachine, standPoint));
                return;
            }

            if (elapsed < airControlLockTime)
            {
                Move(_playerStateMachine.ForceReceiver.Movement * deltaTime, deltaTime);
                return;
            }

            float t = Mathf.Clamp01((elapsed - airControlLockTime) / airControlRestoreTime);
            float controlFactor = Mathf.SmoothStep(0f, 1f, t);

            Vector2 input = _playerStateMachine.InputManager.MovementInput();
            Vector3 moveInput = new Vector3(input.x, 0f, input.y).normalized * controlFactor;

            Vector3 totalMove = new Vector3(moveInput.x, 0f, moveInput.z) * _playerStateMachine.PlayerStats.BaseSpeed
                                + new Vector3(_playerStateMachine.ForceReceiver.Movement.x, 0f, _playerStateMachine.ForceReceiver.Movement.z);

            Move(totalMove * deltaTime, deltaTime);

            // Switch to fall state when moving down
            if (_playerStateMachine.CharacterController.velocity.y <= 0f)
            {
                Vector3 horizontalMomentum = new Vector3(_playerStateMachine.ForceReceiver.Movement.x, 0f, /*_playerStateMachine.ForceReceiver.Movement.z*/0f);
                _playerStateMachine.ForceReceiver.ResetVertical();
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, horizontalMomentum));
            }
        }

        public override void Exit() { }
    }
}
