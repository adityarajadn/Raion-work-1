using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    [SerializeField] private string scene;

    public void LoadScene()
    {
        if (string.IsNullOrEmpty(scene)) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void RestartLevel()
    {
        if (GameManager.CurrentScene == 0) return;
        Time.timeScale = 1f;
        SceneManager.LoadScene(GameManager.CurrentScene);
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
