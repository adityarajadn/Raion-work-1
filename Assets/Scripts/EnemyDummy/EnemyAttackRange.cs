using UnityEngine;
using System;

public class EnemyAttackRange : MonoBehaviour, ICollisionDamageDealer
{
    public static event Action<float, PlayerHealth> OnPlayerHitByEnemy;

    public EnemyDummyMain enemyDummy;
    public event Action<bool> Attack;
    private bool isAttacking;


    void Start()
    {
        enemyDummy = GetComponentInParent<EnemyDummyMain>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        TryDealDamage(collision);
    }

    public void SetAttackState(bool value)
    {
        if (isAttacking == value)
        {
            return;
        }

        isAttacking = value;
        Attack?.Invoke(isAttacking);
    }

    public void TryDealDamage(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        PlayerParry playerParry = collision.GetComponent<PlayerParry>();
        if (playerParry != null && playerParry.TryParry())
        {
            return;
        }

        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth == null || enemyDummy == null)
        {
            return;
        }

        float damageAmount = enemyDummy.DamageAmount;
        playerHealth.TakeDamage(damageAmount);
        OnPlayerHitByEnemy?.Invoke(damageAmount, playerHealth);
    }
}
