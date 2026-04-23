using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private IMoveable moveable;
    private IDashable dashable;
    private IWallJump wallJump;
    private PlayerMovement playerMovement;
    float jumpForce;

    void Awake()
    {
        // mengmabil component IMoveable dan IDashable dari GameObject yang sama
        moveable = GetComponent<IMoveable>();
        dashable = GetComponent<IDashable>();
        wallJump = GetComponent<IWallJump>();
        playerMovement = GetComponent<PlayerMovement>();

        jumpForce = playerMovement.getJumpForce(); // Contoh mengambil nilai jump force dari PlayerMovement
    }

    void Update()
    {
        HandleMovementInput();
        HandleDashInput();
        HandleJumpInput();
    }

    void HandleMovementInput()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        playerMovement.setMoveInputX(dirX);
        moveable.Move(new Vector2(dirX, 0));
        checkFacing(dirX);
    }

    void checkFacing(float dirX)
    {
        if (dirX > 0)
        {
            moveable.SetFacing(1);
        }
        else if (dirX < 0)
        {
            moveable.SetFacing(-1);
        }
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashable.CanDash())
        {
            float dirX = Input.GetAxisRaw("Horizontal");

            if (Mathf.Approximately(dirX, 0f))
            {
                dirX = transform.localScale.x >= 0f ? 1f : -1f;
            }

            dashable.Dash(new Vector2(dirX, 0));
            // Debug.Log("Dash! direction: " + dirX);
        }
    }

    void HandleJumpInput()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;

        if (wallJump != null && wallJump.CanWallJump())
        {
            wallJump.WallJump(Vector2.up, jumpForce);
            return;
        }

        moveable.Jump(jumpForce); // Contoh nilai jump force
    }
}
