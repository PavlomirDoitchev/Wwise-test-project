using UnityEngine;

namespace Assets.Scripts.Environment.Audio
{
    public class AmbienceManager : MonoBehaviour
    {
        [SerializeField] private string ambienceEvent = "Play_Forest_Day";
        [SerializeField] private string ambienceMusicEvent = "Play_Village_Music";
        [SerializeField] private string ambienceMusicStopEvent = "Stop_Village_Music";
        private bool isMusicPlaying = false;
        //[SerializeField] private string stopAmbienceEvent = "Stop_Ambience_Forest";
        private void Start()
        {
            AkUnitySoundEngine.PostEvent(ambienceEvent, gameObject);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M) && !isMusicPlaying)
            {

                AkUnitySoundEngine.PostEvent(ambienceMusicEvent, gameObject);
                isMusicPlaying = true;
            }
            else if (Input.GetKeyDown(KeyCode.M) && isMusicPlaying)
            {
                AkUnitySoundEngine.PostEvent(ambienceMusicStopEvent, gameObject);
                isMusicPlaying = false;
            }
        }
        //private void OnDestroy()
        //{
        //    AkUnitySoundEngine.PostEvent(stopAmbienceEvent, gameObject);
        //}
    }
}
