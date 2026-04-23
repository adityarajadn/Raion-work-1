using UnityEngine;

public interface IMoveable
{
    void Move(Vector2 direction);
    void SetFacing(int direction);
    void Jump(float jumpForce);
}