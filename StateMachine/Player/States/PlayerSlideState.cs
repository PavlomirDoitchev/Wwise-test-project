using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.Utilities.Contracts;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerSlideState : PlayerBaseState
    {
        float _duration;
        float _slideTimer;
        Vector3 momentum;
        public PlayerSlideState(PlayerStateMachine stateMachine, float duration) : base(stateMachine)
        {
            _duration = duration;
        }

        public override void Enter()
        {
            PreserveDirection();
            _playerStateMachine.Animator.CrossFadeInFixedTime("Slide", .1f);
            _playerStateMachine.CharacterController.height = .5f;
            _playerStateMachine.CharacterController.center = new Vector3(0f, 0.25f, 0f);
            momentum = _playerStateMachine.transform.forward * 3f; //testing momentum
        }


        public override void Tick(float deltaTime)
        {
            if (!_playerStateMachine.IsSupported()) 
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, momentum));
            }
            _slideTimer += deltaTime / _duration;
            float normalizedTime = Mathf.Clamp01(_slideTimer);
            float speedMultiplier = _playerStateMachine.slideSpeedCurve.Evaluate(normalizedTime);

            Vector3 move = _playerStateMachine.transform.forward *
                           _playerStateMachine.PlayerStats.DashForce * speedMultiplier;

            _playerStateMachine.CharacterController.Move(move * deltaTime);

            if (_slideTimer >= 1f && CanStandUp())
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
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
     
    }
}
