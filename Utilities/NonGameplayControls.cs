using Assets.Scripts.StateMachine.Player;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities.Contracts;
using UnityEngine.UI;
using Assets.Scripts.Entities;
using NHance.Assets.Scripts.Enums;

public class NonGameplayControls : MonoBehaviour, IObserver
{
    [SerializeField] PlayerStateMachine _player;
    [SerializeField] Canvas _canvas;
    [SerializeField] Canvas _playerInfo;
    [SerializeField] Image playerHealth;
    int currentHealth;

    private bool isPaused = false;

    private void Start()
    {
        AkUnitySoundEngine.SetState("GamePause", "Playing");
        _canvas.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        _playerInfo.enabled = false;    
        _player.InputManager.InputEnabled = false;
        _canvas.enabled = true;
        AkUnitySoundEngine.SetState("GamePause", "Paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        _playerInfo.enabled = true;
        _canvas.enabled = false;
        AkUnitySoundEngine.SetState("GamePause", "Playing");
        StartCoroutine(EnableInputNextFrame());
    }

    private IEnumerator EnableInputNextFrame()
    {
        while (Input.GetMouseButton(0))
            yield return null;

        _player.InputManager.InputEnabled = true;
    }

    public void OnNotify()
    {
        currentHealth = _player.PlayerStats.PlayerHealth;
        playerHealth.fillAmount = _player.PlayerStats.PlayerHealth * .01f;
    }
    private void OnEnable()
    {
        _player.PlayerStats.AddObserver(this);
    }
    private void OnDisable()
    {
        _player.PlayerStats.AddObserver(this);
    }
}
