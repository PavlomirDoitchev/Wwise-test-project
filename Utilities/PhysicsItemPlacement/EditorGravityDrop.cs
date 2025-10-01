using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditorGravityDrop : MonoBehaviour
{
    private float dropDuration = 5f;
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private float timer;
    private bool dropping;

    private void OnEnable()
    {
        if (Application.isPlaying) return;

        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        timer = dropDuration;
        dropping = true;

        EditorApplication.delayCall += () =>
        {
            if (this == null) return;
            EditorApplication.update += SimulatePhysics;
        };
    }

    private void SimulatePhysics()
    {
        if (Application.isPlaying || !dropping) return;

        if (Physics.simulationMode != SimulationMode.Script)
            Physics.simulationMode = SimulationMode.Script;

        timer -= Time.fixedDeltaTime;
        Physics.Simulate(Time.fixedDeltaTime);

        if (timer <= 0f || IsGrounded())
        {
            dropping = false;

            if (rb != null)
                DestroyImmediate(rb);

            EditorApplication.update -= SimulatePhysics;
            Physics.simulationMode = SimulationMode.FixedUpdate;
            DestroyImmediate(this);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }
}