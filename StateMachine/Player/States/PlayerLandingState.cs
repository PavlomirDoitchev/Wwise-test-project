using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerLandingState : PlayerBaseState
    {
        private readonly string landingEvent = "Play_Landing";
        private readonly int LandingAnimation = Animator.StringToHash("Land");

        private float timer = 0f;
        private readonly float landingDuration;
        private string _currentSurface = "Dirt"; 
        private readonly LayerMask groundMask = ~0;
        private Vector3 hangPoint, standPoint;

        public PlayerLandingState(PlayerStateMachine stateMachine, float time) : base(stateMachine)
        {
            landingDuration = time;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime(LandingAnimation, 0.1f);

            UpdateSurface();

            AkUnitySoundEngine.SetSwitch("Landing_Material", _currentSurface, _playerStateMachine.gameObject);
            AkUnitySoundEngine.PostEvent(landingEvent, _playerStateMachine.gameObject);
        }

        public override void Tick(float deltaTime)
        {
            if (_playerStateMachine.InputManager.MoveInput.x != 0)
            {
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
                return;
            }

            DoJump();
            PlayerMove(deltaTime);
            ApplyPlatformMovement(deltaTime);
            DoAttack();
            DoDash();
            DoSlide();
            DoCrouch();
            timer += deltaTime;
            if (CheckLedge(out hangPoint, out standPoint))
            {
                _playerStateMachine.ChangeState(new PlayerMantleState(_playerStateMachine, standPoint));
                return;
            }
            if (timer >= landingDuration)
            {
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
            }
        }

        public override void Exit() { }

        private void UpdateSurface()
        {
            if (Physics.Raycast(_playerStateMachine.transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 1.5f, groundMask))
            {
                if (hit.collider.CompareTag("Stone"))
                    _currentSurface = "Stone";
                else if (hit.collider.CompareTag("Dirt"))
                    _currentSurface = "Dirt";
                else
                    _currentSurface = "Dirt";
            }
        }
    }
}
