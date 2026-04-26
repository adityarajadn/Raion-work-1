using UnityEngine;
using System;
using System.Collections;

public class PlayerAnimationHandler : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

    [Header("Animator Parameters")]
    [SerializeField] private string isRunningParameter = "isRunning";
    [SerializeField] private string isHurtParameter = "isHurt";
    [SerializeField] private string isAttackingParameter = "isAttacking";
    [SerializeField] private float hurtDuration = 0.15f;
    [SerializeField] private float jumpDuration = 0.1f;

    private Action<bool> movingHandler;
    private Action<bool> attackHandler;
    private Action<float, float> damagedHandler;
    private Coroutine hurtRoutine;


    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();

        movingHandler = HandleMovingChanged;
        attackHandler = HandleAttackChanged;
        damagedHandler = HandleDamaged;
    }

    void OnEnable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnMoving += movingHandler;
            playerMovement.OnJump += handlePlayerJumping;
            playerMovement.OnWallJump += handlePlayerJumping;
        }

        if (playerAttack != null)
        {
            playerAttack.AttackStateChanged += attackHandler;

        }

        if (playerHealth != null)
        {
            playerHealth.OnDamaged += damagedHandler;
        }
        
    }

    void OnDisable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnMoving -= movingHandler;
            playerMovement.OnJump -= handlePlayerJumping;
            playerMovement.OnWallJump -= handlePlayerJumping;
        }

        if (playerAttack != null)
        {
            playerAttack.AttackStateChanged -= attackHandler;
        }

        if (playerHealth != null)
        {
            playerHealth.OnDamaged -= damagedHandler;
        }
    }

    void HandleMovingChanged(bool isMoving)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isRunningParameter, isMoving);
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

        if (hurtRoutine != null)
        {
            StopCoroutine(hurtRoutine);
        }

        hurtRoutine = StartCoroutine(PlayHurtState());
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
        animator.SetBool(isHurtParameter, true);
        yield return new WaitForSeconds(hurtDuration);
        animator.SetBool(isHurtParameter, false);
        hurtRoutine = null;
    }

    IEnumerator PlayJumpState()
    {
        animator.SetBool("isJumping", true);
        yield return new WaitForSeconds(jumpDuration);
        animator.SetBool("isJumping", false);
    }

}
