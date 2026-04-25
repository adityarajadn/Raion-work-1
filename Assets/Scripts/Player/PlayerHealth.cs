using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : DamageableEntity
{
    public static event Action<float, float> OnPlayerDamaged;
    public static event Action OnPlayerDied;

    public CinemachineImpulseSource impulseSource;

    protected override void Awake()
    {
        base.Awake();

        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }
    }

    void OnEnable()
    {
        Damaged += HandleDamaged;
        Died += HandleDied;
    }

    void OnDisable()
    {
        Damaged -= HandleDamaged;
        Died -= HandleDied;
    }

    void HandleDamaged(float current, float max)
    {
        OnPlayerDamaged?.Invoke(current, max);
    }

    void HandleDied()
    {
        OnPlayerDied?.Invoke();
    }

    protected override void OnDeath()
    {
        Debug.Log("Player has died.");
    }
}
