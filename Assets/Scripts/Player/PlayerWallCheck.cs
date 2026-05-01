using UnityEngine;
using System;

public class PlayerWallCheck : MonoBehaviour
{
    public static event Action<bool, string> OnWallContact;
    bool isTouchingWall = false;
    string wallSide;
    bool facingRight = true;
    

    void OnEnable()
    {
        PlayerMovement.PlayerFacingRight += UpdateFacingDirection;
    }

    void OnDisable()
    {
        PlayerMovement.PlayerFacingRight -= UpdateFacingDirection;
    }
    
    void UpdateFacingDirection(bool facingRight)
    {
        this.facingRight = facingRight;
        findWallSide();
    }

    void findWallSide()
    {
        if (facingRight && isTouchingWall)
        {
            wallSide = "Right";
        }
        else if (!facingRight && isTouchingWall)
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
