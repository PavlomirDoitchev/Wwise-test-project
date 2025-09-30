using Assets.Scripts.Entities;
using Assets.Scripts.StateMachine.Player.States;
using UnityEngine;
namespace Assets.Scripts.StateMachine.Player
{
    public class PlayerStateMachine : StateMachine
    {
        [field: SerializeField] public CharacterController CharacterController { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
        [field: SerializeField] public InputManager InputManager { get; private set; }
        [field: SerializeField] public PlayerStats PlayerStats { get; private set; }
        public Cooldown ComboCooldown { get; private set; }
        [SerializeField] float comboTimeout = 2f;
        public int ComboIndex { get; set; } = 0;

        private void Awake()
        {
            ComboCooldown = new Cooldown(comboTimeout);
        }
        private void Start()
        {
            ChangeState(new PlayerLocomotionState(this));
        }
    }
}
