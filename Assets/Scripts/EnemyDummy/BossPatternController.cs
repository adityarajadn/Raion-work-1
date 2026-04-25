using System.Collections;
using UnityEngine;

public class BossPatternController : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rb;
    public Animator animator;
    public EnemyDummyMain bossHealth;

    public float moveSpeed = 3f;
    [SerializeField] private string pattern2Parameter = "isPattern2";
    [SerializeField] private string pattern3Parameter = "isPattern3";
    [SerializeField] private float pattern2Threshold = 0.7f;
    [SerializeField] private float pattern3Threshold = 0.4f;

    enum BossState
    {
        Idle,
        Chase,
        Pattern1,
        Pattern2,
        Pattern3,
        Dead
    }

    private BossState bossState = BossState.Idle;
    private Coroutine bossLoopRoutine;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (bossHealth == null)
        {
            bossHealth = GetComponent<EnemyDummyMain>();
        }
    }

    void OnEnable()
    {
        if (bossLoopRoutine == null)
        {
            bossLoopRoutine = StartCoroutine(BossLoop());
        }
    }

    void OnDisable()
    {
        if (bossLoopRoutine != null)
        {
            StopCoroutine(bossLoopRoutine);
            bossLoopRoutine = null;
        }

        SetPhaseAnimation(false, false);
    }

    IEnumerator BossLoop()
    {
        while (bossState != BossState.Dead)
        {
            if (bossHealth == null)
            {
                yield return null;
                continue;
            }

            float healthRatio = bossHealth.CurrentHealth / Mathf.Max(1f, bossHealth.MaxHealth);

            if (bossHealth.CurrentHealth <= 0f)
            {
                bossState = BossState.Dead;
                SetPhaseAnimation(false, false);
                yield break;
            }

            if (healthRatio <= pattern3Threshold)
            {
                bossState = BossState.Pattern3;
            }
            else if (healthRatio <= pattern2Threshold)
            {
                bossState = BossState.Pattern2;
            }
            else
            {
                bossState = BossState.Pattern1;
            }

            SetPhaseAnimation(
                bossState == BossState.Pattern2,
                bossState == BossState.Pattern3
            );

            HandleChase();
            yield return null;
        }
    }

    void HandleChase()
    {
        if (rb == null || target == null || bossState == BossState.Dead)
        {
            return;
        }

        Vector2 toTarget = (target.position - transform.position);
        if (toTarget.sqrMagnitude <= 0.01f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = toTarget.normalized * moveSpeed;
    }

    void SetPhaseAnimation(bool isPattern2, bool isPattern3)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(pattern2Parameter, isPattern2);
        animator.SetBool(pattern3Parameter, isPattern3);
    }
}
