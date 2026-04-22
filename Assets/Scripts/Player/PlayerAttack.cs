using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static event Action<bool> OnAttack; // Event untuk memberitahu saat serangan dimulai atau berhenti
    public Collider2D hitBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitBox.enabled = false;
    }

    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        bool attacking = Input.GetKey(KeyCode.Mouse0);
        hitBox.enabled = attacking;
        OnAttack?.Invoke(attacking); // Memanggil event dengan status serangan saat ini
    }
}
