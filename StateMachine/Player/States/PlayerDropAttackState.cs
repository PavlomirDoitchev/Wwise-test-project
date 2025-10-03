using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerDropAttackState : PlayerBaseState
    {
        private Vector3 momentum;
        public PlayerDropAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("ARPG_Samurai_Airborne", 0.1f);
            momentum = _playerStateMachine.CharacterController.velocity;
            momentum.y = 0;
        }



        public override void Tick(float deltaTime)
        {
            if (_playerStateMachine.IsSupported()) 
            {
                //Change to landing
            }
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
