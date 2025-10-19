using Assets.Scripts.StateMachine.Player;
using UnityEngine;

public class NonGameplayControls : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _player;
    [SerializeField] Canvas _canvas;


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
            {
                ResumeGame();
                
                _player.InputManager.enabled = true;
            }
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;    
        isPaused = true;
        _player.InputManager.enabled = false;
        _canvas.enabled = true;
        AkUnitySoundEngine.SetState("GamePause", "Paused");
    }

    public void ResumeGame()
    {
        //_player.InputManager.enabled = true;
        Time.timeScale = 1f;
        isPaused = false;
        _canvas.enabled = false;
        AkUnitySoundEngine.SetState("GamePause", "Playing");
    }
}
