using UnityEngine;
using System;
using System.Collections;

public class PlayerAnimationHandler : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;
    private PlayerWallCheck playerWallCheck;
    private PlayerParry playerParry;

    [Header("Animator Parameters")]
    [SerializeField] private string isRunningParameter = "isRunning";
    [SerializeField] private string isHurtParameter = "isHurt";
    [SerializeField] private string isAttackingParameter = "isAttacking";
    [SerializeField] private string isParryingParameter = "isParrying";
    [SerializeField] private string isOnWallParameter = "isOnWall";
    [SerializeField] private float hurtDuration = 0.15f;
    [SerializeField] private float jumpDuration = 0.1f;

    private Action<bool> movingHandler;
    private Action<bool> attackHandler;
    private Action<float, float> damagedHandler;
    private Action<bool> parryHandler;


    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        if (playerAttack == null)
        {
            playerAttack = GetComponent<PlayerAttack>();
        }

        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        if (playerWallCheck == null)
        {
            playerWallCheck = GetComponent<PlayerWallCheck>();
        }

        if (playerParry == null)
        {
            playerParry = GetComponent<PlayerParry>();
        }

        movingHandler = HandleMovingChanged;
        attackHandler = HandleAttackChanged;
        damagedHandler = HandleDamaged;
        parryHandler = HandleParryingChanged;

    }

    void OnEnable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnMoving += movingHandler;
            playerMovement.OnJump += handlePlayerJumping;
        }

        if (playerAttack != null)
        {
            playerAttack.AttackStateChanged += attackHandler;

        }

        if (playerHealth != null)
        {
            playerHealth.OnDamaged += damagedHandler;
        }

        if (playerWallCheck != null)
        {
            playerWallCheck.OnWallContact += HandleWallContactChanged;
        }

        if (playerParry != null)
        {
            playerParry.OnParry += parryHandler;
        }
        
    }

    void OnDisable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnMoving -= movingHandler;
            playerMovement.OnJump -= handlePlayerJumping;
        }

        if (playerAttack != null)
        {
            playerAttack.AttackStateChanged -= attackHandler;
        }

        if (playerHealth != null)
        {
            playerHealth.OnDamaged -= damagedHandler;
        }

        if (playerWallCheck != null)
        {
            playerWallCheck.OnWallContact -= HandleWallContactChanged;
        }

        if (playerParry != null)
        {
            playerParry.OnParry -= parryHandler;
        }
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

        Debug.Log("Parry State Changed: " + isParrying);
        animator.SetBool(isParryingParameter, isParrying);
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
        animator.SetBool(isHurtParameter, true);
        yield return new WaitForSeconds(hurtDuration);
        animator.SetBool(isHurtParameter, false);
    }

    IEnumerator PlayJumpState()
    {
        animator.SetBool("isJumping", true);
        yield return new WaitForSeconds(jumpDuration);
        animator.SetBool("isJumping", false);
    }

}
