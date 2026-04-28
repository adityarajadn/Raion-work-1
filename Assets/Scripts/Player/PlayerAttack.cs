using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static event Action<float> OnAttack;
    public static event Action OnAttackStarted;
    public static event Action OnAttackEnded;
    public event Action<bool> AttackStateChanged;

    public float damageAmount = 25f;
    public Collider2D hitBox;
    private bool isAttacking = false;
    public float attackCooldown = 0.5f;
    public bool canAttack = true;

    public float attackDuration = 0.2f;
    public PlayerParry playerParry;

    private Coroutine cooldownRoutine;

    void Awake()
    {
        if (hitBox != null)
        {
            hitBox.enabled = false;
        }

        if (playerParry == null)
        {
            playerParry = GetComponent<PlayerParry>();
        }
    }

    void OnEnable()
    {
        if (playerParry != null)
        {
            PlayerParry.OnParry += HandleParry;
        }
    }

    void OnDisable()
    {
        if (playerParry != null)
        {
            PlayerParry.OnParry -= HandleParry;
        }
    }

    void HandleParry(bool isParrying)
    {
        SetAttackState(isParrying);
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && canAttack)
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