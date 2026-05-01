using UnityEngine;

public class GameUIAnimationController : MonoBehaviour
{
    public Animator animator;
    public string showPauseMenuParameter = "isPause";
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
        GameUIController.GamePaused += HandleGamePaused;
    }

    void OnDisable() {
        GameUIController.GamePaused -= HandleGamePaused;
    }

    void HandleGamePaused(bool isPaused) {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(showPauseMenuParameter, isPaused);
    }
}
