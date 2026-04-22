using System.Collections;
using UnityEngine;

public class EnemyDummyMain : MonoBehaviour
{
    public Collider2D attackRange;
    private Coroutine attackRoutine;

    public float damageAmount = 10f;
    public SpriteRenderer spriteRenderer;

    IEnumerator attack()
    {
        while (true)
        {
            attackRange.enabled = true;
            yield return new WaitForSeconds(0.5f); // Durasi serangan aktif
            attackRange.enabled = false;
            yield return new WaitForSeconds(3f); // Durasi cooldown serangan
        }
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
        //
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            spriteRenderer.color = Color.red;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            spriteRenderer.color = Color.white;
        }
    }
}
