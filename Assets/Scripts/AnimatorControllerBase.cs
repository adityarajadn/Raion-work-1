using System;
using System.Collections;
using UnityEngine;

public abstract class AnimatorControllerBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected string isDeadParameter = "isDead";
    [SerializeField] protected float deadDuration = 5f;

    protected virtual void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    protected void SafeSetBool(string param, bool value)
    {
        if (animator == null) return;
        animator.SetBool(param, value);
    }

    protected IEnumerator DeadRoutine(bool isDead)
    {
        if (animator == null)
            yield break;

        yield return new WaitForSeconds(deadDuration);
        OnDeadFinished(isDead);
    }

    // subclasses should implement how to notify when death animation finished
    protected abstract void OnDeadFinished(bool isDead);
}
