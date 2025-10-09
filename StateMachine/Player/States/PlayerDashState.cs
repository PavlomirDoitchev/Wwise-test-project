using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerDashState : PlayerBaseState
    {
        float duration;
        public PlayerDashState(PlayerStateMachine stateMachine, float duration) : base(stateMachine)
        {
            this.duration = duration;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Dash", 0.1f);
            _playerStateMachine.ForceReceiver.SetForce(_playerStateMachine.transform.forward * _playerStateMachine.PlayerStats.DashForce);
        }

       

        public override void Tick(float deltaTime)
        {
            _playerStateMachine.ForceReceiver.verticalVelocity = 0f;
            duration -= deltaTime;
            if (duration <= 0f)
            {
                _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                return;
            }
            Move(deltaTime);
        }

        public override void Exit()
        {
            _playerStateMachine.ForceReceiver.SetForce(Vector3.zero);
        }
    }
}
