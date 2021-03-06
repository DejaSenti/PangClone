public interface IPlayerInput
{
    void SetEnabled(bool state);
    bool GetEnabled();

    MovementDirection GetMovementDirection();

    bool IsShootKeyDown();
    bool IsPauseKeyDown();
}
