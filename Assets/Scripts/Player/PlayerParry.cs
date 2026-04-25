using System;
using System.Collections;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    public Collider2D parryCollider;
    public bool isParrying;
    public bool parrySuccess;
    public float parryDuration = 0.3f;

    public event Action<bool> OnParry;
    private Coroutine parryRoutine;

    void Awake()
    {
        if (parryCollider != null)
        {
            parryCollider.enabled = false;
        }
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isParrying)
        {
            parryRoutine = StartCoroutine(ParryRoutine());
        }
    }

    IEnumerator ParryRoutine()
    {
        isParrying = true;
        parrySuccess = false;

        if (parryCollider != null)
        {
            parryCollider.enabled = true;
        }

        yield return new WaitForSeconds(parryDuration);

        isParrying = false;
        if (parryCollider != null)
        {
            parryCollider.enabled = false;
        }

        OnParry?.Invoke(parrySuccess);
        parryRoutine = null;
    }

    public bool TryParry()
    {
        if (!isParrying)
        {
            return false;
        }

        parrySuccess = true;
        isParrying = false;

        if (parryCollider != null)
        {
            parryCollider.enabled = false;
        }

        if (parryRoutine != null)
        {
            StopCoroutine(parryRoutine);
            parryRoutine = null;
        }

        OnParry?.Invoke(true);
        return true;
    }
}
