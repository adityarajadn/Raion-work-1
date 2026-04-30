using System;
using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] protected float damageReceived;

    public static event Action<float, float> OnDamaged;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    protected void TakeDamage(float damage)
    {
        currentHealth -= damage;
        CheckDeath();
        OnDamageTaken(currentHealth, maxHealth);
    }

    protected void CheckDeath()
    {
        if (currentHealth > 0f)
        {
            return;
        }

        currentHealth = 0f;
        Die();
    }

    protected abstract void OnDamageTaken(float currentHealth, float maxHealth);

    protected abstract void Die();
}