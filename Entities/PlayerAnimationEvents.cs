using Assets.Scripts.StateMachine.Player;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        [SerializeField] private string footstepEventName = "Play_Footstep";
        private PlayerStateMachine _playerStateMachine;
        private void Awake()
        {
            _playerStateMachine = GetComponentInParent<PlayerStateMachine>();
        }

        public void PlayFootstep()
        {
            if (_playerStateMachine.InputManager.MovementInput().sqrMagnitude > 0.5f &&
                _playerStateMachine.IsSupported())
            {
                AkUnitySoundEngine.PostEvent(footstepEventName, gameObject);
            }
        }
    }
}
