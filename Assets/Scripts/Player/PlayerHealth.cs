using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<bool> OnDied;
    public static event Action<float, float> OnDamaged;

    public CinemachineImpulseSource impulseSource;
    public float currentHealth;
    public float maxHealth = 60;
    public float damageReceived;

    void Awake()
    {
        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        currentHealth = maxHealth;
    }

    void OnEnable()
    {
        BossStateController.basicAttackDamage += setDamageReceived;
        BossStateController.pattern2Damage += setDamageReceived;
        BossStateController.pattern3Damage += setDamageReceived;
    }

    void OnDisable()
    {
        BossStateController.basicAttackDamage -= setDamageReceived;
        BossStateController.pattern2Damage -= setDamageReceived;
        BossStateController.pattern3Damage -= setDamageReceived;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyAttackHitBox"))
        {
            TakeDamage(damageReceived); // Example damage value
        }
    }

    void setDamageReceived(float damage)
    {
        damageReceived = damage;
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        checkDeath();
        OnDamaged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Current Health: " + currentHealth);
    }

    void checkDeath()
    {
        if (currentHealth <= 0f)
        {
            die();
        }
    }

    void die()
    {
        currentHealth = 0f;
        OnDied?.Invoke(true);
    }
}
