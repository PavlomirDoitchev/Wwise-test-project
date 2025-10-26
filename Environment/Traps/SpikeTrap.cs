using Assets.Scripts.Utilities.Contracts;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps
{
    public class SpikeTrap : Trap
    {
        // TODO: Create a new layer for traps and trap triggers. Make sure they cannot interact with each other!
        [Tooltip("If true, animation will loop, else trggier upon something landing on trap")]
        [SerializeField] bool isAutomatic;
        [SerializeField] float timeToActivate = 1f;
        [SerializeField] private float timer; 
        [SerializeField] private BoxCollider triggerCollider;
        private Animator animator;
        private readonly int Idle = Animator.StringToHash("Idle");
        private readonly int Trigger = Animator.StringToHash("TFD_Floor_Trap_01A");
        bool hasEnteredTrigger = false;
        private void Start()
        {
            if (triggerCollider == null)
                triggerCollider = GetComponentInChildren<BoxCollider>();
            animator = GetComponent<Animator>();
            if (isAutomatic)
                animator.Play(Trigger);
            else
                animator.Play(Idle);

            timer = timeToActivate;

        }
        private void OnTriggerEnter(Collider other)
        {
            if (isAutomatic) return;
            if (other == triggerCollider) return;
            if (other.TryGetComponent<IDamagable>(out var _))
                hasEnteredTrigger = true;
        }
        private void Update()
        {
            if (hasEnteredTrigger && !isAutomatic)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    animator.Play(Trigger);
                    hasEnteredTrigger = false;
                }
            }
        }
     
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IDamagable>(out var damagable) && !isAutomatic)
            {
                hasEnteredTrigger = false;
                timer = timeToActivate; 
            }

        }

        public void EnableDamageCollider() => triggerCollider.enabled = true;
        public void DisableDamageCollider() => triggerCollider.enabled = false;
    }
}
