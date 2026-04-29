using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static event Action<bool> OnJump;
    public static event Action<bool> DashStateChanged;
    public static event Action<bool> OnMoving;
    public static event Action<bool> PlayerFacingRight;
    public PlayerWallCheck playerWallCheck;

    public float speed = 10f;
    private Vector2 dir;
    public Rigidbody2D rb;

    [Header("Jump")]
    public float jumpForce = 5f;
    public bool isGrounded;

    [Header("Facing")]
    public bool facingRight = true;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.02f;
    public float dashCooldown = 0.75f;
    private bool isDashing;
    private float dashTimer;
    private float nextDashTime;
    private bool canInput = true;

    [Header("Air Control")]
    public float groundAcceleration = 30f;
    public float airAcceleration = 10f;

    [Header("Double Jump")]
    [SerializeField] private int maxJump = 2;
    private int currentJump;

    // Wall Jump
    public float wallJumpForceX = 10f;
    public float wallJumpForceY = 5f;
    string wallSide;
    public bool canWallJump; // nilai nya didapat dari PlayerWallCheck
    public float originalGravityScale;

    void OnEnable() {
        PlayerWallCheck.OnWallContact += HandleWallContact;
    }

    void OnDisable() {
        PlayerWallCheck.OnWallContact -= HandleWallContact;
    }
    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        DashTick();
        HandleInput();
        HandleJumpInput();
        Flip();
    }
    

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleInput()
    {
        if (!canInput)
        {
            return;
        }

        // Handle horizontal input
        if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1f;
            facingRight = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1f;
            facingRight = false;
        }
        else
        {
            dir.x = 0f;
        }

        HandleJumpInput();

        // Handle dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && Time.time >= nextDashTime)
        {
            StartDash();
            Debug.Log("Can Input: " + canInput);
        }
    }

    void HandleMovement()
    {
        if (isDashing)
        {
            return;
        }

        float targetSpeed = dir.x * speed;
        float accel = isGrounded ? groundAcceleration : airAcceleration;

        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accel * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);

        OnMoving?.Invoke(dir.x != 0f);
    }

    void HandleJumpInput()
    {
        if (!canInput)
        {
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        if (canWallJump)
        {
            performJumpWall();
            return;
        }

        // Normal jump atau double jump
        if (currentJump < maxJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            currentJump++;
            isGrounded = false;
            OnJump?.Invoke(true);
        }
    }

    void HandleWallContact(bool isTouchingWall, string wallSide)
    {
        canWallJump = isTouchingWall;
        this.wallSide = wallSide;
    }

    void performJumpWall()
    {
        if (!canWallJump || isGrounded)
        {
            return;
        }

        float jumpDir = wallSide == "Right" ? -1f : 1f; // Melompat ke arah berlawanan dari dinding
        rb.linearVelocity = new Vector2(0f, 0f); // Reset velocity sebelum
        rb.AddForce(new Vector2(jumpDir * wallJumpForceX, wallJumpForceY), ForceMode2D.Impulse);
        canWallJump = false; // Mencegah wall jump berulang tanpa menyentuh tanah
        OnJump?.Invoke(true);
        facingRight = !facingRight;
    }


    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
        PlayerFacingRight?.Invoke(facingRight);
    }

    void StartDash()
    {
        isDashing = true;
        canInput = false;
        dashTimer = dashDuration;
        nextDashTime = Time.time + dashCooldown;
        rb.gravityScale = 0f;
        DashStateChanged?.Invoke(true);
    }

    void DashTick()
    {
        if (!isDashing)
        {
            return;
        }

        dashTimer -= Time.deltaTime;
        rb.linearVelocity = new Vector2(facingRight ? dashSpeed : -dashSpeed, 0f);

        if (dashTimer <= 0f)
        {
            isDashing = false;
            dashTimer = 0f;
            canInput = true;
            rb.gravityScale = originalGravityScale;
            // rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);
            DashStateChanged?.Invoke(false);
        }
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
}