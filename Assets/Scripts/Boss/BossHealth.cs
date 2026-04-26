using System;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public float damageReceived;

    public event Action<bool> OnDied;
    public event Action<float, float> OnDamaged;

    void OnEnable()
    {
        PlayerAttack.OnAttack += setDamageReceived;
    }

    void OnDisable()
    {
        PlayerAttack.OnAttack -= setDamageReceived;
    }

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void takeDamage(float damage)
    {
        currentHealth -= damage;
        checkDeath();
        OnDamaged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Boss HP: " + currentHealth);
    }

    void checkDeath()
    {
        if (currentHealth <= 0f)
        {
            die();
        }
    }

    void setDamageReceived(float damage)
    {
        damageReceived = damage;
    }

    void die()
    {
        Debug.Log("Boss defeated!");
        currentHealth = 0f;
        OnDied?.Invoke(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttackHitBox"))
        {
            takeDamage(damageReceived);
        }
    }
}
