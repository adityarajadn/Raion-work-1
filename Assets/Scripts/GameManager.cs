using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int currentScene;
    public static int CurrentScene => currentScene;
    public static GameManager Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;

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
        disableMusic();
    }

    void checkScene()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }
    
    void disableMusic() {
        if (currentScene == 1) { // bossbattlescene
            audioSource.Stop();
        } else
        {
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
    }
}
