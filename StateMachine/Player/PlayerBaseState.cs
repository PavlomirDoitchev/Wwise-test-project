using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;

namespace Assets.Scripts.StateMachine.Player
{
    /// <summary>
    /// Base class for all player states, handling movement and rotation.
    /// </summary>
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine _playerStateMachine;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            _playerStateMachine = stateMachine;
        }

        #region Movement

        /// <summary>
        /// Moves the player including applied forces from ForceReceiver.
        /// </summary>
        protected void Move(Vector3 movement, float deltaTime)
        {
            _playerStateMachine.CharacterController.Move((movement + _playerStateMachine.ForceReceiver.Movement) * deltaTime);

            Vector3 pos = _playerStateMachine.transform.position;
            pos.z = 0f;
            _playerStateMachine.transform.position = pos;
        }

        /// <summary>
        /// Moves only by forces (used when no input movement).
        /// </summary>
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        /// <summary>
        /// Standard locomotion movement. Sets speed to .5 for running, 1 for sprint.
        /// </summary>
        protected void PlayerMove(float deltaTime)
        {
            Vector2 input = _playerStateMachine.InputManager.MovementInput();
            bool isSprinting = _playerStateMachine.InputManager.SprintInput();

            float baseSpeed = _playerStateMachine.PlayerStats.BaseSpeed;

            float speedMultiplier = isSprinting ? 1.3f : 1f;
            Vector2 filteredInput = GetFilteredMovementInput();
            Vector3 movement = new Vector3(filteredInput.x, 0f, 0f) * baseSpeed * speedMultiplier;

            Move(movement, deltaTime);
            HandleFlip(filteredInput.x);

            float locomotionValue = 0f;
            if (filteredInput != Vector2.zero)
                locomotionValue = isSprinting ? 1f : 0.5f;

            _playerStateMachine.Animator.SetFloat("Locomotion", locomotionValue, 0.01f, deltaTime);

        }
        protected void PlayerMoveAirborne(float deltaTime)
        {
            //Vector2 input = _playerStateMachine.InputManager.MovementInput();

            //float baseSpeed = _playerStateMachine.PlayerStats.BaseSpeed;

            //Vector2 filteredInput = GetFilteredMovementInput();
            //Vector3 movement = new Vector3(filteredInput.x, 0f, 0f) * baseSpeed;

            //Move(movement, deltaTime);
            //HandleFlip(filteredInput.x);
            Vector2 input = _playerStateMachine.InputManager.MovementInput();
            bool isSprinting = _playerStateMachine.InputManager.SprintInput();

            float baseSpeed = _playerStateMachine.PlayerStats.BaseSpeed;
            float maxSpeed = isSprinting ? baseSpeed * 1.3f : baseSpeed;
            float acceleration = _playerStateMachine.PlayerStats.AirAcceleration; 

            Vector2 filteredInput = GetFilteredMovementInput();
            Vector3 moveDir = new Vector3(filteredInput.x, 0f, 0f).normalized;

            Vector3 currentVelocity = new Vector3(
                _playerStateMachine.ForceReceiver.Movement.x,
                0f,
                0f
            );

            Vector3 targetVelocity = moveDir * maxSpeed;

            Vector3 newVelocity = Vector3.MoveTowards(
                currentVelocity,
                targetVelocity,
                acceleration * deltaTime
            );

            _playerStateMachine.ForceReceiver.SetForce(new Vector3(newVelocity.x, 0f, 0f));

            Move(newVelocity, deltaTime);
            HandleFlip(filteredInput.x);

        }
        /// <summary>
        /// Movement used during attacks to preserve rotation/flip without modifying speed.
        /// </summary>
        protected void RotateDuringAttack(float deltaTime)
        {
            Move(deltaTime);
            HandleFlip(_playerStateMachine.InputManager.MovementInput().x);
        }

        /// <summary>
        /// Calculates horizontal movement vector based on input.
        /// </summary>
        private Vector3 CalculateHorizontalMovement()
        {

            float horizontal = _playerStateMachine.InputManager.MovementInput().x;
            return new Vector3(horizontal, 0f, 0f);
        }
        protected Vector2 GetFilteredMovementInput()
        {
            Vector2 input = _playerStateMachine.InputManager.MovementInput();

            // Only block if the blocker still exists
            if (_playerStateMachine.currentLeftBlocker != null && input.x < 0) input.x = 0f;
            if (_playerStateMachine.currentRightBlocker != null && input.x > 0) input.x = 0f;

            return input;
        }
        protected Vector3 GetHorizontalMomentum()
        {
            Vector3 velocity = _playerStateMachine.CharacterController.velocity;
            velocity.y = 0f;
            return velocity;
        }
        #endregion

        #region Rotation

        /// <summary>
        /// Flips player to face the direction of horizontal input.
        /// </summary>
        protected void HandleFlip(float horizontalInput)
        {
            if (horizontalInput != 0f)
            {
                float yRotation = horizontalInput > 0 ? 90f : -90f;
                _playerStateMachine.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            }
        }

        #endregion

        #region State Methods
        /// <summary>
        /// Checks if the player is grounded using CharacterController's isGrounded property.
        /// </summary>
        /// <returns></returns>
        protected bool IsGrounded() 
        {
            return _playerStateMachine.CharacterController.isGrounded;
        }      
        protected bool CheckGrounded()
        {
            return _playerStateMachine.CharacterController.isGrounded;
        }
        protected void HandleLanding()
        {
            if (_playerStateMachine.InputManager.MoveInput.x == 0)
            {
                _playerStateMachine.ChangeState(
                    new PlayerLandingState(_playerStateMachine, 0.7f)
                );
            }
            else
            {
                _playerStateMachine.ChangeState(
                    new PlayerLocomotionState(_playerStateMachine)
                );
            }
        }
        protected void DoJump()
        {
            if (_playerStateMachine.InputManager.JumpInput() && _playerStateMachine.CharacterController.isGrounded)
            {
                AkUnitySoundEngine.PostEvent("Play_Jump", _playerStateMachine.gameObject);

                Vector3 momentum = new Vector3(
                    _playerStateMachine.CharacterController.velocity.x,
                    0f,
                    0f
                );

                _playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine, momentum));
            }
        }
        protected void DoAttack()
        {
            if (_playerStateMachine.InputManager.AttackInput())
            {
                _playerStateMachine.ChangeState(new PlayerAttackState(_playerStateMachine));
            }
        }
        protected bool DoAirborneAttack()
        {
            if (_playerStateMachine.InputManager.AttackInput())
            {
                Vector3 currentMomentum = GetHorizontalMomentum();
                _playerStateMachine.ChangeState(new PlayerAirborneAttackState(_playerStateMachine, currentMomentum));
                return true;
            }
            return false;
        }
        protected void DropAttack()
        {
            if (_playerStateMachine.InputManager.DropAttackInput())
            {
                _playerStateMachine.ChangeState(new PlayerDropAttackState(_playerStateMachine));
            }
        }

        #endregion
    }
}