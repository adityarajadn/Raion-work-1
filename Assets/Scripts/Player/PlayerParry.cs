using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    bool isParrying = false;
    bool canParry = false;
    [SerializeField] private float parryDuration = 0.3f;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    public static event Action<bool> OnParry;

    void OnEnable()
    {
        PlayerInputHandler.OnParryAction += HandleParryInput;
    }

    void OnDisable()
    {
        PlayerInputHandler.OnParryAction -= HandleParryInput;
    }

    void HandleParryInput()
    {
        if (!isParrying && canParry)
        {
            StartCoroutine(ParryRoutine());
        }
    }

    IEnumerator ParryRoutine()
    {
        isParrying = true; // player ga dibolehin input E lagi sampe parry selesai
        OnParry?.Invoke(isParrying);

        yield return new WaitForSeconds(parryDuration);

        isParrying = false;
        OnParry?.Invoke(isParrying);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttackHitBox"))
        {
            canParry = true;
            // Debug.Log("Can Parry");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttackHitBox"))
        {
            canParry = false;
            // Debug.Log("Can't Parry");
        }
    }
}