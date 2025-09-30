using Assets.Scripts.StateMachine.Player;

using UnityEngine;
namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerAttackState : PlayerBaseState
    {
        float timer;
        float comboWindow = .5f;
        int attackIndex = 0;
        private readonly string[] attackAnimations = { "Attack_1", "Attack_2", "Attack_3" };
        public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            attackIndex = 0;
            _playerStateMachine.Animator.Play(attackAnimations[attackIndex]);
            timer = comboWindow;
        }
        public override void Tick(float deltaTime)
        {
            AnimatorStateInfo stateInfo = _playerStateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            timer -= deltaTime;
            if (stateInfo.normalizedTime >= 1f)
            {
                if (attackIndex < attackAnimations.Length - 1 && _playerStateMachine.InputManager.PlayerAttackInput())
                {
                    attackIndex++;
                    PlayAttackAnimation();
                }
                else
                {
                    _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                }
            }
        }
        public override void Exit()
        {
        }
        private void PlayAttackAnimation()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime(attackAnimations[attackIndex], 0.1f);
        }
    }
}
