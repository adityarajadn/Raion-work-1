using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    public string scene;

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(scene)) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void RestartLevel()
    {
        if (GameManager.currentScene == 0) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameManager.currentScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        GameUIController.TogglePause();
    }
}
