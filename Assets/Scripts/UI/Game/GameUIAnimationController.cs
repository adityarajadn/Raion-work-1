using UnityEngine;
using GameContracts;

public class GameUIAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string showPauseMenuParameter = "isPause";

    [SerializeField] private GameUIController gameUIController;

    IPauseStateSource pauseStateSource;

    void Awake()
    {
        if (gameUIController == null)
        {
            gameUIController = FindAnyObjectByType<GameUIController>();
        }

        pauseStateSource = gameUIController;
    }

    void Start()
    {
        if (animator == null) {
            animator = GetComponent<Animator>();
        }

        if (animator == null)
        {
            return;
        }

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void OnEnable() {
        if (pauseStateSource != null)
        {
            pauseStateSource.GamePaused += HandleGamePaused;
        }
    }

    void OnDisable() {
        if (pauseStateSource != null)
        {
            pauseStateSource.GamePaused -= HandleGamePaused;
        }
    }

    void HandleGamePaused(bool isPaused) {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(showPauseMenuParameter, isPaused);
    }
}
