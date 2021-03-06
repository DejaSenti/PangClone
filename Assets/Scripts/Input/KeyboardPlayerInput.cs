using UnityEngine;

public class KeyboardPlayerInput : IPlayerInput
{
    private InputConfig config;

    private bool isEnabled;

    public KeyboardPlayerInput(InputConfig config)
    {
        this.config = config;
    }

    public void SetEnabled(bool state)
    {
        isEnabled = state;
    }

    public bool GetEnabled()
    {
        return isEnabled;
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
        return Input.GetKey(config.LeftKey) && isEnabled;
    }

    public bool IsRightKeyHeld()
    {
        return Input.GetKey(config.RightKey) && isEnabled;
    }

    public bool IsShootKeyDown()
    {
        return Input.GetKeyDown(config.ShootKey) && isEnabled;
    }

    public bool IsPauseKeyDown()
    {
        return Input.GetKeyDown(config.PauseKey) && isEnabled;
    }
}