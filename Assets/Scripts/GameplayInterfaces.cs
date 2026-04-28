using System;
using UnityEngine;

public interface IAttackController
{
    bool IsAttacking { get; }
    event Action<bool> AttackStateChanged;
}

public interface IDamageFeedback
{
    void PlayDamageFeedback(float currentHealth, float maxHealth);
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