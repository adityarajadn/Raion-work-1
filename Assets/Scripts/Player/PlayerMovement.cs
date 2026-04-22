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
    public float dashDuration = 2f;
    public float dashCooldown = 0.75f;
    bool isDashing = false;
    private float dashTimer = 0f;
    private float nextDashTime = 0f;
    private float originalGravityScale;

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

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb != null)
        {
            originalGravityScale = rb.gravityScale;
        }
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= nextDashTime)
        {
            StartDash();
        }
        dash();
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

    void StartDash()
    {   
        isDashing = true;
        dashTimer = dashDuration;
        nextDashTime = Time.time + dashCooldown; // Set waktu berikutnya untuk dash
        rb.gravityScale = 0f;
    }
    void dash()
    {
        if (!isDashing) return;

        dashTimer -= Time.deltaTime;

        rb.linearVelocity = new Vector2(
            facingRight ? dashSpeed : -dashSpeed,
            0
        );

        if (dashTimer <= 0f)
        {
            isDashing = false;
            rb.gravityScale = originalGravityScale;
        }
    }
}