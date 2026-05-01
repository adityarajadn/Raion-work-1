using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : DamageableEntity
{
    public static event Action<bool> OnDied;
    public static event Action<float, float> OnDamaged;

    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private Collider2D playerHitBox;

    public CinemachineImpulseSource ImpulseSource => impulseSource;

    protected override void Awake()
    {
        base.Awake();

        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        if (playerHitBox == null)
        {
            playerHitBox = GetComponent<Collider2D>();
            if (playerHitBox != null)
            {
                playerHitBox.enabled = true;
            }
        }
    }

    void OnEnable()
    {
        BossStateController.basicAttackDamage += setDamageReceived;
        BossStateController.pattern2Damage += setDamageReceived;
        BossStateController.pattern3Damage += setDamageReceived;
        PlayerParry.OnParry += HandleParryEvent;
    }

    void OnDisable()
    {
        BossStateController.basicAttackDamage -= setDamageReceived;
        BossStateController.pattern2Damage -= setDamageReceived;
        BossStateController.pattern3Damage -= setDamageReceived;
        PlayerParry.OnParry -= HandleParryEvent;
    }

    void HandleParryEvent(bool isParrying)
    {
        if (playerHitBox == null)
        {
            return;
        }

        if (!isParrying) {
            playerHitBox.enabled = true;
        } else {
            playerHitBox.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyAttackHitBox") || other.CompareTag("EnemyAttackEffect_Slash"))
        {
            TakeDamage(damageReceived);
        }
    }

    void setDamageReceived(float damage)
    {
        damageReceived = damage;
    }

    protected override void OnDamageTaken(float currentHealth, float maxHealth)
    {
        OnDamaged?.Invoke(currentHealth, maxHealth);
        Debug.Log("Current Health: " + currentHealth);
    }

    protected override void Die()
    {
        OnDied?.Invoke(true);
    }
}
