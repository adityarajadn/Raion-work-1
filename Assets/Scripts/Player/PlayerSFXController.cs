using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSFXController : SFXControllerBase
{
    protected override void RegisterEvents()
    {
        PlayerAttack.OnAttackStarted += HandleBasicAttack;
        PlayerHealth.OnDied += HandleDead;
        PlayerHealth.OnDamaged += HandleDamaged;
    }

    protected override void UnregisterEvents()
    {
        PlayerAttack.OnAttackStarted -= HandleBasicAttack;
        PlayerHealth.OnDied -= HandleDead;
        PlayerHealth.OnDamaged -= HandleDamaged;
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

    // uses PlaySFX from SFXControllerBase
}
