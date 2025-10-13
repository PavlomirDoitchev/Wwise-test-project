using UnityEngine;
namespace Assets.Scripts.StateMachine.Player.States
{
    public class PlayerCrouchState : PlayerBaseState
    {
        public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _playerStateMachine.Animator.CrossFadeInFixedTime("Crouch_Idle", 0.1f);
            //Debug.Log("Crouch State");
        }

       

        public override void Tick(float deltaTime)
        {
            if(!_playerStateMachine.InputManager.CrouchInput())
            {
                _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
                return;
            }
            Move(deltaTime);
            //HandleFlip(deltaTime);
            DoJump();
            DoDash();
            DoSlide();
            MeleeAttacks();
            Fall(deltaTime);

        }

        public override void Exit()
        {
        }
    }
}
