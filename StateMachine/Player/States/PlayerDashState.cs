using Assets.Scripts.Utilities.Contracts;
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
        public override void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.TryGetComponent<IPushable>(out _)) return;

            if (hit.gameObject.TryGetComponent<INotPushable>(out INotPushable notPushable))
            {
                if (Mathf.Abs(hit.normal.y) < 0.1f)
                {
                    if (hit.normal.x > 0) _playerStateMachine.currentLeftBlocker = notPushable;
                    if (hit.normal.x < 0) _playerStateMachine.currentRightBlocker = notPushable;

                    _playerStateMachine.ChangeState(new PlayerCollisionState(_playerStateMachine, 0.2f));
                }
            }
        }
    }
}
