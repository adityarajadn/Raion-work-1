using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static event Action<float> OnAttack;
    public static event Action OnAttackStarted;
    public static event Action OnAttackEnded;
    public static event Action<bool> AttackStateChanged;

    [SerializeField] private float damageAmount = 25f;
    [SerializeField] private Collider2D hitBox;
    private bool isAttacking = false;
    [SerializeField] private float attackCooldown = 0.5f;
    private bool canAttack = true;

    [SerializeField] private float attackDuration = 0.2f;

    private Coroutine cooldownRoutine;

    public float AttackCooldown => attackCooldown;

    void Awake()
    {
        if (hitBox != null)
        {
            hitBox.enabled = false;
        }

    }

    void OnEnable()
    {
        PlayerParry.OnParry += HandleParry;
        PlayerInputHandler.OnAttackAction += HandleAttackInput;
    }

    void OnDisable()
    {
        PlayerParry.OnParry -= HandleParry;
        PlayerInputHandler.OnAttackAction -= HandleAttackInput;
    }

    void HandleParry(bool isParrying)
    {
        SetAttackState(isParrying);
    }

    void HandleAttackInput()
    {
        if (!isAttacking && canAttack)
        {
            StartCoroutine(AttackingRoutine());
        }
    }

    IEnumerator AttackingRoutine()
    {
        SetAttackState(true);
        yield return new WaitForSeconds(attackDuration);
        SetAttackState(false);
    }

    void SetAttackState(bool value)
    {
        if (isAttacking == value)
        {
            return;
        }

        isAttacking = value;

        if (hitBox != null)
        {
            hitBox.enabled = isAttacking;
        }

        AttackStateChanged?.Invoke(isAttacking);

        if (isAttacking)
        {
            OnAttackStarted?.Invoke();
            OnAttack?.Invoke(damageAmount); // Trigger sekali saat mulai menyerang/parry attack

            if (cooldownRoutine != null)
            {
                StopCoroutine(cooldownRoutine);
            }
            cooldownRoutine = StartCoroutine(AttackCooldownRoutine());
        }
        else
        {
            OnAttackEnded?.Invoke();
        }
    }

    IEnumerator AttackCooldownRoutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        cooldownRoutine = null;
    }
}