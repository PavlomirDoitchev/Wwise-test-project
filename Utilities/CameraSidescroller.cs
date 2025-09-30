using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
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

    [Header("Look-Ahead")]
    [SerializeField] private float lookAheadDistance = 2f;
    [SerializeField] private float lookAheadSmooth = 5f;

    [Header("Tilt")]
    [SerializeField] private float tiltX = 15f; 

    private Camera cam;
    private float currentDistance;
    private Vector3 cameraTargetPos;

    private float currentLookAheadX = 0f;
    private float targetLookAheadX = 0f;

    private InputManager inputManager;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        currentDistance = maxDistance;
        cameraTargetPos = transform.position;

        inputManager = FindFirstObjectByType<InputManager>();
    }

    private void LateUpdate()
    {
        if (focus == null) return;

        HandleZoom();
        HandleLookAhead();

        Vector3 focusPoint = focus.position + focusOffset + new Vector3(currentLookAheadX, 0f, 0f);

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
            cameraTargetPos += new Vector3(worldOffset.x, worldOffset.y, 0f);
        }

        Vector3 desiredPos = new Vector3(cameraTargetPos.x, cameraTargetPos.y, focusPoint.z - currentDistance);

        cameraTargetPos = Vector3.Lerp(cameraTargetPos, desiredPos, cameraFollowSpeed * Time.deltaTime);

        Vector3 posShake = ImpulseManager.GetPositionShake();

        transform.position = cameraTargetPos + posShake;

        transform.rotation = Quaternion.Euler(tiltX, 0f, 0f);
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

    private void HandleLookAhead()
    {
        if (inputManager == null) return;

        float horizontal = inputManager.MovementInput().x;
        targetLookAheadX = horizontal * lookAheadDistance;

        currentLookAheadX = Mathf.Lerp(
            currentLookAheadX,
            targetLookAheadX,
            Time.deltaTime * lookAheadSmooth
        );
    }
}
