using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

public class PlayerStoppingState : PlayerBaseState
{
    private readonly float _duration = 0.2f;
    private float _elapsedTime;

    public PlayerStoppingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        _playerStateMachine.Animator.CrossFadeInFixedTime("RunEnd", 0.1f);
        _elapsedTime = 0f;
    }

    public override void Tick(float deltaTime)
    {
        _elapsedTime += deltaTime;

        Vector3 velocity = _playerStateMachine.ForceReceiver.Movement;
        velocity.x = Mathf.MoveTowards(velocity.x, 0f, deltaTime * 20f);
        _playerStateMachine.ForceReceiver.SetForce(velocity);

        Move(deltaTime);
        if (_elapsedTime >= _duration)
        {
            _playerStateMachine.ChangeState(new PlayerLocomotionState(_playerStateMachine));
        }
    }

    public override void Exit() { }
}
