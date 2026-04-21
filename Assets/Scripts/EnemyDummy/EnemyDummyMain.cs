using UnityEngine;

public class EnemyDummyMain : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttackHitBox"))
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
