using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static event Action<bool> OnJump;
    public static event Action<bool> OnMoving;
    public static event Action<bool> PlayerFacingRight;

    [SerializeField] private float speed = 10f;
    private Vector2 dir;
    [SerializeField] private Rigidbody2D rb;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private bool isGrounded;

    [Header("Facing")]
    [SerializeField] private bool facingRight = true;

    private bool canInput = true;
    private bool isDashActive;

    [Header("Air Control")]
    [SerializeField] private float groundAcceleration = 30f;
    [SerializeField] private float airAcceleration = 10f;

    [Header("Double Jump")]
    [SerializeField] private int maxJump = 2;
    private int currentJump;

    // Wall Jump
    [SerializeField] private float wallJumpForceX = 10f;
    [SerializeField] private float wallJumpForceY = 5f;
    string wallSide;
    bool canWallJump;

    public bool FacingRight => facingRight;

    void OnEnable() {
        PlayerWallCheck.OnWallContact += HandleWallContact;
        PlayerDashController.DashStateChanged += HandleDashStateChanged;
        PlayerInputHandler.OnMoveAction += HandleMoveInput;
        PlayerInputHandler.OnJumpAction += HandleJumpInput;
    }

    void OnDisable() {
        PlayerWallCheck.OnWallContact -= HandleWallContact;
        PlayerDashController.DashStateChanged -= HandleDashStateChanged;
        PlayerInputHandler.OnMoveAction -= HandleMoveInput;
        PlayerInputHandler.OnJumpAction -= HandleJumpInput;
    }
    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (GetComponent<PlayerDashController>() == null)
        {
            gameObject.AddComponent<PlayerDashController>();
        }
    }

    void Update()
    {
        Flip();
    }
    

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMoveInput(Vector2 input)
    {
        if (!canInput)
        {
            dir.x = 0f;
            OnMoving?.Invoke(false);
            return;
        }

        dir.x = input.x;

        if (dir.x > 0f)
        {
            facingRight = true;
        }
        else if (dir.x < 0f)
        {
            facingRight = false;
        }

        OnMoving?.Invoke(dir.x != 0f);
    }

    void HandleMovement()
    {
        if (isDashActive) return;

        float targetSpeed = dir.x * speed;
        float accel = isGrounded ? groundAcceleration : airAcceleration;
        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accel * Time.fixedDeltaTime);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    void HandleJumpInput()
    {
        if (!canInput) return;

        if (canWallJump)
        {
            performJumpWall();
            return;
        }

        if (currentJump >= maxJump) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        currentJump++;
        isGrounded = false;
        OnJump?.Invoke(true);
    }

    void HandleWallContact(bool isTouchingWall, string wallSide)
    {
        canWallJump = isTouchingWall;
        this.wallSide = wallSide;
    }

    void HandleDashStateChanged(bool isDashing)
    {
        isDashActive = isDashing;
        canInput = !isDashing;

        if (isDashing)
        {
            dir.x = 0f;
            OnMoving?.Invoke(false);
        }
    }

    void performJumpWall()
    {
        if (!canWallJump || isGrounded) return;

        float jumpDir = wallSide == "Right" ? -1f : 1f;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(jumpDir * wallJumpForceX, wallJumpForceY), ForceMode2D.Impulse);
        canWallJump = false;
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