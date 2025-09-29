using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 momentum;
        private float maxFallSpeed;
        private int fallDamage;
        bool canCastWhileFalling = false;
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("ARPG_Samurai_Airborne", 0.1f);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
            maxFallSpeed = 0f;
        }

        public override void Tick(float deltaTime)
        {
            PlayerMove(deltaTime);

            if (_playerStateMachine.CharacterController.isGrounded)
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit Fall State");
        }

    }
}