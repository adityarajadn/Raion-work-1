using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    public Animator animator;
    public EnemyAttackRange attackRange;
    [SerializeField] private string isAttackingParameter = "isAttacking";

    void Awake()
    {
        CacheReferences();
    }

    void CacheReferences()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (attackRange == null)
        {
            attackRange = GetComponentInChildren<EnemyAttackRange>();
        }
    }

    void OnEnable()
    {
        CacheReferences();

        if (attackRange != null)
        {
            attackRange.Attack += handleAttackEvent;
        }
    }

    void OnDisable()
    {
        if (attackRange != null)
        {
            attackRange.Attack -= handleAttackEvent;
        }
    }

    void handleAttackEvent(bool isAttacking)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(isAttackingParameter, isAttacking);
    }

}
