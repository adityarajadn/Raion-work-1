using UnityEngine;
using UnityEngine.InputSystem;

public class BossSFXController : MonoBehaviour
{
    public AudioSource[] audioSources;

    void OnEnable()
    {
        BossStateController.OnBasicAttack += HandleBasicAttack;
        BossHealth.OnDied += HandleDead;
        BossHealth.OnDamaged += HandleDamaged;
        BossStateController.OnPattern2 += HandlePattern2;
    }

    void OnDisable()
    {
        BossStateController.OnBasicAttack -= HandleBasicAttack;
        BossHealth.OnDied -= HandleDead;
        BossHealth.OnDamaged -= HandleDamaged;
        BossStateController.OnPattern2 -= HandlePattern2;

        StopAllCoroutines();
    }

    void HandlePattern2(bool isPattern2)
    {
        if (!isPattern2) return;
        PlaySFX(3);
    }
    
    void HandleBasicAttack(bool isAttacking)
    {
        if (!isAttacking) return;
        PlaySFX(0);
    }

    void HandleDead(bool isDead)
    {
        if (!isDead) return;
        PlaySFX(1);
    }

    void HandleDamaged(float currentHealth, float maxHealth)
    {
        if (currentHealth <= 0) return;
        PlaySFX(2);
    }

    void PlaySFX(int index)
    {
        if (audioSources == null || index < 0 || index >= audioSources.Length)
        {
            Debug.LogWarning("AudioSource array is not properly set up or index is out of range.");
            return;
        }

        audioSources[index].Play();
    }
}
