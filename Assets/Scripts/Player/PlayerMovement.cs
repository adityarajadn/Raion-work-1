using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 dir;
    public Rigidbody2D rb;

    [Header("Jump")]
    public float jumpForce = 5f;
    private bool isGrounded;

    [Header("Facing")]
    public bool facingRight = true;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    bool isDashing;

    [Header("Air Control")]
    public float groundAcceleration = 30f;
    public float airAcceleration = 10f;

    [Header("Double Jump")]
    private int maxJump = 2;
    private int currentJump = 0;
    
    [Header("Wall Check")]
    public bool isTouchingLeftWall = false;
    public bool isTouchingRightWall = false;
    private float wallSlideSpeed = 2f;

    void Update()
    {
        handleInput();
        jump();
        flip();
    }

    void FixedUpdate()
    {
        handleMovement();
    }

    public void wallSlideLeft()
    {
        if (isTouchingLeftWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
    }

    public void wallSlideRight()
    {
        if (isTouchingRightWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
    }

    public void handleInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            facingRight = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            facingRight = false;
        }
        else
        {
            dir.x = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(dash());
        }
    }

    public void flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void handleMovement()
    {
        if (isDashing) return;

        float targetSpeed = dir.x * speed;

        float accel = isGrounded ? groundAcceleration : airAcceleration;

        float newX = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetSpeed,
            accel * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            currentJump = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    public void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentJump < maxJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            currentJump++;
            isGrounded = false;
        }
    }

    IEnumerator dash()
    {
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(
            facingRight ? dashSpeed : -dashSpeed,
            0
        );

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }
}