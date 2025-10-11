using Assets.Scripts.Entities;
using Assets.Scripts.StateMachine.Player.States;
using Assets.Scripts.Utilities.Contracts;
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
        public INotPushable currentLeftBlocker = null;
        public INotPushable currentRightBlocker = null;

        public LayerMask groundMask;
        public float probeDistance = 0.1f;
        [SerializeField] private int minProbesRequired = 2;
        public Transform[] groundProbes;
        public Collider currentGroundCollider = null;

        [SerializeField] private float blockerCheckHeight = 1.8f;
        [SerializeField] private int blockerCheckRays = 3;
        [SerializeField] private float blockerCheckDistance = 0.6f;

        public enum WallSide
        {
            None,
            Left,
            Right
        }

        private void Awake()
        {
            ComboCooldown = new Cooldown(comboTimeout);
        }
        private void Start()
        {
            ChangeState(new PlayerLocomotionState(this));
        }
        
        private void FixedUpdate()
        {
            if (currentLeftBlocker != null)
            {
                bool stillBlocked = false;
                for (int i = 0; i < blockerCheckRays; i++)
                {
                    float t = i / (float)(blockerCheckRays - 1);
                    Vector3 origin = transform.position + Vector3.up * (t * blockerCheckHeight);
                    if (Physics.Raycast(origin, Vector3.left, blockerCheckDistance))
                    {
                        stillBlocked = true;
                        break;
                    }
                }

                if (!stillBlocked)
                    currentLeftBlocker = null;
            }

            if (currentRightBlocker != null)
            {
                bool stillBlocked = false;
                for (int i = 0; i < blockerCheckRays; i++)
                {
                    float t = i / (float)(blockerCheckRays - 1);
                    Vector3 origin = transform.position + Vector3.up * (t * blockerCheckHeight);
                    if (Physics.Raycast(origin, Vector3.right, blockerCheckDistance))
                    {
                        stillBlocked = true;
                        break;
                    }
                }

                if (!stillBlocked)
                    currentRightBlocker = null;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (CurrentState is PlayerBaseState playerState)
            {
                playerState.OnControllerColliderHit(hit);
            }

            if (hit.gameObject.TryGetComponent<IPushable>(out _)) return;

            if (hit.gameObject.TryGetComponent<INotPushable>(out INotPushable notPushable))
            {
                if (Mathf.Abs(hit.normal.y) < 0.1f)
                {
                    if (hit.normal.x > 0) currentLeftBlocker = notPushable;
                    if (hit.normal.x < 0) currentRightBlocker = notPushable;
                }
            }
        }
        public bool IsTouchingWall => GetWallContact() != WallSide.None;
        public WallSide GetWallContact()
        {
            if (currentLeftBlocker != null)
                return WallSide.Left;
            if (currentRightBlocker != null)
                return WallSide.Right;
            return WallSide.None;
        }
        public bool IsSupported()
        {
            int hits = 0;
            foreach (var probe in groundProbes)
            {
                if (Physics.Raycast(probe.position, Vector3.down, probeDistance, groundMask))
                    hits++;
            }
            return hits >= minProbesRequired;
        }
        private void OnDrawGizmos()
        {
            if (groundProbes == null) return;
            Gizmos.color = Color.red;
            foreach (var probe in groundProbes)
            {
                Gizmos.DrawLine(probe.position, probe.position + Vector3.down * probeDistance);
            }
        }
    }
}