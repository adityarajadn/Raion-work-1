using System;
using Unity.Cinemachine;
using UnityEngine;

public interface IDamageableEntity
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    event Action<float, float> Damaged;
    event Action Died;
    void TakeDamage(float damage);
}

public interface IAttackController
{
    bool IsAttacking { get; }
    event Action<bool> AttackStateChanged;
}

public interface IAttackDamageSource
{
    float DamageAmount { get; }
}

public interface ICollisionDamageDealer
{
    void TryDealDamage(Collider2D collision);
}

public interface IDamageFeedback
{
    void PlayDamageFeedback(float currentHealth, float maxHealth);
}

public interface ICameraImpulseShaker
{
    float GlobalShakeForce { get; }
    void CameraShake(CinemachineImpulseSource impulseSource);
}

public interface IMovementController
{
    Vector2 MoveDirection { get; }
    bool FacingRight { get; }
}

public interface IDashController
{
    bool IsDashing { get; }
    event Action<bool> DashStateChanged;
}

public interface IWallContactSensor
{
    bool IsTouchingWall { get; }
    int WallSide { get; }
    event Action<bool, int> WallContactChanged;
}