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
        /// Standard locomotion movement. Sets speed to .5 for walk, 1 for sprint.
        /// </summary>
        protected void PlayerMove(float deltaTime)
        {
            Vector2 input = _playerStateMachine.InputManager.MovementInput();
            bool isSprinting = _playerStateMachine.InputManager.PlayerSprintInput();

            float baseSpeed = 7.5f;

            float speedMultiplier = isSprinting ? 1.3f : 1f;
            Vector2 filteredInput = GetFilteredMovementInput();
            Vector3 movement = new Vector3(filteredInput.x, 0f, 0f) * baseSpeed * speedMultiplier;

            // Move the character
            Move(movement, deltaTime);
            HandleFlip(filteredInput.x);

            // Update animator locomotion
            float locomotionValue = 0f;
            if (filteredInput != Vector2.zero)
                locomotionValue = isSprinting ? 1f : 0.5f;

            _playerStateMachine.Animator.SetFloat("Locomotion", locomotionValue, 0.01f, deltaTime);
            
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
    }
}