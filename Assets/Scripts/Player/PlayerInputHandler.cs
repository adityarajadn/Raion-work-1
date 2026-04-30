using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMoveAction;
    public static event Action OnJumpAction;
    public static event Action OnAttackAction;
    public static event Action OnDashAction;
    public static event Action OnParryAction;

    private bool canInput = true;

    void Update()
    {
        if (!canInput) return;
        HandleMoveInput();
        HandleJumpInput();
        HandleAttackInput();
        HandleDashInput();
        HandleParryInput();
    }

    void OnEnable()
    {
        GameUIController.GamePaused += HandleGamePaused;
    }

    void OnDisable()
    {
        GameUIController.GamePaused -= HandleGamePaused;
    }

    void HandleGamePaused(bool isPaused) {
        canInput = !isPaused;
    }

    void HandleMoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        OnMoveAction?.Invoke(new Vector2(horizontal, vertical));
    }

    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            OnJumpAction?.Invoke();
        }
    }

    void HandleAttackInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            OnAttackAction?.Invoke();
        }
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDashAction?.Invoke();
        }
    }

    void HandleParryInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnParryAction?.Invoke();
        }
    }
}