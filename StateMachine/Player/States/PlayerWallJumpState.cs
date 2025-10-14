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
        private float initialVerticalPush = 20f;
        private float holdHorizontalFactor = 2f;
        private float holdVerticalFactor = 0.2f;

        private float rotationLockDuration = 0.15f;
        private float rotationLockTimer = 0f;
        private bool rotationLocked = true;
        public PlayerWallJumpState(PlayerStateMachine stateMachine, Vector3 direction) : base(stateMachine)
        {
            jumpDirection = direction.normalized;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Jump", 0.1f);

            _playerStateMachine.ForceReceiver.ResetForces();

            Vector3 horizontalImpulse = new Vector3(jumpDirection.x, 0f, 0f).normalized * initialHorizontalPush;
            _playerStateMachine.ForceReceiver.JumpTo(initialVerticalPush, horizontalImpulse);

            if (jumpDirection.x > 0)
                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.right);
            else if (jumpDirection.x < 0)
                _playerStateMachine.transform.rotation = Quaternion.LookRotation(Vector3.left);


            elapsed = 0f;
            jumpTime = 0f;
            isJumping = true;
            rotationLockTimer = 0f;
        }

        public override void Tick(float deltaTime)
        {
            elapsed += deltaTime;
            rotationLockTimer += deltaTime;
            if (rotationLockTimer > rotationLockDuration)
                rotationLocked = false;
            if (isJumping && _playerStateMachine.InputManager.JumpHeld() && jumpTime < maxJumpTime)
            {
                float extraVertical = initialVerticalPush * holdVerticalFactor * deltaTime / maxJumpTime;
                float extraHorizontal = initialHorizontalPush * holdHorizontalFactor * deltaTime / maxJumpTime;

                Vector3 horizontalDir = new Vector3(jumpDirection.x, 0f, 0f).normalized;

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
            }
            else
            {
                float t = Mathf.Clamp01((elapsed - airControlLockTime) / airControlRestoreTime);
                float controlFactor = Mathf.SmoothStep(0f, 1f, t);

                Vector2 input = _playerStateMachine.InputManager.MovementInput();
                Vector3 moveInput = new Vector3(input.x, 0f, 0f).normalized * controlFactor;

                Vector3 totalMove = new Vector3(
                    moveInput.x * _playerStateMachine.PlayerStats.BaseSpeed + _playerStateMachine.ForceReceiver.Movement.x,
                    _playerStateMachine.ForceReceiver.Movement.y,
                    0f
                );

                Move(totalMove * deltaTime, deltaTime);

                if (!rotationLocked)
                {
                    float inputX = _playerStateMachine.InputManager.MovementInput().x;
                    if (Mathf.Abs(inputX) > 0.1f)
                        HandleFlip(inputX);
                }
            }

            // Transition to fall
            if (_playerStateMachine.ForceReceiver.verticalVelocity <= -10f)
            {
                Vector3 horizontalMomentum = GetHorizontalMomentum();
                //_playerStateMachine.ForceReceiver.ResetVertical();
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, horizontalMomentum));
            }
        }

        public override void Exit() { }
    }
}

