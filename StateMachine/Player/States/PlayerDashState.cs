using Assets.Scripts.StateMachine.Player.States;
using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.Utilities.Contracts;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float _duration;
    private float _dashDuration;
    private bool shouldGoInOppositeDir;
    private float normalizedTime;
    private float speedMultiplier;
    private Vector3 _dashDirection;
    public PlayerDashState(PlayerStateMachine stateMachine, float duration) : base(stateMachine)
    {
        this._duration = duration;
    }
    public PlayerDashState(PlayerStateMachine stateMachine, float duration, bool direction) : base(stateMachine)
    {
        this._duration = duration;
        this.shouldGoInOppositeDir = direction;
    }
    public override void Enter()
    {
        _playerStateMachine.Animator.CrossFadeInFixedTime("Dash", 0.1f);

    }

    public override void Tick(float deltaTime)
    {
        _dashDuration += deltaTime / _duration;
        normalizedTime = Mathf.Clamp01(_dashDuration);
        speedMultiplier = _playerStateMachine.slideSpeedCurve.Evaluate(normalizedTime);
        _playerStateMachine.ForceReceiver.verticalVelocity = 0f;

        if (shouldGoInOppositeDir)
        {
            _playerStateMachine.transform.rotation = Quaternion.LookRotation(-_playerStateMachine.transform.forward);
            _dashDirection = -_playerStateMachine.transform.forward;
        }
        else
            _dashDirection = _playerStateMachine.transform.forward;


        Vector3 move = _dashDirection * _playerStateMachine.PlayerStats.DashForce * speedMultiplier;
        _playerStateMachine.CharacterController.Move(move * deltaTime);
        if (_dashDuration >= 1f)
            _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));

    }

    public override void Exit()
    {
        var currentImpact = _playerStateMachine.ForceReceiver.Movement;
        currentImpact.y = 0f;

        _playerStateMachine.ForceReceiver.SetForce(currentImpact * 0.25f);
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
}
