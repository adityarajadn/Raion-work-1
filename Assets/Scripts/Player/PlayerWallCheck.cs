using UnityEngine;
using System;

public class PlayerWallCheck : MonoBehaviour, IWallContactSensor
{
    public event Action<bool, int> WallContactChanged;

    public Collider2D wallCheckCollider;
    public Transform playerRoot;

    private bool isTouchingWall;
    private int wallSide;
    public bool IsTouchingWall => isTouchingWall;
    public int WallSide => wallSide;

    void Awake()
    {
        if (wallCheckCollider == null)
        {
            wallCheckCollider = GetComponent<Collider2D>();
        }

        if (playerRoot == null)
        {
            var movement = GetComponentInParent<PlayerMovement>();
            playerRoot = movement != null ? movement.transform : transform.parent;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            UpdateWallState(collision);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            UpdateWallState(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SetWallState(false, 0);
        }
    }

    void UpdateWallState(Collider2D collision)
    {
        if (playerRoot == null)
        {
            return;
        }

        float deltaX = collision.bounds.center.x - playerRoot.position.x;
        int side = deltaX < 0f ? -1 : 1;
        SetWallState(true, side);
    }

    void SetWallState(bool touching, int side)
    {
        if (isTouchingWall == touching && wallSide == side)
        {
            return;
        }

        isTouchingWall = touching;
        wallSide = side;
        WallContactChanged?.Invoke(isTouchingWall, wallSide);
    }
}
