
using UnityEngine;
namespace Assets.Scripts.Environment.Traps
{
    public class SpikeTrap : Trap
    {
        private BoxCollider boxCollider;
        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
        }
        public void EnableCollider() 
        {
            boxCollider.enabled = !boxCollider.enabled;   
        }
    }
}
