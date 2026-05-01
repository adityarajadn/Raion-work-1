using UnityEngine;

public class PlayerHealthBarController : MonoBehaviour
{
    [SerializeField] private GameObject[] healthBarStates;

    void Awake()
    {
        if (healthBarStates == null || healthBarStates.Length < 4)
        {
            return;
        }

        healthBarStates[0].SetActive(false); // hp empty
        healthBarStates[1].SetActive(false);
        healthBarStates[2].SetActive(false);
        healthBarStates[3].SetActive(true); // hp full
    }

    void OnEnable()
    {
        PlayerHealth.OnDamaged += UpdateHealthBar;
    }

    void OnDisable()
    {
        PlayerHealth.OnDamaged -= UpdateHealthBar;
    }

    void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarStates == null || healthBarStates.Length < 4 || maxHealth <= 0f)
        {
            return;
        }

        float healthPercentage = currentHealth / maxHealth;

        if (healthPercentage > 0.66f)
            SetHealthBarState(3);
        else if (healthPercentage > 0.33f)
            SetHealthBarState(2);
        else if (healthPercentage > 0f)
            SetHealthBarState(1);
        else
            SetHealthBarState(0);
    }

    void SetHealthBarState(int state)
    {
        if (healthBarStates == null || healthBarStates.Length < 4)
        {
            return;
        }

        if (state == 3)
        {
            healthBarStates[3].SetActive(true);
            for (int i = 0; i < healthBarStates.Length; i++)
            {
                if (i == 3) continue;
                healthBarStates[i].SetActive(false);
            }
        }
        else if (state == 2)
        {
            healthBarStates[2].SetActive(true);
            for (int i = 0; i < healthBarStates.Length; i++)
            {
                if (i == 2) continue;
                healthBarStates[i].SetActive(false);
            }
        }
        else if (state == 1)
        {
            healthBarStates[1].SetActive(true);
            for (int i = 0; i < healthBarStates.Length; i++)
            {
                if (i == 1) continue;
                healthBarStates[i].SetActive(false);
            }
        }
        else // state == 0
        {
            healthBarStates[0].SetActive(true);
            for (int i = 0; i < healthBarStates.Length; i++)
            {
                if (i == 0) continue;
                healthBarStates[i].SetActive(false);
            }
        }
    }
}
