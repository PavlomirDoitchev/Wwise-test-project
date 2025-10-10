using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.Utilities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerSlideState : PlayerBaseState
    {
        float _duration;
        public PlayerSlideState(PlayerStateMachine stateMachine, float duration) : base(stateMachine)
        {
            _duration = duration;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Slide", .1f);
            _playerStateMachine.ForceReceiver
                .SetForce(_playerStateMachine.transform.forward * _playerStateMachine.PlayerStats.DashForce);
        }


        public override void Tick(float deltaTime)
        {
            if (_playerStateMachine.CharacterController.velocity.y < -10f)
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, GetHorizontalMomentum()));
            _duration -= deltaTime;
            if(_duration <= 0f)
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
