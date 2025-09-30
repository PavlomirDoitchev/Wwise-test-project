using Assets.Scripts.State_Machine.Player_State_Machine;
using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.Play("Locomotion");
        }


        public override void Tick(float deltaTime)
        {
            PlayerMove(deltaTime);
            Fall();
            DoJump();
            Vector2 moveInput = _playerStateMachine.InputManager.MovementInput();
            bool wantsToAttack = _playerStateMachine.InputManager.PlayerAttackInput();
            bool isSprinting = _playerStateMachine.InputManager.PlayerSprintInput();

            if (wantsToAttack)
            {
                if (isSprinting && moveInput != Vector2.zero)
                {
                    _playerStateMachine.ChangeState(new PlayerSprintAttackState(_playerStateMachine));
                    return;
                }
                else
                {
                    _playerStateMachine.ChangeState(new PlayerAttackState(_playerStateMachine));
                    return;
                }
            }
        }
        public override void Exit()
        {
            
        }
        private void Fall()
        {
            if (_playerStateMachine.CharacterController.velocity.y <= -10)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;
            }
        }

        private void DoJump()
        {
            if (_playerStateMachine.InputManager.PlayerJumpInput() && _playerStateMachine.CharacterController.isGrounded)
            {
                AkUnitySoundEngine.PostEvent("Play_Jump", _playerStateMachine.gameObject);
                _playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine));
            }
        }
        private void DoAttack()
        {
            if (_playerStateMachine.InputManager.PlayerAttackInput())
            {
                _playerStateMachine.ChangeState(new PlayerAttackState(_playerStateMachine));
            }
        }
    }
}
