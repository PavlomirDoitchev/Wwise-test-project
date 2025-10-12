using Assets.Scripts.StateMachine.Player.States;
using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.Utilities.Contracts;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    private float duration;
    //private float lastGroundedTime;
    //private const float groundedBuffer = 0.15f; 
    private bool shouldGoInOppositeDir;

    public PlayerDashState(PlayerStateMachine stateMachine, float duration) : base(stateMachine)
    {
        this.duration = duration;
    }
    public PlayerDashState(PlayerStateMachine stateMachine, float duration, bool direction) : base(stateMachine)
    {
        this.duration = duration;
        this.shouldGoInOppositeDir = direction;
    }
    public override void Enter()
    {
        //if (_playerStateMachine.CharacterController.isGrounded)
        //    lastGroundedTime = Time.time;

        //bool groundedBuffered = Time.time - lastGroundedTime <= groundedBuffer;

        //if (groundedBuffered)
        //    _playerStateMachine.Animator.CrossFadeInFixedTime("Slide", 0.1f);
        //else
        _playerStateMachine.Animator.CrossFadeInFixedTime("Dash", 0.1f);
        if (shouldGoInOppositeDir)
        {
            _playerStateMachine.ForceReceiver.SetForce(-_playerStateMachine.transform.forward * _playerStateMachine.PlayerStats.DashForce);
            _playerStateMachine.transform.rotation = Quaternion.LookRotation(-_playerStateMachine.transform.forward);
        }
        else
        {
            _playerStateMachine.ForceReceiver
                .SetForce(_playerStateMachine.transform.forward * _playerStateMachine.PlayerStats.DashForce);
        }
    }

    public override void Tick(float deltaTime)
    {
        //if (_playerStateMachine.CharacterController.isGrounded)
        //    lastGroundedTime = Time.time;
        _playerStateMachine.ForceReceiver.verticalVelocity = 0f;

        duration -= deltaTime;
        if (duration <= 0f)
        {
            _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
            return;
        }

        Move(deltaTime);
    }

    public override void Exit()
    {
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
}
