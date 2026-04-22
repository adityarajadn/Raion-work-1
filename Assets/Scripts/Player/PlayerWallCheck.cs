using UnityEngine;

public class PlayerWallCheck : MonoBehaviour
{
    public Collider2D wallCheckCollider;
    public PlayerMovement player;

    void Awake()
    {
        if (wallCheckCollider == null)
        {
            wallCheckCollider = GetComponent<Collider2D>();
        }

        if (player == null)
        {
            player = GetComponentInParent<PlayerMovement>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            player.isTouchingWall = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            player.isTouchingWall = false;
        }
    }
}
