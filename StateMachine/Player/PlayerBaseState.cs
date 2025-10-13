using Assets.Scripts.State_Machine.Player_State_Machine;
using Assets.Scripts.StateMachine.Player.States;
using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Assets.Scripts.StateMachine.Player
{
    /// <summary>
    /// Base class for all player states, handling movement and rotation.
    /// </summary>
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine _playerStateMachine;
        protected float _lastInputX = 0f;
        protected bool _isTurning = false;
        protected bool _isSprinting = false;
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
            _isSprinting = _playerStateMachine.InputManager.SprintInput();

            float baseSpeed = _playerStateMachine.PlayerStats.BaseSpeed;
            float speedMultiplier = _isSprinting ? 1.3f : 1f;

            Vector2 filteredInput = GetFilteredMovementInput();
            Vector3 targetMovement = new Vector3(filteredInput.x, 0f, 0f) * baseSpeed * speedMultiplier;

            // Smooth current movement towards targetMovement
            float acceleration = _playerStateMachine.PlayerStats.GroundAcceleration; 
            _playerStateMachine.CurrentVelocity = Vector3.MoveTowards(
                _playerStateMachine.CurrentVelocity,
                targetMovement,
                acceleration * deltaTime
            );

            Move(_playerStateMachine.CurrentVelocity, deltaTime);
            HandleFlip(filteredInput.x);

            float locomotionValue = 0f;
            if (filteredInput != Vector2.zero)
                locomotionValue = _isSprinting ? 1f : 0.5f;

            _playerStateMachine.Animator.SetFloat("Locomotion", locomotionValue, 0.05f, deltaTime);
        }


        protected void PlayerMoveAirborne(float deltaTime, Vector3 scaledInput)
        {
            Vector3 move = scaledInput * 2;
            Move((move + _playerStateMachine.ForceReceiver.Movement) * deltaTime, deltaTime);
        }
        protected void PlayerMoveAirborne(float deltaTime)
        {

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

            if (_playerStateMachine.currentLeftBlocker != null && input.x < 0) input.x = 0f;
            if (_playerStateMachine.currentRightBlocker != null && input.x > 0) input.x = 0f;

            return input;
        }
        protected Vector3 GetHorizontalMomentum()
        {
            //Vector3 velocity = _playerStateMachine.CharacterController.velocity;
            Vector3 velocity = _playerStateMachine.ForceReceiver.Movement;
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
        protected void DoCrouch() 
        {
            var input = _playerStateMachine.InputManager;
            if (input.CrouchInput())
            {
                _playerStateMachine.ChangeState(new PlayerCrouchState(_playerStateMachine));
            }
        }
        protected void DoDash()
        {
            var input = _playerStateMachine.InputManager;
            if (input.DashInput())
            {
                _playerStateMachine.ChangeState(new PlayerDashState(_playerStateMachine, 0.2f));
            }
        }
        protected void DoWallDash()
        {
            var input = _playerStateMachine.InputManager;
            if (input.DashInput())
            {
                _playerStateMachine.ChangeState(new PlayerDashState(_playerStateMachine, 0.2f, true));
            }
        }
        protected void DoSlide()
        {
            var input = _playerStateMachine.InputManager;
            if (input.SlideInput())
            {
                _playerStateMachine.ChangeState(new PlayerSlideState(_playerStateMachine, 0.2f));
            }
        }
        protected void DoJump()
        {
            var input = _playerStateMachine.InputManager;

            if (input.JumpInput() && _playerStateMachine.CharacterController.isGrounded)
            {
                string surface = DetectSurface();

                AkUnitySoundEngine.SetSwitch("Jumping_Material", surface, _playerStateMachine.gameObject);

                AkUnitySoundEngine.PostEvent("Play_Jump", _playerStateMachine.gameObject);

                Vector3 momentum = new Vector3(
                    _playerStateMachine.CharacterController.velocity.x,
                    0f,
                    0f
                );

                _playerStateMachine.ChangeState(new PlayerJumpState(_playerStateMachine, momentum));
            }
        }
        protected bool CheckLedge(out Vector3 ledgeHangPoint, out Vector3 ledgeStandPoint)
        {
            ledgeHangPoint = Vector3.zero;
            ledgeStandPoint = Vector3.zero;

            Transform player = _playerStateMachine.transform;
            _playerStateMachine.origin = player.position + Vector3.up;
            _playerStateMachine.forward = player.forward;



            float forwardCheckDistance = 0.6f;
            float maxLedgeHeight = 1.5f; 
            float minLedgeHeight = 0.3f; 

            if (Physics.Raycast(_playerStateMachine.origin, _playerStateMachine.forward, out RaycastHit wallHit, forwardCheckDistance))
            {
                Vector3 ledgeCheckStart = wallHit.point + Vector3.up * maxLedgeHeight;

                if (Physics.Raycast(ledgeCheckStart, Vector3.down, out RaycastHit ledgeHit, maxLedgeHeight + 0.5f))
                {
                    float ledgeHeight = ledgeHit.point.y - player.position.y;

                    if (ledgeHeight > minLedgeHeight && ledgeHeight <= maxLedgeHeight)
                    {
                        Vector3 forwardOffset = wallHit.normal * -0.4f;
                        ledgeHangPoint = wallHit.point;
                        ledgeStandPoint = ledgeHit.point + forwardOffset;
                        return true;
                    }
                }
            }
            return false;
        }

        protected void DoAttack()
        {
            if (_playerStateMachine.InputManager.AttackInput())
            {
                _playerStateMachine.ChangeState(new PlayerAttackState(_playerStateMachine));
            }
        }
        protected void MeleeAttacks()
        {
            Vector2 moveInput = _playerStateMachine.InputManager.MovementInput();
            bool wantsToAttack = _playerStateMachine.InputManager.AttackInput();
            bool isSprinting = _playerStateMachine.InputManager.SprintInput();

            if (wantsToAttack)
            {
                if (isSprinting && moveInput != Vector2.zero)
                {
                    _playerStateMachine.ChangeState(new PlayerSprintAttackState(_playerStateMachine));
                    return;
                }
                else
                {
                    _playerStateMachine.ChangeState(new PlayerAttackState(_playerStateMachine));
                    return;
                }
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
        protected void Fall(float deltaTime)
        {
            if (_playerStateMachine.CharacterController.velocity.y <= -10f)
            {
                _playerStateMachine.ChangeState(new PlayerFallState(_playerStateMachine, GetHorizontalMomentum()));
            }
        }
        #endregion
        private string DetectSurface()
        {
            RaycastHit hit;
            if (Physics.Raycast(_playerStateMachine.transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 1.5f))
            {
                if (hit.collider.CompareTag("Stone"))
                    return "Stone";
                else if (hit.collider.CompareTag("Dirt"))
                    return "Dirt";
            }

            return "Dirt"; // Default fallback
        }
        public virtual void OnControllerColliderHit(ControllerColliderHit hit) { }
        
    }

}