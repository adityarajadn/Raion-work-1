using System.Collections;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    public TextMeshProUGUI cooldownText;
    public Image ui;
    public PlayerAttack playerAttack;
    public PlayerMovement playerMovement;
    private Coroutine cooldownRoutine;
    public string condition;

    void Awake()
    {
        if (cooldownText != null)
        {
            cooldownText.text = "";
        }
    }

    void OnEnable()
    {
        if (condition == "BasicAttack")
        {
            PlayerAttack.OnAttack += UpdateCooldownUIAttack;    
        }
        
        if (condition == "Parry")
        {
            PlayerParry.OnParry += UpdateCooldownUIParry;
        }

        if (condition == "Dash")
        {
            PlayerMovement.DashStateChanged += UpdateCooldownUIDash;
        }
    }

    void OnDisable()
    {
        PlayerAttack.OnAttack -= UpdateCooldownUIAttack;
        PlayerParry.OnParry -= UpdateCooldownUIParry;
        PlayerMovement.DashStateChanged -= UpdateCooldownUIDash;

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }

        StopAllCoroutines();
    }

    void OnDestroy()
    {
        PlayerAttack.OnAttack -= UpdateCooldownUIAttack;
        PlayerParry.OnParry -= UpdateCooldownUIParry;
        PlayerMovement.DashStateChanged -= UpdateCooldownUIDash;

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }

        StopAllCoroutines();
    }

    void UpdateCooldownUIAttack(float damage)
    {
        if (playerAttack == null || cooldownText == null)
        {
            return;
        }

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }

        cooldownRoutine = StartCoroutine(CooldownRoutine(playerAttack.attackCooldown));
    }

    void UpdateCooldownUIParry(bool isParrying)
    {
        if (playerAttack == null || cooldownText == null)
        {
            return;
        }

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }

        cooldownRoutine = StartCoroutine(CooldownRoutine(playerAttack.attackCooldown));
    }

    void UpdateCooldownUIDash(bool isDashing)
    {
        if (playerMovement == null || cooldownText == null)
        {
            return;
        }

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }

        cooldownRoutine = StartCoroutine(CooldownRoutine(playerMovement.dashCooldown));
    }

    IEnumerator CooldownRoutine(float duration)
    {
        if (ui == null || cooldownText == null)
        {
            cooldownRoutine = null;
            yield break;
        }

        float remaining = duration;
        ui.color = Color.gray;

        while (remaining > 0f)
        {
            cooldownText.text = remaining.ToString("0.0");
            remaining -= Time.deltaTime;
            yield return null;
        }

        if (ui == null || cooldownText == null)
        {
            cooldownRoutine = null;
            yield break;
        }

        ui.color = Color.white;
        cooldownText.text = "";
        cooldownRoutine = null;
    }
}