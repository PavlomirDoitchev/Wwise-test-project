using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerLandingState : PlayerBaseState
    {
        private readonly string landingSound = "Play_Dirt_Landing";
        private readonly int LandingAnimation = Animator.StringToHash("Land");
        float timer = 0f;
        float landingDuration;

        public PlayerLandingState(PlayerStateMachine stateMachine, float time) : base(stateMachine)
        {
            landingDuration = time;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime(LandingAnimation, 0.1f);
            if (_playerStateMachine.InputManager.MoveInput.x == 0)
            {
                AkUnitySoundEngine.PostEvent(landingSound, _playerStateMachine.gameObject);

            }
        }
        public override void Tick(float deltaTime)
        {
            if (_playerStateMachine.InputManager.MoveInput.x != 0) 
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
            }
            PlayerMove(deltaTime);
            DoAttack();
            timer += deltaTime;
            if (timer >= landingDuration)
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                return;
            }
        }
        public override void Exit()
        {
            
        }
        
    }
}
