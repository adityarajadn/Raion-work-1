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

    private Action<bool> movingHandler;
    private Action<bool> attackHandler;
    private Action<float, float> damagedHandler;
    
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
    }

    void OnDisable()
    {
        PlayerMovement.OnMoving -= movingHandler;
        PlayerMovement.OnJump -= handlePlayerJumping;
        PlayerAttack.AttackStateChanged -= attackHandler;
        PlayerHealth.OnDamaged -= damagedHandler;
        PlayerWallCheck.OnWallContact -= HandleWallContactChanged;
        PlayerParry.OnParry -= HandleParryingChanged;
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
