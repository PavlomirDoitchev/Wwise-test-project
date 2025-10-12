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
            _playerStateMachine.Animator.CrossFadeInFixedTime("Locomotion", 0.1f);
        }


        public override void Tick(float deltaTime)
        {
            Vector2 filteredInput = GetFilteredMovementInput();
            bool isMoving = filteredInput.x != 0;
            if (!isMoving && !_isTurning && _playerStateMachine.CurrentVelocity.magnitude > 0f)
            {
                _playerStateMachine.ChangeState(new PlayerRunEndState(_playerStateMachine));
            }
            else
            {
                PlayerMove(deltaTime);
                Fall(deltaTime);
                DoJump();
                MeleeAttacks();
                DoDash();
                DoSlide();
            }
        }
        public override void Exit()
        {

        }
        

    }
}
