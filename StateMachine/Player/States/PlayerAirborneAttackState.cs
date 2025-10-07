//using Assets.Scripts.State_Machine.Player_State_Machine;
//using UnityEngine;
//namespace Assets.Scripts.StateMachine.Player.States
//{
//    public class PlayerAirborneAttackState : PlayerBaseState
//    {
//        //private Vector3 momentum;
//        private float attackDuration;
//        private float elapsedTime;
//        public PlayerAirborneAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
//        {
//        }

//        public override void Enter()
//        {
//            _playerStateMachine.Animator.CrossFadeInFixedTime("Attack_Airborne", 0.1f);
//            //momentum = _playerStateMachine.CharacterController.velocity;
//            //momentum.y = 0;
//            attackDuration = _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).length;
//            elapsedTime = 0f;

//            _playerStateMachine.ForceReceiver.ResetForces();
//        }



//        public override void Tick(float deltaTime)
//        {

//            elapsedTime += deltaTime;

//            Vector3 velocity = _playerStateMachine.CharacterController.velocity;
//            _playerStateMachine.ForceReceiver.verticalVelocity = 0f;
//            _playerStateMachine.ForceReceiver.AddForce(Vector3.up * 5f * deltaTime);

//            //PlayerMoveAirborne(deltaTime);
//            Move(deltaTime);

//            if (elapsedTime >= attackDuration || _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
//            {
//                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
//                return;
//            }

//            if (CheckGrounded())
//            {
//                HandleLanding();
//            }
//        }

//        public override void Exit()
//        {
//        }
//    }
//}
using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerAirborneAttackState : PlayerBaseState
    {
        private float attackDuration;
        private float elapsedTime;
        private float upwardForce = 5f;     
        private float snapHorizontalForce = 15f; 

        public PlayerAirborneAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Attack_Airborne", 0.1f);
            attackDuration = _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).length;
            elapsedTime = 0f;

            _playerStateMachine.ForceReceiver.ResetForces();

            //_playerStateMachine.ForceReceiver.SetForce(_playerStateMachine.transform.forward * 25);
        }

        public override void Tick(float deltaTime)
        {
            elapsedTime += deltaTime;

            _playerStateMachine.ForceReceiver.verticalVelocity = 0f;
            //_playerStateMachine.ForceReceiver.AddForce(Vector3.up * upwardForce * deltaTime);
            if(_playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .3f)
                _playerStateMachine.ForceReceiver.verticalVelocity -= upwardForce * deltaTime;
            Move(deltaTime);

            //HandleFlip(_playerStateMachine.transform.forward.x);

            if (elapsedTime >= attackDuration ||
                _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine));
                return;     
            }

            if (IsGrounded())
            {
                HandleLanding();
            }
        }

        public override void Exit()
        {
            _playerStateMachine.ForceReceiver.SetForce(Vector3.zero);
        }
    }
}
