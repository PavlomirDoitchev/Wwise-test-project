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
            _playerStateMachine.ForceReceiver.Jump(_playerStateMachine.PlayerStats.JumpForce * 0.5f);
             bool isSprinting = _playerStateMachine.InputManager.SprintInput();
            if (isSprinting && _playerStateMachine.InputManager.MoveInput.x != 0)
            {
                float sprintBoost = _playerStateMachine.PlayerStats.BaseSpeed * 2.5f; 
                _playerStateMachine.ForceReceiver.AddForce(new Vector3(_playerStateMachine.InputManager.MoveInput.x * sprintBoost, 0f, 0f));
            }
            //_playerStateMachine.ForceReceiver.Jump(_playerStateMachine.PlayerStats.JumpForce);
            //momentum = _playerStateMachine.CharacterController.velocity;
            //momentum.y = 0;
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