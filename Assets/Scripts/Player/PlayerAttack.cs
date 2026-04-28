using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static event Action<float> OnAttack;
    public static event Action OnAttackStarted;
    public static event Action OnAttackEnded;
    public event Action<bool> AttackStateChanged;
    
    public float damageAmount = 25f;
    public Collider2D hitBox;
    private bool isAttacking = false;
    private float attackCooldown = 0.5f;
    public bool canAttack = true;

    public float attackDuration = 0.2f;


    void Awake()
    {
        if (hitBox != null)
        {
            hitBox.enabled = false;
        }
    }

    void Update()
    {
        handleInput();
    }

    // Method untuk menangani input serangan
    void handleInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && canAttack) // Cek input serangan, pastikan tidak sedang menyerang dan cooldown sudah selesai
        {
            StartCoroutine(AttackingRoutine());
        }
    }

    // Coroutine untuk memulai serangan
    IEnumerator AttackingRoutine()
    {
        SetAttackState(true); // Ubah state serangan menjadi true saat memulai serangan
        yield return new WaitForSeconds(attackDuration); // Durasi serangan, bisa disesuaikan dengan animasi atau kebutuhan
        SetAttackState(false); // Ubah state serangan menjadi false setelah durasi serangan selesai
    }

    // Method untuk mengubah state serangan dan memanggil event terkait
    void SetAttackState(bool value)
    {
        if (isAttacking == value)
        {
            return;
        }

        isAttacking = value; // Update state terlebih dahulu sebelum memanggil event
        StartCoroutine(AttackCooldownRoutine()); // Mulai cooldown setelah mengubah state

        if (hitBox != null)
        {
            hitBox.enabled = isAttacking; // Aktifkan hitbox saat menyerang, nonaktifkan saat tidak menyerang
        }

        if (isAttacking)
        {
            OnAttackStarted?.Invoke(); // Panggil event saat serangan dimulai
        }
        else
        {
            OnAttackEnded?.Invoke(); // Panggil event saat serangan berakhir    
        }
    }

    // Coroutine untuk mengatur cooldown serangan
    IEnumerator AttackCooldownRoutine()
    {
        canAttack = false; // Nonaktifkan kemampuan untuk menyerang selama cooldown
        AttackStateChanged?.Invoke(isAttacking); // Panggil event perubahan state serangan
        OnAttack?.Invoke(damageAmount); // Panggil event serangan dengan jumlah damage yang ditentukan
        yield return new WaitForSeconds(attackCooldown); // Tunggu selama durasi cooldown
        canAttack = true; // Aktifkan kembali kemampuan untuk menyerang setelah cooldown selesai
    }
}
