using Assets.Scripts.Utilities.Contracts;
using UnityEngine;

namespace Assets.Scripts.Environment.Traps
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] int damage;
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamagable damagable)) 
            {
                damagable.TakeDamage(damage);
            }
        }
    }
}
