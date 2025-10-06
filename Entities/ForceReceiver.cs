using Assets.Scripts.StateMachine.Player;
using UnityEngine;
using UnityEngine.AI;

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
        public Vector3 Movement => impact + Vector3.up * verticalVelocity;
        private void Awake()
        {

            verticalVelocity = 0f;
            impact = Vector3.zero;
        }
        private void Update()
        {
            if (verticalVelocity < 0f && characterController.isGrounded)
            {
                verticalVelocity = 0f;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * fallMultiplier * Time.deltaTime;
                verticalVelocity *= (1 - inAirDrag);
                verticalVelocity = Mathf.Max(verticalVelocity, maxFallSpeed);
            }

          

            impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, dragTime);

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
            verticalVelocity = Mathf.Max(verticalVelocity, 0f);
            verticalVelocity += jumpForce;
        }
        public void KnockUp(float force)
        {
            verticalVelocity = force;
        }
        public void KnockDown(float force)
        {
            verticalVelocity = -force;
        }
        public void ResetForces()
        {
            impact = Vector3.zero;
            verticalVelocity = 0f;
            dampingVelocity = Vector3.zero;
        }
        public void ResetVertical() 
        {
            verticalVelocity = 0f;
        }
    }
}
