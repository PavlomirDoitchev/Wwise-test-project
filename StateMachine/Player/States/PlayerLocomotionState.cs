using Assets.Scripts.State_Machine.Player_State_Machine;
using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        private float _justEnteredTimer = 0.1f;
        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _justEnteredTimer = 0.1f;
            _playerStateMachine.Animator.CrossFadeInFixedTime("Locomotion", 0.1f);
        }


        public override void Tick(float deltaTime)
        {
            if (_justEnteredTimer > 0)
                _justEnteredTimer -= deltaTime;

            float velocityThreshold = 0.01f;
            float inputX = GetFilteredMovementInput().x;
            float velocityX = _playerStateMachine.CurrentVelocity.x;

            if (_justEnteredTimer <= 0)
            {
                if (Mathf.Abs(velocityX) > velocityThreshold && inputX != 0 && Mathf.Sign(inputX) != Mathf.Sign(velocityX) && _playerStateMachine.CharacterController.isGrounded)
                {
                    _playerStateMachine.ChangeState(new PlayerTurnState(_playerStateMachine));
                }
                else if (Mathf.Abs(velocityX) <= velocityThreshold && inputX == 0)
                {
                    _playerStateMachine.ChangeState(new PlayerRunEndState(_playerStateMachine));
                }
            }

            PlayerMove(deltaTime);
            Fall(deltaTime);
            DoCrouch();
            DoJump();
            MeleeAttacks();
            DoDash();
            DoSlide();
        }
        public override void Exit()
        {
        }
        

    }
}
