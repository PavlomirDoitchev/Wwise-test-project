using Assets.Scripts.Utilities.Contracts;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class ImmovableObstacle : MonoBehaviour, INotPushable, IDamagable
    {
        public void TakeDamage(int damage)
        {
        }
    }
}
