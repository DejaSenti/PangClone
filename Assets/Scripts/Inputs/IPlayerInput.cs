public interface IPlayerInput
{
    MovementDirection GetMovementDirection();
    bool IsLeftKeyHeld();
    bool IsRightKeyHeld();
    bool IsShootKeyDown();
}
