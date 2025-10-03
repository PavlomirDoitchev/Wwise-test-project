using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("ARPG_Samurai_Airborne", 0.1f);

            _playerStateMachine.ForceReceiver.verticalVelocity = -2f;

            //Collider groundCollider = null;
            //foreach (var probe in _playerStateMachine.groundProbes)
            //{
            //    if (Physics.Raycast(probe.position, Vector3.down, out RaycastHit hit, _playerStateMachine.probeDistance + 0.2f, _playerStateMachine.groundMask))
            //    {
            //        groundCollider = hit.collider;
            //        break;
            //    }
            //}

            //if (groundCollider != null)
            //{
            //    _playerStateMachine.currentGroundCollider = groundCollider;
            //    Physics.IgnoreCollision(_playerStateMachine.CharacterController, groundCollider, true);

            //    _playerStateMachine.CharacterController.Move(Vector3.up * 0.05f);
            //}
        }

        public override void Tick(float deltaTime)
        {
            PlayerMoveAirborne(deltaTime);

            //DoAirborneAttack();

            if (IsGrounded())
            {
                //Physics.IgnoreLayerCollision(_playerStateMachine.gameObject.layer, _playerStateMachine.groundMask, false);
                HandleLanding();
            }
        }

        public override void Exit() { }

        
    }
}