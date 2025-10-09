using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 _momentum;
        public PlayerFallState(PlayerStateMachine stateMachine, Vector3 momentum) : base(stateMachine)
        {
            _momentum = momentum;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("ARPG_Samurai_Airborne", 0.1f);
            _playerStateMachine.ForceReceiver.SetForce(_momentum);
        }

        public override void Tick(float deltaTime)
        {
            PlayerMoveAirborne(deltaTime);
            DoDash();
            if (IsGrounded())
            {
                HandleLanding();
            }
        }
        public override void Exit()
        {
        }
    }
}