
using UnityEngine;

namespace Assets.Scripts.Environment.Audio
{
    public class CustomMusicTrigger : MonoBehaviour
    {
        [SerializeField] private string[] musicStart;
        [SerializeField] private string[] musicStop;

        [SerializeField] int musicIndex = 0;

        private void OnTriggerEnter(Collider other)
        {
            AkUnitySoundEngine.PostEvent(musicStart[musicIndex], gameObject);
        }
        private void OnTriggerExit(Collider other)
        {
            AkUnitySoundEngine.PostEvent(musicStop[musicIndex], gameObject);
        }
    }
}
