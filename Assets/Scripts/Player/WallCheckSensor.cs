using UnityEngine;

public class WallCheckSensor : MonoBehaviour
{
    public PlayerMovement player;
    bool isLeft;
    bool isRight;

    void Start()
    {
        if (transform.position.x < player.transform.position.x) // kiri
        {
            isLeft = true;
            isRight = false;
        } else
        {
            isRight = true;
            isLeft = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (isLeft && Input.GetKeyDown(KeyCode.A)) // kiri
            {
                player.isTouchingLeftWall = true;
                player.wallSlideLeft();
            }
            else if (isRight && Input.GetKeyDown(KeyCode.D)) // kanan
            {
                player.isTouchingRightWall = true;
                player.wallSlideRight();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Wall"))
        {
            player.isTouchingLeftWall = false;
            player.isTouchingRightWall = false;
        }
    }
}
