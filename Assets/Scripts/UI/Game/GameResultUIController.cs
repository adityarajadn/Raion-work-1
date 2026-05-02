using UnityEngine;
using GameContracts;

public class GameResultUIController : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameUIController gameUIController;
    [SerializeField] private BossAnimationController bossAnimationController;
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    IPauseControl pauseControl;
    IGameUIVisibilityControl gameUIVisibilityControl;
    IDeadAnimationSource bossDeathSource;
    IDeadAnimationSource playerDeathSource;

    public void Configure(GameObject winScreen, GameObject loseScreen)
    {
        this.winScreen = winScreen;
        this.loseScreen = loseScreen;
    }

    void Awake()
    {
        if (gameUIController == null)
        {
            gameUIController = FindAnyObjectByType<GameUIController>();
        }

        if (bossAnimationController == null)
        {
            bossAnimationController = FindAnyObjectByType<BossAnimationController>();
        }

        if (playerAnimationHandler == null)
        {
            playerAnimationHandler = FindAnyObjectByType<PlayerAnimationHandler>();
        }

        pauseControl = gameUIController;
        gameUIVisibilityControl = gameUIController;
        bossDeathSource = bossAnimationController;
        playerDeathSource = playerAnimationHandler;
    }

    void OnEnable()
    {
        if (bossDeathSource != null)
        {
            bossDeathSource.DeadFinished += HandleWinCondition;
        }

        if (playerDeathSource != null)
        {
            playerDeathSource.DeadFinished += HandleLoseCondition;
        }
    }

    void OnDisable()
    {
        if (bossDeathSource != null)
        {
            bossDeathSource.DeadFinished -= HandleWinCondition;
        }

        if (playerDeathSource != null)
        {
            playerDeathSource.DeadFinished -= HandleLoseCondition;
        }
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

        pauseControl?.SetPauseEnabled(false);
        Time.timeScale = 0f;

        winScreen.SetActive(isWin);
        loseScreen.SetActive(!isWin);

        gameUIVisibilityControl?.SetGameUIVisibility(true);
    }
}
