using System;
using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    
    public event Action<float, float> Damaged;
    public event Action<bool> OnDamaged;
    public event Action Died;

    protected virtual void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (damage <= 0f || CurrentHealth <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
        Damaged?.Invoke(CurrentHealth, MaxHealth);
        OnDamaged?.Invoke(true);

        if (CurrentHealth <= 0f)
        {
            Died?.Invoke();
            die();
        }
    }

    public void die()
    {
        //
    }
}
