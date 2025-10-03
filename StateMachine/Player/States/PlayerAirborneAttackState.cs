using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;
namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerAirborneAttackState : PlayerBaseState
    {
        private Vector3 momentum;
        public PlayerAirborneAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Attack_Airborne", 0.1f);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
            _playerStateMachine.ForceReceiver.ResetForces();
        }



        public override void Tick(float deltaTime)
        {
            PlayerMoveAirborne(deltaTime);

            if(_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;
            }
            if (CheckGrounded())
            {
                HandleLanding();
            }
        }

        public override void Exit()
        {
        }
    }
}
