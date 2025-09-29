using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBanks();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadBanks()
    {
        AkBankManager.LoadBank("Main", false, false);
    }
}
