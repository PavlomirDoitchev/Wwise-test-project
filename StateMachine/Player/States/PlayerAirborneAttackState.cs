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
            _playerStateMachine.Animator.CrossFadeInFixedTime("Attack_Airborne", 0.1f);

            attackDuration = _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).length;
            elapsedTime = 0f;

            _playerStateMachine.ForceReceiver.ResetVertical();

            _playerStateMachine.ForceReceiver.Jump(upwardKick * 4);
        }

        public override void Tick(float deltaTime)
        {
            elapsedTime += deltaTime;

            // Time since attack started
            float normalizedTime = _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            // Hang phase: slow gravity for the first bit
            if (elapsedTime <= hangTime)
            {
                // Lessen gravity’s effect to “float” a little
                _playerStateMachine.ForceReceiver.verticalVelocity +=
                    Physics.gravity.y * hangGravityMultiplier * deltaTime;
            }
            else
            {
                // Resume normal falling
                _playerStateMachine.ForceReceiver.verticalVelocity +=
                    Physics.gravity.y * 5 * deltaTime;
            }

            Move(deltaTime);

            // Exit conditions
            if (elapsedTime >= attackDuration || normalizedTime >= 1f)
            {
                Vector3 currentMomentum = GetHorizontalMomentum();
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
            // Reset extra forces for clean fall or landing
            _playerStateMachine.ForceReceiver.ResetVertical();
        }
    }
}
