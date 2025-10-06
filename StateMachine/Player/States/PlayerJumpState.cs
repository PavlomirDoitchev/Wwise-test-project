using Assets.Scripts.StateMachine.Player;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerJumpState : PlayerBaseState
    {
        private Vector3 momentum;
        private float jumpTime;
        private float maxJumpTime = 0.7f;
        private bool isJumping;
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Jump", .1f);
            jumpTime = 0f;
            isJumping = true;
            _playerStateMachine.ForceReceiver.ResetVertical();
            _playerStateMachine.ForceReceiver.Jump(_playerStateMachine.PlayerStats.JumpForce * 0.5f);
           
        }
        public override void Tick(float deltaTime)
        {
            PlayerMoveAirborne(deltaTime);
            DoAirborneAttack();

            if (isJumping && _playerStateMachine.InputManager.JumpInput() && jumpTime < maxJumpTime)
            {
                float jumpForcePerFrame = _playerStateMachine.PlayerStats.JumpForce * deltaTime / maxJumpTime;
                _playerStateMachine.ForceReceiver.Jump(jumpForcePerFrame);
                jumpTime += deltaTime;
            }
            else
            {
                isJumping = false; 
            }

            if (_playerStateMachine.CharacterController.velocity.y <= 0)
            {
                _playerStateMachine.ForceReceiver.ResetForces();
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;
            }
        }
        //public override void Tick(float deltaTime)
        //{
        //    PlayerMoveAirborne(deltaTime);
        //    DoAirborneAttack();
        //    if (_playerStateMachine.CharacterController.velocity.y <= 0)
        //    {
        //        _playerStateMachine.ForceReceiver.ResetForces();
        //        _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
        //        return;
        //    }
        //}
        public override void Exit()
        {
        }

    }
}