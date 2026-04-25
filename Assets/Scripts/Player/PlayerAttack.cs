using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttackController
{
    public static event Action<bool> OnAttack;
    public static event Action OnAttackStarted;
    public static event Action OnAttackEnded;
    public event Action<bool> AttackStateChanged;

    public Collider2D hitBox;
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    public float attackDuration = 0.2f;
    private Coroutine attackRoutine;
    public PlayerParry playerParry;

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
            playerParry.OnParry += parryEffect;
        }
    }

    void OnDisable()
    {
        if (playerParry != null)
        {
            playerParry.OnParry -= parryEffect;
        }
    }

    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            attackRoutine = StartCoroutine(AttackingRoutine());
        }
    }

    IEnumerator AttackingRoutine()
    {
        SetAttackState(true);
        yield return new WaitForSeconds(attackDuration);
        SetAttackState(false);
        attackRoutine = null;
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
        OnAttack?.Invoke(isAttacking);

        if (isAttacking)
        {
            OnAttackStarted?.Invoke();
        }
        else
        {
            OnAttackEnded?.Invoke();
        }
    }
    
    void parryEffect(bool isParry) {
        if (isParry && !isAttacking) {
            StartCoroutine(AttackingRoutine());
            Debug.Log("Parry Success!");
        } else {
            Debug.Log("Parry Failed!");
        }
    }
}
