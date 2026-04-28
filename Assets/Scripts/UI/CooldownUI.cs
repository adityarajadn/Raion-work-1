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

    private Coroutine cooldownRoutine;
    public string condition;

    void Awake()
    {
        if (playerAttack == null)
        {
            playerAttack = FindFirstObjectByType<PlayerAttack>();
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
            PlayerAttack.OnAttack += UpdateCooldownUI;    
        }
        
        if (condition == "Parry")
        {
            PlayerParry.OnParry += UpdateCooldownUI;
        }
    }

    void OnDisable()
    {
        PlayerAttack.OnAttack -= UpdateCooldownUI;
        PlayerParry.OnParry -= UpdateCooldownUI;

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }
    }

    void UpdateCooldownUI(float damage)
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

    void UpdateCooldownUI(bool isParrying)
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

    IEnumerator CooldownRoutine(float duration)
    {
        float remaining = duration;
        ui.color = Color.gray;

        while (remaining > 0f)
        {
            cooldownText.text = remaining.ToString("0.0");
            remaining -= Time.deltaTime;
            yield return null;
        }

        ui.color = Color.white;
        cooldownText.text = "";
        cooldownRoutine = null;
    }
}