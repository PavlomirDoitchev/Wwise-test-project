using Assets.Scripts.StateMachine.Player;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerSprintAttackState : PlayerBaseState
    {
        private readonly string sprintAttackAnimation = "Attack_Sprint";
        private float animationTimer = 0f;
        private float currentClipLength = 0f;
        private float crossfadeDuration = 0.05f;
        private float forwardForce = 40f;

        public PlayerSprintAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            animationTimer = 0f;
            _playerStateMachine.Animator.CrossFadeInFixedTime(sprintAttackAnimation, crossfadeDuration);

            _playerStateMachine.ForceReceiver.SetForce(_playerStateMachine.transform.forward * forwardForce);
            PreserveDirection();
            AkUnitySoundEngine.PostEvent("Play_Swing_Woosh", _playerStateMachine.gameObject);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime); 

            animationTimer += deltaTime;

            
            AnimatorClipInfo[] clipInfo = _playerStateMachine.Animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
                currentClipLength = clipInfo[0].clip.length;

            if (animationTimer >= currentClipLength)
            {
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
            }
        }

        public override void Exit()
        {
            _playerStateMachine.ForceReceiver.SetForce(Vector3.zero);
        }
    }
}
