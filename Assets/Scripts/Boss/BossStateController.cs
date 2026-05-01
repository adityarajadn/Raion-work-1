using System;
using System.Collections;
using UnityEngine;

public class BossStateController : MonoBehaviour
{
    [Header("Attack Duration")]
    [SerializeField] private float basicAttackDuration = 2f;
    [SerializeField] private float pattern2Duration = 3f;
    [SerializeField] private float pattern3Duration = 4f;

    bool isBossDead;
    [SerializeField] private float damageBasicAttack = 10;
    [SerializeField] private float damagePattern2 = 15;
    [SerializeField] private float damagePattern3 = 25;

    public static event Action<bool> OnBasicAttack;
    public static event Action<float> basicAttackDamage;
    public static event Action<bool> OnPattern2;
    public static event Action<float> pattern2Damage;
    public static event Action<bool> OnPattern3;
    public static event Action<float> pattern3Damage;

    enum BossPhase
    {
        Phase1,
        Phase2,
        Phase3
    }

    BossPhase currentPhase = BossPhase.Phase1;

    Coroutine patternRoutine;

    void Awake()
    {
    }

    void Start()
    {
        patternRoutine = StartCoroutine(ExecutePattern());
    }

    void OnEnable()
    {
        BossHealth.OnDamaged += HandleDamaged;
        BossHealth.OnDied += HandleDie;
    }

    void OnDisable()
    {
        BossHealth.OnDamaged -= HandleDamaged;
        BossHealth.OnDied -= HandleDie;

        if (patternRoutine != null)
        {
            StopCoroutine(patternRoutine);
            patternRoutine = null;
        }

        StopAllCoroutines();
    }

    void OnDestroy()
    {
        if (patternRoutine != null)
        {
            StopCoroutine(patternRoutine);
            patternRoutine = null;
        }

        StopAllCoroutines();
    }

    void HandleDie(bool isDie)
    {
        isBossDead = isDie;

        if (patternRoutine != null)
        {
            StopCoroutine(patternRoutine);
        }

        ResetAllAttackState();
    }

    void HandleDamaged(float currentHealth, float maxHealth)
    {
        BossPhase newPhase = currentPhase;

        if (currentHealth > maxHealth * 0.5f && currentHealth <= maxHealth) // Phase 1: 100% - 50% HP
        {
            newPhase = BossPhase.Phase1;
        }
        else if (currentHealth > maxHealth * 0.3f && currentHealth <= maxHealth * 0.5f) // Phase 2: 50% - 30% HP
        {
            newPhase = BossPhase.Phase2;
        }
        else
        {
            newPhase = BossPhase.Phase3;
        }

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;

            if (patternRoutine != null)
            {
                StopCoroutine(patternRoutine);
            }

            patternRoutine = StartCoroutine(ExecutePattern());
        }
    }

    IEnumerator ExecutePattern()
    {
        while (!isBossDead)
        {
            switch (currentPhase)
            {
                case BossPhase.Phase1:
                    while (currentPhase != BossPhase.Phase2 && currentPhase != BossPhase.Phase3)
                    yield return StartCoroutine(BasicAttack());
                    break;

                case BossPhase.Phase2:
                    yield return StartCoroutine(BasicAttack());
                    yield return StartCoroutine(Pattern2());
                    break;

                case BossPhase.Phase3:
                    yield return StartCoroutine(BasicAttack());
                    yield return StartCoroutine(Pattern2());
                    yield return StartCoroutine(Pattern3());
                    yield return StartCoroutine(Pattern2());
                    yield return StartCoroutine(BasicAttack());
                    break;
            }
        }
    }

    IEnumerator BasicAttack()
    {
        // Debug.Log("Basic Attack");

        ResetAllAttackState();
        OnBasicAttack?.Invoke(true);
        basicAttackDamage?.Invoke(damageBasicAttack);

        yield return new WaitForSeconds(basicAttackDuration);

        OnBasicAttack?.Invoke(false);
    }

    IEnumerator Pattern2()
    {
        // Debug.Log("Pattern2");

        ResetAllAttackState();
        OnPattern2?.Invoke(true);
        pattern2Damage?.Invoke(damagePattern2);

        yield return new WaitForSeconds(pattern2Duration);

        OnPattern2?.Invoke(false);
    }

    IEnumerator Pattern3()
    {
        // Debug.Log("Pattern3");

        ResetAllAttackState();
        OnPattern3?.Invoke(true);
        pattern3Damage?.Invoke(damagePattern3);

        yield return new WaitForSeconds(pattern3Duration);

        OnPattern3?.Invoke(false);
    }

    void ResetAllAttackState()
    {
        OnBasicAttack?.Invoke(false);
        OnPattern2?.Invoke(false);
        OnPattern3?.Invoke(false);
    }
}