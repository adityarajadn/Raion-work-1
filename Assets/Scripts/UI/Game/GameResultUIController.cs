using UnityEngine;

public class GameResultUIController : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    public void Configure(GameObject winScreen, GameObject loseScreen)
    {
        this.winScreen = winScreen;
        this.loseScreen = loseScreen;
    }

    void OnEnable()
    {
        BossAnimationController.finishDeadAnimation += HandleWinCondition;
        PlayerAnimationHandler.finishDeadAnimation += HandleLoseCondition;
    }

    void OnDisable()
    {
        BossAnimationController.finishDeadAnimation -= HandleWinCondition;
        PlayerAnimationHandler.finishDeadAnimation -= HandleLoseCondition;
    }

    void HandleWinCondition(bool isWin)
    {
        if (!isWin) return;
        ShowResult(true);
    }

    void HandleLoseCondition(bool isLose)
    {
        if (!isLose) return;
        ShowResult(false);
    }

    void ShowResult(bool isWin)
    {
        if (winScreen == null || loseScreen == null)
        {
            return;
        }

        GameUIController.SetPauseEnabled(false);
        Time.timeScale = 0f;

        winScreen.SetActive(isWin);
        loseScreen.SetActive(!isWin);

        GameUIController.SetGameUIVisibility(true);
    }
}
