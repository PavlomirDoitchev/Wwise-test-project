using Assets.Scripts.Utilities.Contracts;
using UnityEngine;

namespace Assets.Scripts.Environment.Platforms
{
    [RequireComponent(typeof(BoxCollider))]
    public class ClimbableSurface : MonoBehaviour, IClimable
    {
        [Header("Optional points (leave empty to auto-calc)")]
        [SerializeField] private Transform hangPoint;
        [SerializeField] private Transform standPoint;

        [Header("Offsets (used if no transforms are set)")]
        [SerializeField] private float hangOffsetY = 1.4f;
        [SerializeField] private float standOffsetZ = -0.4f;

        private BoxCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        public Vector3 GetHangPoint()
        {
            if (hangPoint != null) return hangPoint.position;

            Vector3 topCenter = _collider.bounds.center + Vector3.up * _collider.bounds.extents.y;
            return topCenter + transform.forward * -0.1f;
        }

        public Vector3 GetStandPoint()
        {
            if (standPoint != null) return standPoint.position;

            Vector3 topCenter = _collider.bounds.center + Vector3.up * _collider.bounds.extents.y;
            return topCenter + transform.forward * standOffsetZ;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(GetHangPoint(), 0.05f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(GetStandPoint(), 0.05f);
            Gizmos.color = new Color(0, 1, 0, 0.2f);
            if (TryGetComponent<BoxCollider>(out var col))
                Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        }
#endif
    }
}
