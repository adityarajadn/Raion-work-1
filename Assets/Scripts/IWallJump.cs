using UnityEngine;

public interface IWallJump
{
    bool CanWallJump();
    void WallJump(Vector2 direction, float jumpForce);
}
