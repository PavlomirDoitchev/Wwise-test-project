using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerAirborneAttackState : PlayerBaseState
    {
        private float attackDuration;
        private float elapsedTime;
        private Vector3 horizontalMomentum = Vector3.zero;

        // Tunables for feel
        private float hangGravityMultiplier = 2f;   
        private float hangTime = 0.4f;                
        private float upwardKick = 4.5f;              

        public PlayerAirborneAttackState(PlayerStateMachine stateMachine, Vector3 momentum) : base(stateMachine) 
        {
            this.horizontalMomentum = momentum; 
        }
        public PlayerAirborneAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("DualBlades_JumpAttack1", 0.1f);

            attackDuration = _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).length;
            elapsedTime = 0f;

            //_playerStateMachine.ForceReceiver.ResetVertical();
            //_playerStateMachine.ForceReceiver.SetForce(new Vector3(horizontalMomentum.x, 0f, 0f));
            _playerStateMachine.ForceReceiver.SetForce(horizontalMomentum);

            _playerStateMachine.ForceReceiver.Jump(upwardKick * 4);
            PreserveDirection();
        }

        public override void Tick(float deltaTime)
        {
            elapsedTime += deltaTime;

            float normalizedTime = _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (elapsedTime <= hangTime)
            {
                _playerStateMachine.ForceReceiver.verticalVelocity +=
                    Physics.gravity.y * hangGravityMultiplier * deltaTime;
            }
            else
            {
                _playerStateMachine.ForceReceiver.verticalVelocity +=
                    Physics.gravity.y * 5 * deltaTime;
            }

            Move(deltaTime);

            if (elapsedTime >= attackDuration || normalizedTime >= 1f)
            {
                Vector3 currentMomentum = GetHorizontalMomentum();
                PreserveDirection();
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, currentMomentum));
                return;
            }

            if (IsGrounded())
            {
                HandleLanding();
            }
        }

        public override void Exit()
        {
            _playerStateMachine.ForceReceiver.ResetVertical();
        }
    }
}
