using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameUIController : MonoBehaviour
{
    public static event Action<bool> GamePaused;
    public static event Action<bool> showingGameUI;
    public static bool isPaused = false;
    bool canPause = true;
    public GameObject winScreen;
    public GameObject loseScreen;

    void OnEnable()
    {
        PlayerInputHandler.OnPauseAction += HandlePauseInput;
        BossAnimationController.finishDeadAnimation += HandleWinCondition;
        PlayerAnimationHandler.finishDeadAnimation += HandleLoseCondition;
    }

    void OnDisable()
    {
        PlayerInputHandler.OnPauseAction -= HandlePauseInput;
        BossAnimationController.finishDeadAnimation -= HandleWinCondition;
        PlayerAnimationHandler.finishDeadAnimation -= HandleLoseCondition;
    }
    
    void HandlePauseInput() {
        if (!canPause) return;

        TogglePause();
    }

    void HandleWinCondition(bool isWin)
    {
        if (!isWin) return;
        canPause = false;
        Time.timeScale = 0f;
        showWinScreen();
    }

    void HandleLoseCondition(bool isLose)
    {
        if (!isLose) return;
        canPause = false;
        Time.timeScale = 0f;
        showLoseScreen();
    }

    void showWinScreen() {
        if (winScreen == null || loseScreen == null) return;
        winScreen.SetActive(true);
        loseScreen.SetActive(false);
        showingGameUI?.Invoke(true);
    }

    void showLoseScreen() {
        if (winScreen == null || loseScreen == null) return;
        loseScreen.SetActive(true);
        winScreen.SetActive(false);
        showingGameUI?.Invoke(true);
    }

    public static void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        showPauseMenu(isPaused);
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
