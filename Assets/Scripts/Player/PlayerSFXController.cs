using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSFXController : MonoBehaviour
{
    public AudioSource[] audioSources;

    void OnEnable()
    {
        PlayerAttack.OnAttackStarted += HandleBasicAttack;
        PlayerHealth.OnDied += HandleDead;
        PlayerHealth.OnDamaged += HandleDamaged;
    }

    void OnDisable()
    {
        PlayerInputHandler.OnAttackAction -= HandleBasicAttack;
        PlayerHealth.OnDied -= HandleDead;
        PlayerHealth.OnDamaged -= HandleDamaged;

        StopAllCoroutines();
    }

    void HandleBasicAttack()
    {
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
