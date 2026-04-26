// ==============================
// BossAnimationController.cs
// ==============================
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [Header("Animator Parameter")]
    public string isAttackingParameter = "isAttacking";
    public string isPattern2Parameter = "isPattern2";
    public string isPattern3Parameter = "isPattern3";

    public Animator animator;

    BossStateController bossStateController;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        bossStateController = GetComponent<BossStateController>();
    }

    void OnEnable()
    {
        if (bossStateController != null)
        {
            bossStateController.OnBasicAttack += HandleBasicAttack;
            bossStateController.OnPattern2 += HandlePattern2;
            bossStateController.OnPattern3 += HandlePattern3;
        }
    }

    void OnDisable()
    {
        if (bossStateController != null)
        {
            bossStateController.OnBasicAttack -= HandleBasicAttack;
            bossStateController.OnPattern2 -= HandlePattern2;
            bossStateController.OnPattern3 -= HandlePattern3;
        }
    }

    void HandleBasicAttack(bool state)
    {
        animator.SetBool(isAttackingParameter, state);
    }

    void HandlePattern2(bool state)
    {
        animator.SetBool(isPattern2Parameter, state);
    }

    void HandlePattern3(bool state)
    {
        animator.SetBool(isPattern3Parameter, state);
    }
}