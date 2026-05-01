using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance { get; private set; }
    [SerializeField] private CinemachineCamera cam;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float globalShakeForce = 1f;
    [SerializeField] private float parryZoomFov = 30f;
    [SerializeField] private float parryZoomDuration = 0.1f;
    [SerializeField] private float parryFreezeDuration = 1f;

    float defaultFov;
    Coroutine parryEffectRoutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (playerHealth == null)
        {
            playerHealth = FindAnyObjectByType<PlayerHealth>();
        }

        if (cam != null)
        {
            defaultFov = cam.Lens.FieldOfView;
        }
    }

    void OnEnable()
    {
        PlayerHealth.OnDamaged += HandlePlayerDamaged;
        PlayerParry.OnParry += HandlePlayerParry;

        BossHealth.OnDamaged += HandleBossDamaged; // Shake camera when boss takes damage as well
    }

    void OnDisable()
    {
        PlayerHealth.OnDamaged -= HandlePlayerDamaged;
        PlayerParry.OnParry -= HandlePlayerParry;

        BossHealth.OnDamaged -= HandleBossDamaged;

        if (parryEffectRoutine != null)
        {
            StopCoroutine(parryEffectRoutine);
            parryEffectRoutine = null;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void HandlePlayerDamaged(float current, float max)
    {
        if (playerHealth == null || playerHealth.ImpulseSource == null)
        {
            return;
        }
        
        CameraShake(playerHealth.ImpulseSource);
    }

    void HandleBossDamaged(float current, float max)
    {
        StartCoroutine(freezeForSeconds(0.1f));
    }
    IEnumerator freezeForSeconds(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    void HandlePlayerParry(bool isParrying)
    {
        if (!isParrying || cam == null || !isActiveAndEnabled)
        {
            return;
        }

        if (parryEffectRoutine != null)
        {
            StopCoroutine(parryEffectRoutine);
        }
        
        parryEffectRoutine = StartCoroutine(ParryEffect());
    }

    IEnumerator ParryEffect()
    {
        if (cam == null)
        {
            parryEffectRoutine = null;
            yield break;
        }

        yield return StartCoroutine(zoom(parryZoomFov, parryZoomDuration)); // zoom in
        yield return StartCoroutine(freeze(parryFreezeDuration));
        yield return StartCoroutine(zoom(defaultFov, parryZoomDuration)); // zoom out
        parryEffectRoutine = null;
    }

    IEnumerator zoom(float targetFOV, float duration)
    {
        if (cam == null)
        {
            yield break;
        }

        float startFov = cam.Lens.FieldOfView;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cam.Lens.FieldOfView = Mathf.Lerp(startFov, targetFOV, t);
            yield return null;
        }

        cam.Lens.FieldOfView = targetFOV;
    }

    IEnumerator freeze(float duration)
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }
}
