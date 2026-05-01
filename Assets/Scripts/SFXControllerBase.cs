using UnityEngine;

// Base class for SFX controllers to centralize AudioSource handling
public abstract class SFXControllerBase : MonoBehaviour
{
    [SerializeField] protected AudioSource[] audioSources;

    protected virtual void OnEnable()
    {
        RegisterEvents();
    }

    protected virtual void OnDisable()
    {
        UnregisterEvents();
        StopAllCoroutines();
    }

    protected void PlaySFX(int index)
    {
        if (audioSources == null || index < 0 || index >= audioSources.Length)
        {
            return;
        }

        var src = audioSources[index];
        if (src == null)
        {
            return;
        }

        src.Play();
    }

    protected abstract void RegisterEvents();
    protected abstract void UnregisterEvents();
}
