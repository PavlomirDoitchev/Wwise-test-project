using Assets.Scripts.Utilities.Contracts;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] Collider damageCollider;
       
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamagable damagable)) 
            {
                damagable.TakeDamage(damage);
            }
        }
        /// <summary>
        /// Used in animation events
        /// </summary>
        public void EnableCollider()
        {
            damageCollider.enabled = !damageCollider.enabled;
        }
    }
}
