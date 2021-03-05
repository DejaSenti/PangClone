using UnityEngine;

public class UIPlayerInput : MonoBehaviour, IPlayerInput
{
    private bool isLeftKeyHeld;
    private bool isRightKeyHeld;
    private bool isShootKeyDown;

    public void SetLeftKey(bool state)
    {
        isLeftKeyHeld = state;
    }

    public void SetRightKey(bool state)
    {
        isRightKeyHeld = state;
    }

    public void SetShootKey()
    {
        isShootKeyDown = true;
    }

    public bool IsLeftKeyHeld()
    {
        return isLeftKeyHeld;
    }

    public bool IsRightKeyHeld()
    {
        return isRightKeyHeld;
    }

    public bool IsShootKeyDown()
    {
        if (isShootKeyDown)
        {
            isShootKeyDown = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public MovementDirection GetMovementDirection()
    {
        throw new System.NotImplementedException();
    }
}