using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.StateMachine.Player;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerAttackState : PlayerBaseState
    {
        private readonly string[] attackAnimations = { "Attack_1", "Attack_2", "Attack_3" };

        private bool queuedNextAttack = false;
        private float animationTimer = 0f;
        private float currentClipLength = 0f;
        private float comboWindowStart = 0.7f; 
        private float crossfadeDuration = 0.05f;
        private float attackForwardForce = 25f;
        private float cancelWindowStart = 0.1f;

        public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) 
        {
          
        }

        public override void Enter()
        {
            queuedNextAttack = false;
            PlayAttackAnimation();
            AddForwardMomentum();
            AkUnitySoundEngine.PostEvent("Play_Sword_Swing", _playerStateMachine.gameObject);

        }

        public override void Tick(float deltaTime)
        {
            //Vector2 moveInput = _playerStateMachine.InputManager.MovementInput();
            bool wantsToJump = _playerStateMachine.InputManager.JumpInput();

            if (animationTimer >= currentClipLength * cancelWindowStart)
            {
                if (/*moveInput != Vector2.zero ||*/ wantsToJump)
                {
                    _playerStateMachine.ComboIndex = 0;
                    //_playerStateMachine.InputManager.ConsumeJump();
                    if (wantsToJump && _playerStateMachine.CharacterController.isGrounded)
                    {
                        _playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine));
                        return;
                    }
                    //else
                    //{
                    //    _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                    //    return;
                    //}
                }
            }
            RotateDuringAttack(deltaTime);
            animationTimer += deltaTime;
            _playerStateMachine.ComboCooldown.Tick(deltaTime);

            bool isLastAttack = _playerStateMachine.ComboIndex >= attackAnimations.Length - 1;

            if (!queuedNextAttack && animationTimer >= currentClipLength * comboWindowStart)
            {
                if (_playerStateMachine.InputManager.AttackInput() && !isLastAttack)
                {
                    queuedNextAttack = true;
                    _playerStateMachine.ComboIndex++;
                    PlayAttackAnimation();
                    AddForwardMomentum();
                    _playerStateMachine.ComboCooldown.Reset();
                    AkUnitySoundEngine.PostEvent("Play_Sword_Swing", _playerStateMachine.gameObject);
                }
            }

            float exitThreshold = currentClipLength * 1.01f;
            if (animationTimer >= exitThreshold)
            {
                if (isLastAttack || !queuedNextAttack)
                {
                    _playerStateMachine.ComboIndex = 0;
                    _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
                }
            }
        }

        public override void Exit() { }

        private void AddForwardMomentum() 
        {
            Vector3 forward = _playerStateMachine.transform.forward;
            _playerStateMachine.ForceReceiver.SetForce(forward * attackForwardForce);
        }
        private void PlayAttackAnimation()
        {
            int index = _playerStateMachine.ComboIndex;

            AnimatorClipInfo[] clipInfo = _playerStateMachine.Animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
                currentClipLength = clipInfo[0].clip.length;
            else
                currentClipLength = 1f; // fallback

            animationTimer = 0f;
            queuedNextAttack = false;

            _playerStateMachine.Animator.CrossFadeInFixedTime(attackAnimations[index], crossfadeDuration);

            _playerStateMachine.ComboCooldown.Start();
        }
    }
}
