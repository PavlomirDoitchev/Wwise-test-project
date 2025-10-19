using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

public class PlayerCollisionState : PlayerBaseState
{
    private float duration;
    Vector3 impulse = Vector3.one;


    public PlayerCollisionState(PlayerStateMachine stateMachine, float duration)
        : base(stateMachine)
    {
        this.duration = duration;
    }

    public override void Enter()
    {
        _playerStateMachine.Animator.CrossFadeInFixedTime("Collision", 0.1f);
        _playerStateMachine.ForceReceiver.SetForce(_playerStateMachine.gameObject.transform.forward * -10f);
        ImpulseManager.GenerateImpulse(new Vector3(0.1f, 0.1f, 0f), new Vector3(1f, 1f, 0f), duration);
    }

    public override void Tick(float deltaTime)
    {
        duration -= deltaTime;
        Move(Vector3.zero, deltaTime);

        if (duration <= 0f)
            _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
    }

    public override void Exit() { }
}
