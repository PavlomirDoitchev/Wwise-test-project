using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class BackgroundNPCs : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] string[] animations;
        [SerializeField] int animationToPlay = 0;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            animator.Play(animations[animationToPlay]);
        }
    }
}
