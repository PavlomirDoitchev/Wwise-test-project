using Assets.Scripts.StateMachine.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class BackgroundNPCs : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] string[] animations;
        [SerializeField] int animationToPlay = 0;
        [SerializeField] string soundEffectToPlay = string.Empty;
        [SerializeField] PlayerStateMachine player;
        [SerializeField] float distanceThreshold = 10f;
        [SerializeField] bool hasGreetingSound = false;
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
            if (string.IsNullOrEmpty(soundEffectToPlay)) return;
            timer += Time.deltaTime;
            if (timer >= timeBetweenSounds)
            {
                if (Vector3.Distance(player.transform.position, transform.position) > distanceThreshold)
                    return;
                else if(hasGreetingSound)
                {
                    AkUnitySoundEngine.PostEvent(soundEffectToPlay, gameObject);
                    var npcRotation = Quaternion.LookRotation(player.transform.position - this.gameObject.transform.position);
                    npcRotation.x = 0f;
                    npcRotation.z = 0f;
                    hasGreetingSound = false;
                }
                else if(!hasGreetingSound)
                    AkUnitySoundEngine.PostEvent(soundEffectToPlay, gameObject);

                timeBetweenSounds = Random.Range(1f, 3f);

                timer = 0f;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distanceThreshold);
        }
    }
}
