using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 100f;
    private float currentHealth;
    public static event Action<bool> OnDamage;
    public CinemachineImpulseSource impulseSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        CameraShakeManager.instance.CameraShake(impulseSource);
        currentHealth -= damage;
        OnDamage?.Invoke(true); // Memanggil event dengan status terkena damage
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()  
    {
        Debug.Log("Player has died.");
    }
    
}
