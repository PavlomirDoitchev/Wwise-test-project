using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.Utilities.Contracts;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerCrouchedState : PlayerBaseState
    {
        public PlayerCrouchedState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
        }

        public override void Tick(float deltaTime)
        {
            PlayerMove(deltaTime);
            Fall(deltaTime);
            DoJump();
            MeleeAttacks();
            DoDash();
            DoSlide();
            DetectSurface();
            if (_playerStateMachine.CharacterController.velocity.y < -10f)
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, GetHorizontalMomentum()));
        }

        public override void Exit()
        {
        }
        private void DetectSurface() 
        {
            RaycastHit hit;
            if (Physics.Raycast(_playerStateMachine.transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 1.5f))
            {
                if (hit.collider.gameObject.TryGetComponent<IPlatformDropable>(out var droppable)) 
                {
                    if(_playerStateMachine.InputManager.DropPlatform())
                        hit.collider.enabled = false;
                }

            }

        }
    }
}
