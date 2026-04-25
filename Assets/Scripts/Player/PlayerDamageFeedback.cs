using System.Collections;
using UnityEngine;

public class PlayerDamageFeedback : MonoBehaviour, IDamageFeedback
{
    public PlayerHealth playerHealth;
    public SpriteRenderer targetRenderer;
    public Color normalColor = Color.white;
    public Color damageColor = Color.red;
    public float flashDuration = 0.12f;
    

    private Coroutine flashRoutine;

    void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.Damaged += HandleDamageEvent;
        }
    }

    void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.Damaged -= HandleDamageEvent;
        }
    }

    void HandleDamageEvent(float currentHealth, float maxHealth)
    {
        PlayDamageFeedback(currentHealth, maxHealth);
    }

    public void PlayDamageFeedback(float currentHealth, float maxHealth)
    {
        if (targetRenderer == null)
        {
            return;
        }

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashDamage());
    }

    IEnumerator FlashDamage()
    {
        targetRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        targetRenderer.color = normalColor;
        flashRoutine = null;
    }
}
