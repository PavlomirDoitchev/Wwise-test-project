using UnityEngine;

namespace Assets.Scripts.Environment.Audio
{
    public class AmbienceManager : MonoBehaviour
    {
        [SerializeField] private string ambienceEvent = "Play_Forest_Day";
        [SerializeField] private string ambienceMusicEvent = "Play_Village_Music";
        //[SerializeField] private string stopAmbienceEvent = "Stop_Ambience_Forest";
        private void Start()
        {
            AkUnitySoundEngine.PostEvent(ambienceEvent, gameObject);
            AkUnitySoundEngine.PostEvent(ambienceMusicEvent, gameObject);
        }
        //private void OnDestroy()
        //{
        //    AkUnitySoundEngine.PostEvent(stopAmbienceEvent, gameObject);
        //}
    }
}
