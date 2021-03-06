using UnityEngine;

public class UIPlayerInput : MonoBehaviour, IPlayerInput
{
    private bool isEnabled;

    private bool isLeftKeyHeld;
    private bool isRightKeyHeld;
    private bool isShootKeyDown;
    private bool isPauseKeyDown;

    public void SetEnabled(bool state)
    {
        isEnabled = state;
    }

    public bool GetEnabled()
    {
        return isEnabled;
    }

    public void SetLeftKey(bool state)
    {
        isLeftKeyHeld = state && isEnabled;
    }

    public void SetRightKey(bool state)
    {
        isRightKeyHeld = state && isEnabled;
    }

    public void SetShootKey()
    {
        isShootKeyDown = true && isEnabled;
    }

    public void SetPauseKey()
    {
        isPauseKeyDown = true && isEnabled;
    }

    private void ResetShootKey()
    {
        isShootKeyDown = false;
    }

    private void ResetPauseKey()
    {
        isPauseKeyDown = false;
    }

    public MovementDirection GetMovementDirection()
    {
        MovementDirection result = MovementDirection.None;

        if (isLeftKeyHeld && !isRightKeyHeld)
        {
            result = MovementDirection.Left;
        }
        else if (!isLeftKeyHeld && isRightKeyHeld)
        {
            result = MovementDirection.Right;
        }

        return result;
    }

    public bool IsShootKeyDown()
    {
        return isShootKeyDown;
    }

    public bool IsPauseKeyDown()
    {
        return isPauseKeyDown;
    }

    private void Update()
    {
        if (isShootKeyDown)
        {
            Invoke("ResetShootKey", 0);
        }

        if (isPauseKeyDown)
        {
            Invoke("ResetPauseKey", 0);
        }
    }
}