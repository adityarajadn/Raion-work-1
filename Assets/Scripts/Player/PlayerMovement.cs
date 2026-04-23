using System;
using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour, IMoveable, IDashable, IWallJump
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    public float getJumpForce() {
        return jumpForce;
    }

    [Header("Dash")]
    [SerializeField] private float dashForce = 1.5f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    // --- Delegates ---
    public event Action OnDashStart;
    public event Action OnDashEnd;
    public event Action OnLand;
    public event Action OnWallJump;

    // --- State ---
    private Rigidbody2D rb;
    private bool isDashing;
    private bool isGrounded;
    private float dashCooldownTimer;
    private bool isTouchingWall;
    private int wallSide;
    private float moveInputX;

    public void setIsTouchingWall(bool value)
    {
        isTouchingWall = value;

        if (!value)
        {
            wallSide = 0;
        }
    }

    public void setWallSide(int side)
    {
        wallSide = Mathf.Clamp(side, -1, 1);
        isTouchingWall = wallSide != 0;
    }

    public void setMoveInputX(float inputX)
    {
        moveInputX = Mathf.Clamp(inputX, -1f, 1f);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        handleDashCooldown();
    }

    void handleDashCooldown()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
            Debug.Log("Dash cooldown: " + dashCooldownTimer);
        }
    }

    // --- IMoveable ---
    public void Move(Vector2 direction)
    {
        if (isDashing) return; // tidak bisa bergerak biasa saat dash

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    public void SetFacing(int direction)
    {
        if (direction == 0) return;

        int currentScaleY = (int)transform.localScale.y;
        int currentScaleZ = (int)transform.localScale.z;
        transform.localScale = new Vector3(direction, currentScaleY, currentScaleZ);
    }

    public void Jump(float jumpForce)
    {
        if (isDashing || !isGrounded) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }
    
    // --- IDashable ---
    public bool CanDash()
    {
        return !isDashing && dashCooldownTimer <= 0f;
    }

    public void Dash(Vector2 direction)
    {
        if (!CanDash()) return;
        StartCoroutine(DashRoutine(direction));
    }

    // --- IWallJump ---
    public bool CanWallJump()
    {
        return isTouchingWall && wallSide != 0 && !isGrounded && !isDashing && IsHoldingTowardWall();
    }

    public void WallJump(Vector2 direction, float jumpForce)
    {
        if (!CanWallJump()) return;

        float jumpDirectionX = wallSide == -1 ? 1f : -1f;
        Vector2 jumpDirection = new Vector2(jumpDirectionX, 1f).normalized;

        rb.linearVelocity = Vector2.zero; // reset velocity sebelum wall jump
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
        OnWallJump?.Invoke(); // subscriber dikasih tau kalau wall jump udah dilakukan

        isTouchingWall = false;
        wallSide = 0;
        isGrounded = false;
    }

    bool IsHoldingTowardWall()
    {
        if (Mathf.Abs(moveInputX) > 0.01f)
        {
            return (wallSide == -1 && moveInputX < 0f) || (wallSide == 1 && moveInputX > 0f);
        }

        float facingX = transform.localScale.x;
        return (wallSide == -1 && facingX < 0f) || (wallSide == 1 && facingX > 0f);
    }
    IEnumerator DashRoutine(Vector2 direction)
    {
        isDashing = true;
        Debug.Log("Dash started!");
        
        dashCooldownTimer = dashCooldown;

        OnDashStart?.Invoke(); // subscriber dikasih tau kalau dash udah mulai

        rb.linearVelocity = direction.normalized * dashForce;

        // tunggu sampai dash selesai
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        OnDashEnd?.Invoke(); // subscriber dikasih tau kalau dash udah selesai
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (!isGrounded)
            {
                isGrounded = true;
                OnLand?.Invoke(); // subscriber dikasih tau kalau player udah mendarat
            }
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