using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private Image ui;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerDashController playerDashController;
    private Coroutine cooldownRoutine;
    [SerializeField] private string condition;

    void Awake()
    {
        if (playerDashController == null)
        {
            playerDashController = FindAnyObjectByType<PlayerDashController>();
        }

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
            PlayerDashController.DashStateChanged += UpdateCooldownUIDash;
        }
    }

    void OnDisable()
    {
        PlayerAttack.OnAttack -= UpdateCooldownUIAttack;
        PlayerParry.OnParry -= UpdateCooldownUIParry;
        PlayerDashController.DashStateChanged -= UpdateCooldownUIDash;

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
        PlayerDashController.DashStateChanged -= UpdateCooldownUIDash;

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

        cooldownRoutine = StartCoroutine(CooldownRoutine(playerAttack.AttackCooldown));
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

        cooldownRoutine = StartCoroutine(CooldownRoutine(playerAttack.AttackCooldown));
    }

    void UpdateCooldownUIDash(bool isDashing)
    {
        if (playerDashController == null || cooldownText == null)
        {
            return;
        }

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
        }

        cooldownRoutine = StartCoroutine(CooldownRoutine(playerDashController.DashCooldown));
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