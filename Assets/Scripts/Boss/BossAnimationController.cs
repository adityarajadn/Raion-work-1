using System;
using UnityEngine;
using GameContracts;

public class BossAnimationController : AnimatorControllerBase, IDeadAnimationSource
{
    [Header("Animator Parameter")]
    [SerializeField] private string isAttackingParameter = "isAttacking";
    [SerializeField] private string isPattern2Parameter = "isPattern2";
    [SerializeField] private string isPattern3Parameter = "isPattern3";
    // isDeadParameter and deadDuration are provided by base class

    [SerializeField] private float hurtDuration = 0.15f;
    public static event Action<bool> isHandlingAttack;
    public event Action<bool> DeadFinished;


    void OnEnable()
    {
        BossStateController.OnBasicAttack += HandleBasicAttack;
        BossStateController.OnPattern2 += HandlePattern2;
        BossStateController.OnPattern3 += HandlePattern3;
        BossHealth.OnDied += HandleDead;
    }

    protected override void OnDisable()
    {
        BossStateController.OnBasicAttack -= HandleBasicAttack;
        BossStateController.OnPattern2 -= HandlePattern2;
        BossStateController.OnPattern3 -= HandlePattern3;
        BossHealth.OnDied -= HandleDead;
        // StopAllCoroutines is handled by base OnDisable

        base.OnDisable();
    }

    void HandleDead(bool isDead)
    {
        SafeSetBool(isDeadParameter, isDead);
        StartCoroutine(DeadRoutine(isDead));
    }

    protected override void OnDeadFinished(bool isDead)
    {
        DeadFinished?.Invoke(isDead);
    }

    void HandleBasicAttack(bool state)
    {
        SafeSetBool(isAttackingParameter, state);
        isHandlingAttack?.Invoke(state);
    }

    void HandlePattern2(bool state)
    {
        SafeSetBool(isPattern2Parameter, state);
        isHandlingAttack?.Invoke(state);
    }

    void HandlePattern3(bool state)
    {
        SafeSetBool(isPattern3Parameter, state);
        isHandlingAttack?.Invoke(state);
    }
}