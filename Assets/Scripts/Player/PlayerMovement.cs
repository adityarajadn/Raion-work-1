using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovementController, IDashController
{
    public event Action OnJump;
    public event Action OnWallJump;
    public event Action<bool> OnDashStateChanged;
    public event Action<bool> DashStateChanged;
    public event Action<bool> OnMoving;

    public float speed = 10f;
    private Vector2 dir;
    public Vector2 MoveDirection => dir;
    public Rigidbody2D rb;
    public PlayerWallCheck wallCheck;

    [Header("Jump")]
    public float jumpForce = 5f;
    public bool isGrounded;

    [Header("Facing")]
    public bool facingRight = true;
    public bool FacingRight => facingRight;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.02f;
    public float dashCooldown = 0.75f;
    private bool isDashing;
    public bool IsDashing => isDashing;
    private float dashTimer;
    private float nextDashTime;
    private float originalGravityScale;
    private bool canInput = true;

    [Header("Air Control")]
    public float groundAcceleration = 30f;
    public float airAcceleration = 10f;

    [Header("Double Jump")]
    [SerializeField] private int maxJump = 2;
    private int currentJump;

    [Header("Wall Movement")]
    public float wallJumpForce = 5f;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private int wallSide;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (wallCheck == null)
        {
            wallCheck = GetComponentInChildren<PlayerWallCheck>();
        }

        if (rb != null)
        {
            originalGravityScale = rb.gravityScale;
        }
    }

    void OnEnable()
    {
        if (wallCheck != null)
        {
            wallCheck.WallContactChanged += HandleWallContactChanged;
        }
    }

    void OnDisable()
    {
        if (wallCheck != null)
        {
            wallCheck.WallContactChanged -= HandleWallContactChanged;
        }
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
            // dir = Vector2.zero;
            return;
        }

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

        if (CanWallJump())
        {
            PerformWallJump();
            return;
        }

        if (currentJump < maxJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            currentJump++;
            isGrounded = false;
            OnJump?.Invoke();
        }
    }

    bool CanWallJump()
    {
        if (!isTouchingWall || isGrounded || isDashing || wallSide == 0)
        {
            return false;
        }

        bool holdingTowardLeftWall = wallSide == -1 && Input.GetKey(KeyCode.A);
        bool holdingTowardRightWall = wallSide == 1 && Input.GetKey(KeyCode.D);

        bool usingFacingDirection = !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) &&
                                    ((wallSide == -1 && !facingRight) || (wallSide == 1 && facingRight));

        return holdingTowardLeftWall || holdingTowardRightWall || usingFacingDirection;
    }

    void PerformWallJump()
    {
        float jumpDirectionX = wallSide == -1 ? 1f : -1f;
        rb.linearVelocity = new Vector2(jumpDirectionX * wallJumpForce, jumpForce);

        facingRight = !facingRight;

        isTouchingWall = false;
        wallSide = 0;
        isGrounded = false;
        currentJump = 1;
        OnWallJump?.Invoke();
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = facingRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void HandleWallContactChanged(bool touchingWall, int side)
    {
        isTouchingWall = touchingWall;
        wallSide = side;
    }

    void StartDash()
    {
        isDashing = true;
        canInput = false;
        dashTimer = dashDuration;
        nextDashTime = Time.time + dashCooldown;
        rb.gravityScale = 0f;
        DashStateChanged?.Invoke(true);
        OnDashStateChanged?.Invoke(true);
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
            OnDashStateChanged?.Invoke(false);
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