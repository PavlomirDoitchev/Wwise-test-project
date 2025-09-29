using System;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerLocomotionState : PlayerBaseState
    {
        public PlayerLocomotionState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            
        }


        public override void Tick(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                AkUnitySoundEngine.PostEvent("Play_Jump", _playerStateMachine.gameObject);
        }
        public override void Exit()
        {
            
        }
    }
}
