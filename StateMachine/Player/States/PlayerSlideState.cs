using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.Utilities.Contracts;
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
            _playerStateMachine.CharacterController.height = .5f;
            _playerStateMachine.CharacterController.center = new Vector3(0f, 0.25f, 0f);
            _playerStateMachine.ForceReceiver
                .SetForce(_playerStateMachine.transform.forward * _playerStateMachine.PlayerStats.DashForce);
        }


        public override void Tick(float deltaTime)
        {
            if (_playerStateMachine.CharacterController.velocity.y < -10f)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, GetHorizontalMomentum()));
                return;
            }

            _duration -= deltaTime;

            if (_duration <= 0f)
            {
                if (CanStandUp())
                {
                    _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                    return;
                }
                else
                {
                    _duration = 0.1f;
                }
            }

            ApplySlideMovement(deltaTime);
        }

        public override void Exit()
        {
            _playerStateMachine.CharacterController.center = new Vector3(0f, 1f, 0f);
            _playerStateMachine.CharacterController.height = 2f;

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
        private bool CanStandUp()
        {
            var cc = _playerStateMachine.CharacterController;
            Vector3 start = _playerStateMachine.transform.position + Vector3.up * (cc.radius);
            Vector3 end = start + Vector3.up * (2f - cc.height);

            return !Physics.CheckCapsule(start, end, cc.radius,
                _playerStateMachine.groundMask); 
        }
        private void ApplySlideMovement(float deltaTime)
        {
            Vector3 slideDir = _playerStateMachine.transform.forward;

            _playerStateMachine.ForceReceiver.AddForce(slideDir * _playerStateMachine.PlayerStats.DashForce * deltaTime);

            Move(deltaTime);
        }
    }
}
