using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    [SerializeField] private GameObject[] levelUI;

    void OnEnable()
    {
        GameUIController.showingGameUI += HandleShowingGameUI;
    }

    void OnDisable()
    {
        GameUIController.showingGameUI -= HandleShowingGameUI;
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
