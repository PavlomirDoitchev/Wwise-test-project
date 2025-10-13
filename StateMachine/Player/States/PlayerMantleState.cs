using Assets.Scripts.State_Machine.Player_State_Machine;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerMantleState : PlayerBaseState
    {
        private Vector3 _targetPosition;
        private float _mantleAnimationDuration = 0.8f; 
        private float _timer = 0f;
        public PlayerMantleState(PlayerStateMachine stateMachine, Vector3 targetPosition)
            : base(stateMachine)
        {
            _targetPosition = targetPosition;
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Mantle", 0.1f);
           // _playerStateMachine.CharacterController.enabled = false;
        }

        public override void Tick(float deltaTime)
        {
            _timer += deltaTime;
            float t = Mathf.Clamp01(_timer / _mantleAnimationDuration);
            _playerStateMachine.ForceReceiver.verticalVelocity = 0f;
            if (_timer >= _playerStateMachine.waitForClimbThreshold)
            {
            // Move to top of ledge
                _playerStateMachine.transform.position = Vector3.Lerp(
                _playerStateMachine.transform.position,
                _targetPosition,
                t * .1f
                );
            }

            if (_timer >= _mantleAnimationDuration)
            {
                _playerStateMachine.CharacterController.enabled = true;
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
            }
        }

        public override void Exit()
        {
            //_playerStateMachine.CharacterController.enabled = true;
            _playerStateMachine.ForceReceiver.ResetForces();
        }
    }
}
