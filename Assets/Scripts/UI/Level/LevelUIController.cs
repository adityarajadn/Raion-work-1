using UnityEngine;
using GameContracts;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] private GameObject[] levelUI;

    [SerializeField] private GameUIController gameUIController;

    IGameUIVisibilitySource gameUIVisibilitySource;

    void Awake()
    {
        if (gameUIController == null)
        {
            gameUIController = FindAnyObjectByType<GameUIController>();
        }

        gameUIVisibilitySource = gameUIController;
    }

    void OnEnable()
    {
        if (gameUIVisibilitySource != null)
        {
            gameUIVisibilitySource.ShowingGameUI += HandleShowingGameUI;
        }
    }

    void OnDisable()
    {
        if (gameUIVisibilitySource != null)
        {
            gameUIVisibilitySource.ShowingGameUI -= HandleShowingGameUI;
        }
    }

    void HandleShowingGameUI(bool isShowing) {
        if (levelUI == null)
        {
            return;
        }

        foreach (GameObject ui in levelUI) {
            if (ui != null)
            {
                ui.SetActive(!isShowing);
            }
        }
    }
}
