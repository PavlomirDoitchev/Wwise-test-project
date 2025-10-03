using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 momentum;
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("ARPG_Samurai_Airborne", 0.1f);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }

        public override void Tick(float deltaTime)
        {
            PlayerMove(deltaTime);
            AirborneAttack();
            if (_playerStateMachine.CharacterController.isGrounded)
            {
                if (_playerStateMachine.InputManager.MoveInput.x == 0)
                {
                    _playerStateMachine.ChangeState(new PlayerLandingState(_playerStateMachine, 0.7f));
                }
                else
                {
                    _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                }
            }
        }

        public override void Exit()
        {
        }

    }
}