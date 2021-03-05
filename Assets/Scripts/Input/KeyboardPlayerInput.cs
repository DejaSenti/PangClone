using UnityEngine;

public class KeyboardPlayerInput : IPlayerInput
{
    private InputConfig config;

    public KeyboardPlayerInput(InputConfig config)
    {
        this.config = config;
    }

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
        return Input.GetKey(config.LeftKey);
    }

    public bool IsRightKeyHeld()
    {
        return Input.GetKey(config.RightKey);
    }

    public bool IsShootKeyDown()
    {
        return Input.GetKeyDown(config.ShootKey);
    }
}