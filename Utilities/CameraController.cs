using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Transform focus;                  
    [SerializeField] private Vector3 focusOffset = new Vector3(0, 1.5f, 0); 
    [SerializeField, Range(1f, 20f)] private float minDistance = 2f;
    [SerializeField, Range(1f, 20f)] private float maxDistance = 10f;
    [SerializeField] private float zoomSpeed = 5f;

    private Camera cam;
    private float currentDistance;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        currentDistance = maxDistance;
    }

    private void LateUpdate()
    {
        if (focus == null) return;

        float scrollInput = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            currentDistance = Mathf.Clamp(
                currentDistance - scrollInput * zoomSpeed * Time.deltaTime,
                minDistance,
                maxDistance
            );
        }

        Vector3 focusPoint = focus.position + focusOffset;

        // Impulses (camera shake)
        Vector3 posShake = ImpulseManager.GetPositionShake();
        Vector3 rotShake = ImpulseManager.GetRotationShake();

        transform.position = focusPoint - transform.forward * currentDistance + posShake;
        transform.LookAt(focusPoint);
        transform.rotation *= Quaternion.Euler(rotShake);
    }
}
