using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraCinematic : MonoBehaviour
{
    [Header("Focus")]
    [SerializeField] private Transform focus;
    [SerializeField] private Vector3 focusOffset = new Vector3(0, 1.5f, 0);

    [Header("Zoom")]
    [SerializeField, Range(1f, 20f)] private float minDistance = 2f;
    [SerializeField, Range(1f, 20f)] private float maxDistance = 10f;
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Dead Zone")]
    [SerializeField] private Vector2 deadZoneSize = new Vector2(2f, 1f);
    [SerializeField] private float cameraFollowSpeed = 5f; 

    private Camera cam;
    private float currentDistance;
    private Vector3 cameraTargetPos;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        currentDistance = maxDistance;
        cameraTargetPos = transform.position;
    }

    private void LateUpdate()
    {
        if (focus == null) return;

        HandleZoom();

        Vector3 focusPoint = focus.position + focusOffset;

        Vector3 localFocus = transform.InverseTransformPoint(focusPoint);

        float halfWidth = deadZoneSize.x / 2f;
        float halfHeight = deadZoneSize.y / 2f;

        bool outsideDeadZone = false;

        if (localFocus.x > halfWidth) { localFocus.x -= halfWidth; outsideDeadZone = true; }
        else if (localFocus.x < -halfWidth) { localFocus.x += halfWidth; outsideDeadZone = true; }
        else { localFocus.x = 0; }

        if (localFocus.y > halfHeight) { localFocus.y -= halfHeight; outsideDeadZone = true; }
        else if (localFocus.y < -halfHeight) { localFocus.y += halfHeight; outsideDeadZone = true; }
        else { localFocus.y = 0; }

        if (outsideDeadZone)
        {
            Vector3 worldOffset = transform.TransformVector(localFocus);
            cameraTargetPos += worldOffset;
        }

        Vector3 desiredPos = focusPoint - transform.forward * currentDistance;
        cameraTargetPos = Vector3.Lerp(cameraTargetPos, desiredPos, cameraFollowSpeed * Time.deltaTime);

        Vector3 posShake = ImpulseManager.GetPositionShake();
        Vector3 rotShake = ImpulseManager.GetRotationShake();

        transform.position = cameraTargetPos + posShake;
        transform.LookAt(focusPoint);
        transform.rotation *= Quaternion.Euler(rotShake);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            currentDistance = Mathf.Clamp(
                currentDistance - scrollInput * zoomSpeed * Time.deltaTime,
                minDistance,
                maxDistance
            );
        }
    }
}
