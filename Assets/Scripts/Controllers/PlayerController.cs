using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player Player;
    public Gun Gun;
    public KeyboardPlayerInput PlayerInput;

    private void Awake()
    {
        Gun.Initialize();
    }

    private void Update()
    {
        var inputMovementDirection = PlayerInput.GetMovementDirection();

        Player.Move(inputMovementDirection);

        if (PlayerInput.IsShootKeyDown())
        {
            Gun.Shoot();
        }
    }
}
