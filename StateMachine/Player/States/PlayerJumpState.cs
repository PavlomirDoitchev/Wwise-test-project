using Assets.Scripts.Entities;
using Assets.Scripts.StateMachine.Player;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerJumpState : PlayerBaseState
    {
        private Vector3 horizontalMomentum = Vector3.zero;
        private float jumpTime;
        private float maxJumpTime = 0.7f;
        private bool isJumping;
        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
        public PlayerJumpState(PlayerStateMachine stateMachine, Vector3 momentum) : base(stateMachine)
        {
            this.horizontalMomentum = momentum;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Jump", .1f);
            jumpTime = 0f;
            isJumping = true;

            _playerStateMachine.ForceReceiver.ResetVertical();
            _playerStateMachine.ForceReceiver.Jump(_playerStateMachine.PlayerStats.JumpForce * 0.5f);

            _playerStateMachine.ForceReceiver.SetForce(new Vector3(horizontalMomentum.x, 0f, 0f));
        }
        public override void Tick(float deltaTime)
        {
            PlayerMoveAirborne(deltaTime);
            DoAirborneAttack();
            DoDash();
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
                Vector3 currentMomentum = GetHorizontalMomentum();

                _playerStateMachine.ForceReceiver.ResetVertical();

                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, currentMomentum));
                return;
            }
        }
        
        public override void Exit()
        {
        }
        
    }
}