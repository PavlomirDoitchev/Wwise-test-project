using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class BackgroundNPCs : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] string[] animations;
        [SerializeField] int animationToPlay = 0;
        [SerializeField] string soundEffectToPlay = string.Empty;
        float timer = 0f;
        float timeBetweenSounds;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void Start()
        {
            animator.Play(animations[animationToPlay]);
            timeBetweenSounds = Random.Range(1f, 3f);
        }
        private void Update()
        {
            if(string.IsNullOrEmpty(soundEffectToPlay)) return;
            timer += Time.deltaTime;
            if (timer >= timeBetweenSounds)
            {
                AkUnitySoundEngine.PostEvent(soundEffectToPlay, gameObject);
                timeBetweenSounds = Random.Range(1f, 3f);
                timer = 0f;
            }
        }
    }
}
