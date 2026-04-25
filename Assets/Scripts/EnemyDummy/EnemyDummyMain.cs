using System.Collections;
using UnityEngine;

public class EnemyDummyMain : DamageableEntity, IAttackDamageSource
{
    public static event System.Action<EnemyDummyMain> OnEnemyDefeated;

    public Collider2D attackRange;
    private EnemyAttackRange attackRangeEventSource;
    private Coroutine attackRoutine;

    public float damageAmount = 10f;
    public float DamageAmount => damageAmount;
    public SpriteRenderer spriteRenderer;
    public float playerAttackDamage = 25f;

    IEnumerator attack()
    {
        while (true)
        {
            if (attackRangeEventSource != null)
            {
                attackRangeEventSource.SetAttackState(true);
            }

            attackRange.enabled = true;
            yield return new WaitForSeconds(0.5f); // Durasi serangan aktif
            attackRange.enabled = false;

            if (attackRangeEventSource != null)
            {
                attackRangeEventSource.SetAttackState(false);
            }

            yield return new WaitForSeconds(3f); // Durasi cooldown serangan
        }
    }

    void Awake()
    {
        base.Awake();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (attackRange != null)
        {
            attackRangeEventSource = attackRange.GetComponent<EnemyAttackRange>();
        }

        if (attackRangeEventSource == null)
        {
            attackRangeEventSource = GetComponentInChildren<EnemyAttackRange>();
        }
    }

    void Update()
    {
        Debug.Log($"Enemy Health: " + CurrentHealth + "/" +maxHealth);
    }

    void Start()
    {
        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(attack());
        }
    }
    
    void OnEnable()
    {
        PlayerAttack.OnAttack += HandlePlayerAttack; // Subscribe ke event serangan pemain
    }

    void OnDisable()
    {
        PlayerAttack.OnAttack -= HandlePlayerAttack; // Unsubscribe dari event serangan pemain  
    }

    void HandlePlayerAttack(bool isAttacking)
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (!isAttacking)
        {
            spriteRenderer.color = Color.white;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
            }

            TakeDamage(playerAttackDamage);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white;
            }
        }
    }

    protected override void OnDeath()
    {
        OnEnemyDefeated?.Invoke(this);
        Destroy(gameObject);
    }
}
