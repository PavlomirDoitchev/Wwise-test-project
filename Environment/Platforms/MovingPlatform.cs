using UnityEngine;

namespace Assets.Scripts.Environment.Platforms
{
    /// <summary>
    /// Kinematic moving platform using Rigidbody.MovePosition in FixedUpdate
    /// and exposing a per-frame PlatformDeltaPosition computed from the
    /// interpolated transform in LateUpdate.
    /// Add a Rigidbody to the platform GameObject and set isKinematic = true,
    /// interpolation = Interpolate.
    /// </summary>
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] Transform[] waypoints;
        [SerializeField] float speed = 5f;
        [SerializeField] float arrivalThreshold = 0.1f;

        private int currentWaypointIndex = 0;
        private Rigidbody rb;

        // Physics-step position (used for MovePosition)
        private Vector3 previousFixedPosition;

        // Render-step position for per-frame delta
        private Vector3 lastRenderPosition;

        // Per-frame displacement to be applied to the player (in world space)
        public Vector3 PlatformDeltaPosition { get; private set; }

        // Optional: velocity derived from the per-frame delta
        public Vector3 PlatformVelocity { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("MovingPlatform: Add a Rigidbody and set isKinematic = true.");
            }
            else
            {
                rb.isKinematic = true;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }

        private void Start()
        {
            if (waypoints != null && waypoints.Length > 0 && waypoints[0] != null)
            {
                if (rb != null)
                    rb.position = waypoints[0].position;
                else
                    transform.position = waypoints[0].position;
            }

            previousFixedPosition = transform.position;
            lastRenderPosition = transform.position;
            PlatformDeltaPosition = Vector3.zero;
            PlatformVelocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (waypoints == null || waypoints.Length == 0 || waypoints[currentWaypointIndex] == null) return;

            float step = speed * Time.fixedDeltaTime;
            Vector3 currentPos = rb != null ? rb.position : transform.position;
            Vector3 target = waypoints[currentWaypointIndex].position;
            Vector3 newPos = Vector3.MoveTowards(currentPos, target, step);

            if (rb != null)
                rb.MovePosition(newPos);
            else
                transform.position = newPos;

            PlatformVelocity = (newPos - previousFixedPosition) / Time.fixedDeltaTime;

            if (Vector3.Distance(newPos, target) < arrivalThreshold)
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

            previousFixedPosition = newPos;
        }

        // Read the interpolated transform.position so the visual motion is smooth,
        // and compute the per-frame delta for the player to apply.
        private void LateUpdate()
        {
            PlatformDeltaPosition = transform.position - lastRenderPosition;

            float dt = Time.deltaTime > 0f ? Time.deltaTime : Time.fixedDeltaTime;
            PlatformVelocity = (dt > 0f) ? (PlatformDeltaPosition / dt) : Vector3.zero;

            lastRenderPosition = transform.position;
        }

        private void OnDrawGizmos()
        {
            if (waypoints == null || waypoints.Length == 0) return;

            Gizmos.color = Color.yellow;
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null) continue;

                Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);

                int nextIndex = (i + 1) % waypoints.Length;
                if (waypoints[nextIndex] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
                }
            }
        }
    }
}