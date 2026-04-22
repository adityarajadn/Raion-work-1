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
        PlayerHealth.OnDamage += HandleDamageEvent;
    }

    void OnDisable()
    {
        PlayerHealth.OnDamage -= HandleDamageEvent;
    }

    void HandleDamageEvent(bool isDamaged)
    {
        if (!isDamaged || targetRenderer == null)
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
