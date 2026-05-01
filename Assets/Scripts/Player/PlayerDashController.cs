using System;
using UnityEngine;

public class PlayerDashController : MonoBehaviour
{
    public static event Action<bool> DashStateChanged;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.02f;
    [SerializeField] private float dashCooldown = 0.75f;
    [SerializeField] private Rigidbody2D rb;

    private bool isDashing;
    private bool isDead;
    private float dashTimer;
    private float nextDashTime;
    private float originalGravityScale;
    private bool facingRight = true;

    public float DashCooldown => dashCooldown;

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

    void OnEnable()
    {
        PlayerInputHandler.OnDashAction += HandleDashInput;
        PlayerHealth.OnDied += HandleDead;
        PlayerMovement.PlayerFacingRight += HandleFacingChanged;
    }

    void OnDisable()
    {
        PlayerInputHandler.OnDashAction -= HandleDashInput;
        PlayerHealth.OnDied -= HandleDead;
        PlayerMovement.PlayerFacingRight -= HandleFacingChanged;

        if (isDashing)
        {
            EndDash();
        }
    }

    void HandleDead(bool isDead)
    {
        this.isDead = isDead;

        if (isDead && isDashing)
        {
            EndDash();
        }
    }

    void HandleFacingChanged(bool isFacingRight)
    {
        facingRight = isFacingRight;
    }

    void Update()
    {
        DashTick();
    }

    void HandleDashInput()
    {
        if (isDead || isDashing || Time.time < nextDashTime || rb == null)
        {
            return;
        }

        StartDash();
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        nextDashTime = Time.time + dashCooldown;
        rb.gravityScale = 0f;
        DashStateChanged?.Invoke(true);
    }

    void DashTick()
    {
        if (!isDashing || rb == null)
        {
            return;
        }

        dashTimer -= Time.deltaTime;
        rb.linearVelocity = new Vector2(facingRight ? dashSpeed : -dashSpeed, 0f);

        if (dashTimer <= 0f)
        {
            EndDash();
        }
    }

    void EndDash()
    {
        isDashing = false;

        if (rb != null)
        {
            rb.gravityScale = originalGravityScale;
        }

        DashStateChanged?.Invoke(false);
    }
}
