using UnityEngine;

public class PlayerHealthBarController : MonoBehaviour
{
    
    public GameObject healthBar3of3;
    public GameObject healthBar2of3;
    public GameObject healthBar1of3;
    public GameObject healthBar0of3;

    void Awake()
    {
        healthBar0of3.SetActive(false);
        healthBar1of3.SetActive(false);
        healthBar2of3.SetActive(false);
        healthBar3of3.SetActive(true);
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
        if (state == 3)
        {
            healthBar3of3.SetActive(true);
            healthBar2of3.SetActive(false);
            healthBar1of3.SetActive(false);
            healthBar0of3.SetActive(false);
        }
        else if (state == 2)
        {
            healthBar3of3.SetActive(false);
            healthBar2of3.SetActive(true);
            healthBar1of3.SetActive(false);
            healthBar0of3.SetActive(false);
        }
        else if (state == 1)
        {
            healthBar3of3.SetActive(false);
            healthBar2of3.SetActive(false);
            healthBar1of3.SetActive(true);
            healthBar0of3.SetActive(false);
        }
        else // state == 0
        {
            healthBar3of3.SetActive(false);
            healthBar2of3.SetActive(false);
            healthBar1of3.SetActive(false);
            healthBar0of3.SetActive(true);
        }
    }
}
