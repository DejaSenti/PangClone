using UnityEngine;

public class KeyboardPlayerInput : MonoBehaviour, IPlayerInput
{
    public InputConfig InputConfig;
    public MovementDirection MovementDirection;

    public MovementDirection GetMovementDirection()
    {
        MovementDirection result = MovementDirection.None;

        if (IsLeftKeyHeld() && !IsRightKeyHeld())
        {
            result = MovementDirection.Left;
        }
        else if (!IsLeftKeyHeld() && IsRightKeyHeld())
        {
            result = MovementDirection.Right;
        }

        return result;
    }

    public bool IsLeftKeyHeld()
    {
        return Input.GetKey(InputConfig.LeftKey);
    }

    public bool IsRightKeyHeld()
    {
        return Input.GetKey(InputConfig.RightKey);
    }

    public bool IsShootKeyDown()
    {
        return Input.GetKeyDown(InputConfig.ShootKey);
    }
}