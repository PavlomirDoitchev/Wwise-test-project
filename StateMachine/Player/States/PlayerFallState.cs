using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 momentum;
        private float maxFallSpeed;
        private readonly string landingSound = "Play_Dirt_Landing";
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
                if (_playerStateMachine.InputManager.MoveInput.x == 0)
                {
                    AkUnitySoundEngine.PostEvent(landingSound, _playerStateMachine.gameObject);

                }
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }
        }

        public override void Exit()
        {
        }

    }
}