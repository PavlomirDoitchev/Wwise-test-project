using Assets.Scripts.State_Machine.Player_State_Machine;
using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        private float unsupportedTime = 0f;
        private float fallDelay = 1f;
        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Locomotion", 0.1f);
        }


        public override void Tick(float deltaTime)
        {
            PlayerMove(deltaTime);
            Fall(deltaTime);
            DoJump();
            MeleeAttacks();
            DoDash();
            DoSlide();
        }
        public override void Exit()
        {

        }
        
        

        //private void Fall(float deltaTime)
        //{
        //    if (!IsGrounded() || _playerStateMachine.CharacterController.velocity.y <= -10f)
        //    {
        //        unsupportedTime += deltaTime;

        //        if (unsupportedTime >= fallDelay)
        //        {
        //            Vector3 currentMomentum = GetHorizontalMomentum();

        //            Physics.IgnoreLayerCollision(_playerStateMachine.gameObject.layer, _playerStateMachine.groundMask, true);
        //            _playerStateMachine.ForceReceiver.verticalVelocity = -2f;
        //            _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, currentMomentum));
        //        }
        //    }
        //    else
        //    {
        //        unsupportedTime = 0f;
        //    }
        //}
        

        

    }
}
