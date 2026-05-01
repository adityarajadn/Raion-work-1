using UnityEngine;
using System;
using System.Collections;

public class PlayerAnimationHandler : AnimatorControllerBase
{
    [Header("Animator Parameters")]
    [SerializeField] private string isRunningParameter = "isRunning";
    [SerializeField] private string isHurtParameter = "isHurt";
    [SerializeField] private string isAttackingParameter = "isAttacking";
    [SerializeField] private string isParryingParameter = "isParrying";
    [SerializeField] private string isOnWallParameter = "isOnWall";
    [SerializeField] private float hurtDuration = 0.15f;
    [SerializeField] private float jumpDuration = 0.1f;
    [SerializeField] private float attackDuration = 0.3f;
    // isDeadParameter and deadDuration are provided by base class

    public static event Action<bool> movingHandler;
    public static event Action<bool> attackHandler;
    public static event Action<float, float> damagedHandler;

    public static event Action<bool> finishDeadAnimation;
    
    void Awake()
    {
        base.Awake();

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
    }

    void HandlePlayerDeath(bool isDead)
    {
        SafeSetBool(isDeadParameter, isDead);
        StartCoroutine(DeadRoutine(isDead));
    }

    protected override void OnDeadFinished(bool isDead)
    {
        finishDeadAnimation?.Invoke(true);
    }

    void HandleWallContactChanged(bool isTouchingWall, string wallSide)
    {
        SafeSetBool(isOnWallParameter, isTouchingWall);
    }

    void HandleMovingChanged(bool isMoving)
    {
        SafeSetBool(isRunningParameter, isMoving);
    }

    void HandleParryingChanged(bool isParrying)
    {
        SafeSetBool(isParryingParameter, isParrying);
        // Debug.Log("Success Parry | " + isParrying);
    }

    void HandleAttackChanged(bool isAttacking)
    {
        SafeSetBool(isAttackingParameter, isAttacking);
    }

    void HandleDamaged(float currentHealth, float maxHealth)
    {
        if (currentHealth <= 0f) return;
        StartCoroutine(PlayHurtState());
    }

    public void handlePlayerJumping(bool isJumping)
    {
        StartCoroutine(PlayJumpState());
    }

    IEnumerator PlayAttackState()
    {
        SafeSetBool(isAttackingParameter, true);
        yield return new WaitForSeconds(attackDuration);
        SafeSetBool(isAttackingParameter, false);
    }

    IEnumerator PlayHurtState()
    {
        SafeSetBool(isHurtParameter, true);
        yield return new WaitForSeconds(hurtDuration);
        SafeSetBool(isHurtParameter, false);
    }

    IEnumerator PlayJumpState()
    {
        SafeSetBool("isJumping", true);
        yield return new WaitForSeconds(jumpDuration);
        SafeSetBool("isJumping", false);
    }

}
