using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int currentScene;
    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        checkScene();
    }

    void checkScene()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }
}
