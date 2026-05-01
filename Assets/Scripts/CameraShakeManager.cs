using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    public CinemachineCamera cam;

    public PlayerHealth playerHealth;
    public float globalShakeForce = 1f;
    public float parryZoomFov = 30f;
    public float parryZoomDuration = 0.1f;
    public float parryFreezeDuration = 1f;

    float defaultFov;
    Coroutine parryEffectRoutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
    }

    void OnDisable()
    {
        PlayerHealth.OnDamaged -= HandlePlayerDamaged;
        PlayerParry.OnParry -= HandlePlayerParry;

        if (parryEffectRoutine != null)
        {
            StopCoroutine(parryEffectRoutine);
            parryEffectRoutine = null;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    void HandlePlayerDamaged(float current, float max)
    {
        if (playerHealth == null || playerHealth.impulseSource == null)
        {
            return;
        }

        CameraShake(playerHealth.impulseSource);
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
