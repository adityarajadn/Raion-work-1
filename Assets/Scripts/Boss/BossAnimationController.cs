// ==============================
// BossAnimationController.cs
// ==============================
using System;
using System.Collections;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [Header("Animator Parameter")]
    public string isAttackingParameter = "isAttacking";
    public string isPattern2Parameter = "isPattern2";
    public string isPattern3Parameter = "isPattern3";
    public string isHurtParameter = "isHurt";
    public string isDeadParameter = "isDead";

    public float hurtDuration = 0.15f;
    public Animator animator;
    public static event Action<bool> isHandlingAttack;


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
        BossHealth.OnDamaged += HandleHurt;
        BossHealth.OnDied += HandleDead;
    }

    void OnDisable()
    {
        BossStateController.OnBasicAttack -= HandleBasicAttack;
        BossStateController.OnPattern2 -= HandlePattern2;
        BossStateController.OnPattern3 -= HandlePattern3;
        BossHealth.OnDamaged -= HandleHurt;
        BossHealth.OnDied -= HandleDead;
    }

    void HandleDead(bool isDead)
    {
        animator.SetBool(isDeadParameter, isDead);
    }

    void HandleBasicAttack(bool state)
    {
        animator.SetBool(isAttackingParameter, state);
        isHandlingAttack?.Invoke(state);
    }

    void HandlePattern2(bool state)
    {
        animator.SetBool(isPattern2Parameter, state);
        isHandlingAttack?.Invoke(state);
    }

    void HandlePattern3(bool state)
    {
        animator.SetBool(isPattern3Parameter, state);
        isHandlingAttack?.Invoke(state);
    }

    void HandleHurt(float currentHealth, float maxHealth)
    {
        StartCoroutine(HurtAnimation(currentHealth, maxHealth));
    }
    
    IEnumerator HurtAnimation(float currentHealth, float maxHealth) {
        animator.SetBool(isHurtParameter, true);
        yield return new WaitForSeconds(hurtDuration);
        animator.SetBool(isHurtParameter, false);
    }
}