using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public EnemyDummyMain enemyDummy;

    void Start()
    {
        enemyDummy = GetComponentInParent<EnemyDummyMain>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                float damageAmount = enemyDummy.damageAmount; // Ambil jumlah damage dari EnemyDummyMain
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
