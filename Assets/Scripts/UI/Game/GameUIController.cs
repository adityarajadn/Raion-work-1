using System;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public static event Action<bool> GamePaused;
    bool isPaused = false;

    void Update()
    {
        HandlePauseInput();
    }

    void HandlePauseInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        showPauseMenu(isPaused);
    }

    void showPauseMenu(bool show) {
        GamePaused?.Invoke(show);
    }
}
