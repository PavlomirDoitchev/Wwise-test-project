using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
using static Assets.Scripts.StateMachine.Player.PlayerStateMachine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerWallSlideState : PlayerBaseState
    {
        private const float slideSpeed = -3.5f;

        public PlayerWallSlideState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("WallSlide", 0.1f);
            Debug.Log("Entered Wall Slide State");
        }

        public override void Tick(float deltaTime)
        {
            Vector3 move = Vector3.zero;
            move.y = slideSpeed;

            _playerStateMachine.ForceReceiver.AddForce(Vector3.down * 9.81f * 0.5f);

            if (_playerStateMachine.InputManager.JumpInput())
            {
                Vector3 jumpDir = _playerStateMachine.GetWallContact() == WallSide.Left
                    ? (Vector3.up + Vector3.right).normalized
                    : (Vector3.up + Vector3.left).normalized;

                _playerStateMachine.ForceReceiver.ResetForces();
                _playerStateMachine.ForceReceiver.AddForce(jumpDir * 8f);

                _playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine));
                return;
            }

            if (!_playerStateMachine.IsTouchingWall || _playerStateMachine.IsSupported())
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }

            Move(move, deltaTime);
        }

        public override void Exit() { }
    }
}
