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

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void OnEnable() {
        GameUIController.GamePaused += HandleGamePaused;
    }

    void OnDisable() {
        GameUIController.GamePaused -= HandleGamePaused;
    }

    void HandleGamePaused(bool isPaused) {
        animator.SetBool(showPauseMenuParameter, isPaused);
    }
}
