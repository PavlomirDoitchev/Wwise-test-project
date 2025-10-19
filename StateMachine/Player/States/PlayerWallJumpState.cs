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

        private float initialHorizontalPush = 5f;
        private float initialVerticalPush = 15f;
        private float holdVerticalFactor = 0.2f;
        private Vector3 forwardMotion;

        public PlayerWallJumpState(PlayerStateMachine stateMachine, Vector3 direction) : base(stateMachine)
        {
            jumpDirection = direction.normalized;
        }

        public override void Enter()
        {

            _playerStateMachine.Animator.CrossFadeInFixedTime("Jump", 0.1f);
            _playerStateMachine.ForceReceiver.ResetForces();

            var wallSide = _playerStateMachine.GetWallContact();

            Vector3 horizontalDir;
            if (wallSide == PlayerStateMachine.WallSide.Left)
            {
                horizontalDir = Vector3.right;
            }
            else if (wallSide == PlayerStateMachine.WallSide.Right)
            {
                horizontalDir = Vector3.left;
            }
            else
            {
                horizontalDir = jumpDirection.x >= 0 ? Vector3.right : Vector3.left;
            }

            Vector3 finalJumpDir = (horizontalDir + Vector3.up).normalized;

            _playerStateMachine.ForceReceiver.ResetForces();
            _playerStateMachine.ForceReceiver.JumpTo(initialVerticalPush, horizontalDir * initialHorizontalPush);

            Quaternion targetRotation = Quaternion.LookRotation(horizontalDir, Vector3.up);
            _playerStateMachine.transform.rotation = targetRotation;

            elapsed = 0f;
            jumpTime = 0f;
            isJumping = true;
        }

        public override void Tick(float deltaTime)
        {

            elapsed += deltaTime;
            DoDash();
            if (isJumping && _playerStateMachine.InputManager.JumpHeld() && jumpTime < maxJumpTime)
            {
                float extraVertical = initialVerticalPush * holdVerticalFactor * deltaTime / maxJumpTime;
                if (jumpDirection.x >= 0)
                {
                    forwardMotion = Vector3.left;
                }
                else { forwardMotion = Vector3.right; }
                _playerStateMachine.ForceReceiver.JumpTo(extraVertical, forwardMotion);
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

            Move(_playerStateMachine.ForceReceiver.Movement * deltaTime, deltaTime);

            if (_playerStateMachine.ForceReceiver.verticalVelocity <= -10f)
            {
                Vector3 horizontalMomentum = GetHorizontalMomentum();
                Debug.Log($"[WallJump→Fall] Momentum: {_playerStateMachine.ForceReceiver.Movement}, Facing: {_playerStateMachine.transform.forward}");
                _playerStateMachine.LastFacingDirection = _playerStateMachine.transform.forward;
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, horizontalMomentum));
            }
        }

        public override void Exit() { }
    }
}