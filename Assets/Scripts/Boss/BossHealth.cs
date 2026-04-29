using System;
using UnityEngine;

public class BossHealth : DamageableEntity
{
    public static event Action<bool> OnDied;
    public static event Action<float, float> OnDamaged;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        PlayerAttack.OnAttack += setDamageReceived;
    }

    void OnDisable()
    {
        PlayerAttack.OnAttack -= setDamageReceived;
    }

    void setDamageReceived(float damage)
    {
        damageReceived = damage;
    }

    protected override void OnDamageTaken(float currentHealth, float maxHealth)
    {
        OnDamaged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Boss HP: " + currentHealth);
    }

    protected override void Die()
    {
        Debug.Log("Boss defeated!");
        OnDied?.Invoke(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            TakeDamage(damageReceived);
        }
    }
}
