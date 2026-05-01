using System;
using System.Collections;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [Header("Animator Parameter")]
    public string isAttackingParameter = "isAttacking";
    public string isPattern2Parameter = "isPattern2";
    public string isPattern3Parameter = "isPattern3";
    public string isDeadParameter = "isDead";

    public float hurtDuration = 0.15f;
    public float deadDuration = 5f;
    public Animator animator;
    public static event Action<bool> isHandlingAttack;
    public static event Action<bool> finishDeadAnimation;


    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        BossStateController.OnBasicAttack += HandleBasicAttack;
        BossStateController.OnPattern2 += HandlePattern2;
        BossStateController.OnPattern3 += HandlePattern3;
        BossHealth.OnDied += HandleDead;
    }

    void OnDisable()
    {
        BossStateController.OnBasicAttack -= HandleBasicAttack;
        BossStateController.OnPattern2 -= HandlePattern2;
        BossStateController.OnPattern3 -= HandlePattern3;
        BossHealth.OnDied -= HandleDead;

        StopAllCoroutines();
    }

    void HandleDead(bool isDead)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isDeadParameter, isDead);
        StartCoroutine(DeadRoutine(isDead));
    }

    IEnumerator DeadRoutine(bool isDead)
    {
        yield return new WaitForSeconds(deadDuration); // Durasi animasi mati, sesuaikan dengan animasi yang digunakan
        finishDeadAnimation?.Invoke(isDead);
    }

    void HandleBasicAttack(bool state)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isAttackingParameter, state);
        isHandlingAttack?.Invoke(state);
    }

    void HandlePattern2(bool state)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isPattern2Parameter, state);
        isHandlingAttack?.Invoke(state);
    }

    void HandlePattern3(bool state)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isPattern3Parameter, state);
        isHandlingAttack?.Invoke(state);
    }
}