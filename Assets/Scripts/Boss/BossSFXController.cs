public class BossSFXController : SFXControllerBase
{
    protected override void RegisterEvents()
    {
        BossStateController.OnBasicAttack += HandleBasicAttack;
        BossHealth.OnDied += HandleDead;
        BossHealth.OnDamaged += HandleDamaged;
        BossStateController.OnPattern2 += HandlePattern2;
    }

    protected override void UnregisterEvents()
    {
        BossStateController.OnBasicAttack -= HandleBasicAttack;
        BossHealth.OnDied -= HandleDead;
        BossHealth.OnDamaged -= HandleDamaged;
        BossStateController.OnPattern2 -= HandlePattern2;
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

}
