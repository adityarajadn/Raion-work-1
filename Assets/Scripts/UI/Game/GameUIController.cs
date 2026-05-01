using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameUIController : MonoBehaviour
{
    public static event Action<bool> GamePaused;
    public static event Action<bool> showingGameUI;
    static bool isPaused;
    static bool canPause = true;
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

    public static void TogglePause()
    {
        if (!canPause) return;

        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        showPauseMenu(isPaused);
    }

    public static void SetPauseEnabled(bool value)
    {
        canPause = value;

        if (!canPause && isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            showPauseMenu(false);
        }
    }

    public static void SetGameUIVisibility(bool isVisible)
    {
        showingGameUI?.Invoke(isVisible);
    }

    static void showPauseMenu(bool isPaused) {
        GamePaused?.Invoke(isPaused);
        showingGameUI?.Invoke(isPaused);

        // Clear any selected UI object when resuming so keyboard (Space/Enter)
        // doesn't accidentally activate a button left selected by the pause menu.
        if (!isPaused && EventSystem.current != null) {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
