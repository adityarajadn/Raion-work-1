using System.Collections;
using UnityEngine;

public class PlayerDamageFeedback : MonoBehaviour
{
    public SpriteRenderer targetRenderer;
    public Color normalColor = Color.white;
    public Color damageColor = Color.red;
    public float flashDuration = 0.12f;
    

    private Coroutine flashRoutine;

    void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void OnEnable()
    {
        PlayerHealth.OnDamaged += HandleDamageEvent;
    }

    void OnDisable()
    {
        PlayerHealth.OnDamaged -= HandleDamageEvent;

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }

        StopAllCoroutines();
    }

    void OnDestroy()
    {
        PlayerHealth.OnDamaged -= HandleDamageEvent;

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
    }

    void HandleDamageEvent(float currentHealth, float maxHealth)
    {
        PlayDamageFeedback(currentHealth, maxHealth);
    }

    public void PlayDamageFeedback(float currentHealth, float maxHealth)
    {
        if (targetRenderer == null || !isActiveAndEnabled)
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
        if (targetRenderer == null)
        {
            flashRoutine = null;
            yield break;
        }

        targetRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);

        if (targetRenderer == null)
        {
            flashRoutine = null;
            yield break;
        }

        targetRenderer.color = normalColor;
        flashRoutine = null;
    }
}
