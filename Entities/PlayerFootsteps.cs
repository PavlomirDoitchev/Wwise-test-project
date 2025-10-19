using UnityEngine;
using Assets.Scripts.StateMachine.Player;
using Assets.Scripts.StateMachine.Player.States;

namespace Assets.Scripts.Entities
{
    public class PlayerFootsteps : MonoBehaviour
    {
        [SerializeField] private string footstepEventName = "Play_Footstep";
        private float stepDistance = 2.5f;
        [SerializeField] private LayerMask groundMask;

        private PlayerStateMachine _playerStateMachine;
        private Vector3 _lastPosition;
        private float _distanceTraveled;
        private string _currentSurface = "Dirt"; 

        private void Awake()
        {
            _playerStateMachine = GetComponentInParent<PlayerStateMachine>();
            _lastPosition = transform.position;
        }

        private void Update()
        {
            HandleFootsteps();
        }

        private void HandleFootsteps()
        {
            UpdateSurface();

            if (!_playerStateMachine.CharacterController.isGrounded || _playerStateMachine.CurrentState is not PlayerLocomotionState)
                return;

            Vector3 currentPosition = transform.position;
            _distanceTraveled += Vector3.Distance(_lastPosition, currentPosition);

            float currentStepDistance = stepDistance;
            if (_playerStateMachine.InputManager.SprintInput())
            {
                stepDistance = 2f;
                currentStepDistance *= 1.1f;

            }
            else 
            {
                stepDistance = 2.5f;
            }


            if (_distanceTraveled >= currentStepDistance)
            {
                AkUnitySoundEngine.SetSwitch("Material", _currentSurface, gameObject);
                AkUnitySoundEngine.PostEvent(footstepEventName, gameObject);

                _distanceTraveled = 0f;
            }

            _lastPosition = currentPosition;
        }

        private void UpdateSurface()
        {
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 1f, groundMask))
            {
                if (hit.collider.CompareTag("Stone"))
                    _currentSurface = "Stone";
                else if (hit.collider.CompareTag("Dirt"))
                    _currentSurface = "Dirt";
                else
                    _currentSurface = "Dirt";
            }
        }
    }
}
