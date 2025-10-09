using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

public class PlayerCollisionState : PlayerBaseState
{
    private float duration;

    public PlayerCollisionState(PlayerStateMachine stateMachine, float duration)
        : base(stateMachine)
    {
        this.duration = duration;
    }

    public override void Enter()
    {
        _playerStateMachine.Animator.CrossFadeInFixedTime("Collision", 0.1f);
        _playerStateMachine.ForceReceiver.SetForce(Vector3.zero);
        ImpulseManager.GenerateImpulse(new Vector3(0.1f, 0.1f, 0f), new Vector3(1f, 1f, 0f), 0.2f);
    }

    public override void Tick(float deltaTime)
    {
        duration -= deltaTime;
        Move(Vector3.zero, deltaTime);

        if (duration <= 0f)
            _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
    }

    public override void Exit() { }
}
