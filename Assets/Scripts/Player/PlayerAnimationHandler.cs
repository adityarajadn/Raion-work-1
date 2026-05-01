using UnityEngine;
using System;
using System.Collections;

public class PlayerAnimationHandler : MonoBehaviour
{
    public Animator animator;

    [Header("Animator Parameters")]
    [SerializeField] private string isRunningParameter = "isRunning";
    [SerializeField] private string isHurtParameter = "isHurt";
    [SerializeField] private string isAttackingParameter = "isAttacking";
    [SerializeField] private string isParryingParameter = "isParrying";
    [SerializeField] private string isOnWallParameter = "isOnWall";
    [SerializeField] private float hurtDuration = 0.15f;
    [SerializeField] private float jumpDuration = 0.1f;
    [SerializeField] private string isDeadParameter = "isDead";

    public static event Action<bool> movingHandler;
    public static event Action<bool> attackHandler;
    public static event Action<float, float> damagedHandler;
    public float deadDuration = 5f;

    public static event Action<bool> finishDeadAnimation;
    
    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        movingHandler = HandleMovingChanged;
        attackHandler = HandleAttackChanged;
        damagedHandler = HandleDamaged;


        this.enabled = true;

    }

    void OnEnable()
    {
        PlayerMovement.OnMoving += movingHandler;
        PlayerMovement.OnJump += handlePlayerJumping;
        PlayerAttack.AttackStateChanged += attackHandler;
        PlayerHealth.OnDamaged += damagedHandler;
        PlayerWallCheck.OnWallContact += HandleWallContactChanged;
        PlayerParry.OnParry += HandleParryingChanged;
        PlayerHealth.OnDied += HandlePlayerDeath;
    }

    void OnDisable()
    {
        PlayerMovement.OnMoving -= movingHandler;
        PlayerMovement.OnJump -= handlePlayerJumping;
        PlayerAttack.AttackStateChanged -= attackHandler;
        PlayerHealth.OnDamaged -= damagedHandler;
        PlayerWallCheck.OnWallContact -= HandleWallContactChanged;
        PlayerParry.OnParry -= HandleParryingChanged;
        PlayerHealth.OnDied -= HandlePlayerDeath;

        StopAllCoroutines();
    }

    void HandlePlayerDeath(bool isDead)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isDeadParameter, isDead);
        StartCoroutine(PlayDeathState());
    }

    IEnumerator PlayDeathState()
    {
        if (animator == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(deadDuration); // Durasi animasi mati, sesuaikan dengan animasi yang digunakan
        if (animator == null)
        {
            yield break;
        }
        finishDeadAnimation?.Invoke(true);
    }

    void HandleWallContactChanged(bool isTouchingWall, string wallSide)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isOnWallParameter, isTouchingWall);
    }

    void HandleMovingChanged(bool isMoving)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isRunningParameter, isMoving);
    }

    void HandleParryingChanged(bool isParrying)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isParryingParameter, isParrying);
        // Debug.Log("Success Parry | " + isParrying);
    }

    void HandleAttackChanged(bool isAttacking)
    {
        if (animator == null)
        {
            return;
        }
        animator.SetBool(isAttackingParameter, isAttacking);    
        
    }

    void HandleDamaged(float currentHealth, float maxHealth)
    {
        if (animator == null)
        {
            return;
        }

        if (currentHealth <= 0f)
        {
            return;
        }

        StartCoroutine(PlayHurtState());
    }

    public void handlePlayerJumping(bool isJumping)
    {
        if (animator == null)
        {
            return;
        }

        StartCoroutine(PlayJumpState());
    }

    IEnumerator PlayHurtState()
    {
        if (animator == null)
        {
            yield break;
        }

        animator.SetBool(isHurtParameter, true);
        yield return new WaitForSeconds(hurtDuration);
        if (animator == null)
        {
            yield break;
        }
        animator.SetBool(isHurtParameter, false);
    }

    IEnumerator PlayJumpState()
    {
        if (animator == null)
        {
            yield break;
        }

        animator.SetBool("isJumping", true);
        yield return new WaitForSeconds(jumpDuration);
        if (animator == null)
        {
            yield break;
        }
        animator.SetBool("isJumping", false);
    }

}
