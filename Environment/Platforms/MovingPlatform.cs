using UnityEngine;

namespace Assets.Scripts.Environment.Platforms
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] Transform[] waypoints;
        [SerializeField] float speed = 5f;

        private int currentWaypointIndex = 0;
        private Vector3 lastPosition;
        private Vector3 platformVelocity;

        public Vector3 PlatformVelocity => platformVelocity;

        private void Start()
        {
            if (waypoints.Length > 0)
                transform.position = waypoints[0].position;

            lastPosition = transform.position;
        }

        private void Update()
        {
            if (waypoints.Length == 0) return;

            Transform targetWaypoint = waypoints[currentWaypointIndex];
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, step);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }

            platformVelocity = (transform.position - lastPosition) / Time.deltaTime;
            lastPosition = transform.position;
        }
    }
}
