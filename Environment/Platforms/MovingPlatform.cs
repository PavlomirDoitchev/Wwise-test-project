using UnityEngine;

namespace Assets.Scripts.Environment.Platforms
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] Transform[] waypoints;
        [SerializeField] float speed = 5f;
        [SerializeField] float arrivalThreshold = 0.1f;

        private int currentWaypointIndex = 0;
        private Vector3 lastPosition;

        // Per-frame delta position (world-space) for this frame. Use this on the player.
        public Vector3 PlatformDeltaPosition { get; private set; }

        // Per-second velocity (derived from PlatformDeltaPosition / Time.deltaTime)
        public Vector3 PlatformVelocity { get; private set; }

        private void Start()
        {
            if (waypoints.Length > 0)
                transform.position = waypoints[0].position;
            lastPosition = transform.position;
            PlatformDeltaPosition = Vector3.zero;
            PlatformVelocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (waypoints.Length == 0) return;

            Transform targetWaypoint = waypoints[currentWaypointIndex];
            float step = speed * Time.fixedDeltaTime;

            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

            // Move platform (physics timestep)
            transform.position = newPosition;

            // Waypoint arrival check (based on position after moving)
            if (Vector3.Distance(transform.position, targetWaypoint.position) < arrivalThreshold)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }

            // Note: we do NOT compute PlatformDeltaPosition/PlatformVelocity here,
            // because FixedUpdate runs on the physics timestep while the player
            // expects per-frame deltas. We compute the per-frame delta in LateUpdate.
        }

        // Compute the per-frame delta in LateUpdate so that it reflects the actual
        // displacement that happened this frame and can be applied to the CharacterController.
        private void LateUpdate()
        {
            // Platform delta since last frame (world-space)
            PlatformDeltaPosition = transform.position - lastPosition;

            float dt = Time.deltaTime > 0f ? Time.deltaTime : Time.fixedDeltaTime;
            PlatformVelocity = PlatformDeltaPosition / dt;

            lastPosition = transform.position;
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