using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class ForceReceiver : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private float dragTime = 0.1f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private float inAirDrag = .05f;
        [SerializeField] private float maxFallSpeed = -100f;

        public float verticalVelocity;
        private Vector3 impact;
        private Vector3 dampingVelocity;

        private float wallSlideTimer = 0f;
        private const float wallSlideRampDuration = 2f;

        // Platform state
        private bool onPlatform = false;
        public bool OnPlatform => onPlatform;

        public Vector3 Movement => impact + Vector3.up * verticalVelocity;

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if (onPlatform && characterController.isGrounded)
            {
                // Freeze forces while on platform: no gravity integration, no residual impact.
                verticalVelocity = 0f;
                impact = Vector3.zero;
                dampingVelocity = Vector3.zero;
                wallSlideTimer = 0f;
                return;
            }

            if (verticalVelocity < 0f && characterController.isGrounded)
            {
                verticalVelocity = 0f;
                wallSlideTimer = 0f;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * fallMultiplier * dt;
                verticalVelocity *= (1f - inAirDrag);
                verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed);
            }

            // SmoothDamp with explicit deltaTime to match fixed update
            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, dragTime, Mathf.Infinity, dt);
        }

        public void EnterPlatform()
        {
            onPlatform = true;
            // clear residual forces so we don't get unexpected motion
            impact = Vector3.zero;
            dampingVelocity = Vector3.zero;
            verticalVelocity = 0f;
            wallSlideTimer = 0f;
        }

        public void ExitPlatform()
        {
            onPlatform = false;
            // keep forces cleared on exit; vertical will start to integrate again in FixedUpdate
        }

        public void ApplyWallSlideGravity(float slideMultiplier = 0.85f, float maxSlideSpeed = -12f)
        {
            wallSlideTimer += Time.deltaTime;
            float rampFactor = Mathf.Clamp01(wallSlideTimer / wallSlideRampDuration);

            verticalVelocity += Physics.gravity.y * fallMultiplier * slideMultiplier * rampFactor * Time.deltaTime;

            verticalVelocity = Mathf.Max(verticalVelocity, maxSlideSpeed);
        }

        public void ResetForces()
        {
            impact = Vector3.zero;
            verticalVelocity = 0f;
            dampingVelocity = Vector3.zero;
            wallSlideTimer = 0f;
        }

        public void ResetVertical()
        {
            verticalVelocity = 0f;
            wallSlideTimer = 0f;
        }

        public void AddForce(Vector3 force)
        {
            impact += force;
        }
        public void SetForce(Vector3 force)
        {
            impact = force;
        }
        public void Jump(float jumpForce)
        {
            // when jump happens we should exit platform mode so gravity & vertical motion works
            ExitPlatform();
            verticalVelocity = Mathf.Max(verticalVelocity, 0f);
            verticalVelocity += jumpForce;
        }
        public void JumpTo(float jumpForce, Vector3 jumpDirection)
        {
            ExitPlatform();
            verticalVelocity = Mathf.Max(verticalVelocity, 0f);
            impact += jumpDirection;
            verticalVelocity += jumpForce;
        }
        public void KnockUp(float force)
        {
            ExitPlatform();
            verticalVelocity = force;
        }
        public void KnockDown(float force)
        {
            ExitPlatform();
            verticalVelocity = -force;
        }
    }
}