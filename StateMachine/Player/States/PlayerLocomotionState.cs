using Assets.Scripts.State_Machine.Player_State_Machine;
using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        private float unsupportedTime = 0f;
        private float fallDelay = 1f;
        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Locomotion", 0.1f);
        }


        public override void Tick(float deltaTime)
        {
            PlayerMove(deltaTime);
            Fall(deltaTime);
            DoJump();
            MeleeAttacks();
            
        }
        public override void Exit()
        {

        }
        private void MeleeAttacks()
        {
            Vector2 moveInput = _playerStateMachine.InputManager.MovementInput();
            bool wantsToAttack = _playerStateMachine.InputManager.AttackInput();
            bool isSprinting = _playerStateMachine.InputManager.SprintInput();

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

        private void Fall(float deltaTime)
        {
            if (!CheckGrounded())
            {
                unsupportedTime += deltaTime;

                if (unsupportedTime >= fallDelay)
                {
                    Physics.IgnoreLayerCollision(_playerStateMachine.gameObject.layer, _playerStateMachine.groundMask, true);
                    _playerStateMachine.ForceReceiver.verticalVelocity = -2f;
                    _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                }
            }
            else
            {
                unsupportedTime = 0f;
            }
        }


    }
}
