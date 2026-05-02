using System;
using UnityEngine;
using UnityEngine.EventSystems;
using GameContracts;

public class GameUIController : MonoBehaviour, IPauseStateSource, IGameUIVisibilitySource, IPauseControl, IGameUIVisibilityControl
{
    public event Action<bool> GamePaused;
    public event Action<bool> ShowingGameUI;

    bool isPaused;
    bool canPause = true;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    void Awake()
    {
        GameResultUIController resultController = GetComponent<GameResultUIController>();
        if (resultController == null)
        {
            resultController = gameObject.AddComponent<GameResultUIController>();
        }

        resultController.Configure(winScreen, loseScreen);
    }

    void OnEnable()
    {
        PlayerInputHandler.OnPauseAction += HandlePauseInput;
    }

    void OnDisable()
    {
        PlayerInputHandler.OnPauseAction -= HandlePauseInput;
    }
    
    void HandlePauseInput() {
        if (!canPause) return;

        TogglePause();
    }

    public void TogglePause()
    {
        if (!canPause) return;

        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        showPauseMenu(isPaused);
    }

    public void SetPauseEnabled(bool value)
    {
        canPause = value;

        if (!canPause && isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            showPauseMenu(false);
        }
    }

    public void SetGameUIVisibility(bool isVisible)
    {
        ShowingGameUI?.Invoke(isVisible);
    }

    void showPauseMenu(bool isPaused) {
        GamePaused?.Invoke(isPaused);
        ShowingGameUI?.Invoke(isPaused);
        
        if (!isPaused && EventSystem.current != null) {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
