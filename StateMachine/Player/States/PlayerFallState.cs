using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

namespace Assets.Scripts.State_Machine.Player_State_Machine
{
    public class PlayerFallState : PlayerBaseState
    {
        private Vector3 _momentum;
        private Vector3 hangPoint, standPoint;

        public PlayerFallState(PlayerStateMachine stateMachine, Vector3 momentum) : base(stateMachine)
        {
            _momentum = momentum;

        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("ARPG_Samurai_Airborne", 0.1f);
            _playerStateMachine.ForceReceiver.SetForce(_momentum);
            Debug.Log("Fall state entered");
        }

        public override void Tick(float deltaTime)
        {
            PlayerMoveAirborne(deltaTime);
            DoDash();
            if (_playerStateMachine.IsTouchingWall) 
            {
                _playerStateMachine.ChangeState(new PlayerWallSlideState(_playerStateMachine));
            }
            if (CheckLedge(out hangPoint, out standPoint) && _playerStateMachine.ForceReceiver.verticalVelocity > -50f)
            {
                _playerStateMachine.ChangeState(new PlayerMantleState(_playerStateMachine, standPoint));
                return;
            }
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