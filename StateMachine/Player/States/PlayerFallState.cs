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
            //_playerStateMachine.Animator.CrossFadeInFixedTime("ARPG_Samurai_Airborne", 0.1f);
            //_playerStateMachine.ForceReceiver.SetForce(_momentum);


            _playerStateMachine.Animator.CrossFadeInFixedTime("ARPG_Samurai_Airborne", 0.1f);

            Vector3 horizontal = new Vector3(_momentum.x, 0f, 0f);

            _playerStateMachine.ForceReceiver.SetForce(new Vector3(horizontal.x, _playerStateMachine.ForceReceiver.verticalVelocity, 0f));

            _playerStateMachine.CurrentVelocity = new Vector3(horizontal.x, _playerStateMachine.CurrentVelocity.y, 0f);
            _playerStateMachine.transform.forward = _playerStateMachine.LastFacingDirection;
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