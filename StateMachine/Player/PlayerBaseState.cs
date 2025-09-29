using UnityEngine;
namespace Assets.Scripts.StateMachine.Player
{
    /// <summary>
    /// The base state for all player states.
    /// </summary>
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine _playerStateMachine;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this._playerStateMachine = stateMachine;
        }
        /// <summary>
        /// Preserve momentum. Used in PlayerMove().
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="deltaTime"></param>
        protected void Move(Vector3 movement, float deltaTime)
        {
            _playerStateMachine.CharacterController.Move((movement + _playerStateMachine.ForceReceiver.Movement) * deltaTime);
        }
        /// <summary>
        /// Apply physics. Sets Vector3 to 0.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }
        protected void PlayerMove(float deltaTime)
        {
            Vector3 movement = CalculateMovement();
            float multiplier = 10f;
            Move(movement * multiplier, deltaTime);

            HandleFlip(_playerStateMachine.InputManager.MovementInput().x);

            if (movement != Vector3.zero)
                _playerStateMachine.Animator.SetFloat("Locomotion", 1, 0.01f, deltaTime);
            else
                _playerStateMachine.Animator.SetFloat("Locomotion", 0, 0.1f, deltaTime);
        }
        /// <summary>
        /// Basic movement calculation
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateMovement()
        {
            float horizontal = _playerStateMachine.InputManager.MovementInput().x;

            Vector3 movement = new Vector3(horizontal, 0, 0);
            return movement;
        }
        protected void HandleFlip(float horizontalInput)
        {
            if (horizontalInput != 0)
            {
                float yRotation = horizontalInput > 0 ? 90f : -90f;

                _playerStateMachine.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            }
        }
    }
}
