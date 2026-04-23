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
            updateWallSide(collision);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            updateWallSide(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            player.setWallSide(0);
        }
    }

    void updateWallSide(Collider2D collision)
    {
        if (player == null) return;

        float deltaX = collision.bounds.center.x - player.transform.position.x;
        int side = deltaX < 0f ? -1 : 1;
        player.setWallSide(side);

        if (wallCheckCollider != null)
        {
            player.setIsTouchingWall(true);
        }
    }
}
