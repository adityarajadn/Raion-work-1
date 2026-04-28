using UnityEngine;
using System;

public class PlayerWallCheck : MonoBehaviour
{
    public event Action<bool, string> OnWallContact;
    bool isTouchingWall = false;
    public string wallSide;
    public PlayerMovement player;
    

    void OnEnable()
    {
        PlayerMovement.PlayerFacingRight += UpdateFacingDirection;
    }

    void OnDisable()
    {
        PlayerMovement.PlayerFacingRight -= UpdateFacingDirection;
    }
    
    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    void UpdateFacingDirection(bool facingRight)
    {
        findWallSide();
    }

    void findWallSide()
    {
        if (player.facingRight && isTouchingWall)
        {
            wallSide = "Right";
        }
        else if (!player.facingRight && isTouchingWall)
        {
            wallSide = "Left";
        }
        else
        {
            wallSide = "None";
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            isTouchingWall = true;
            findWallSide();
            OnWallContact?.Invoke(true, wallSide);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            isTouchingWall = false;
            OnWallContact?.Invoke(false, wallSide);
        }
    }
}
