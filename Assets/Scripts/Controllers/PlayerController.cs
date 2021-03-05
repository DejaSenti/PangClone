using UnityEngine;

public class PlayerController
{
    public Player Player { get; private set; }

    private IPlayerInput playerInput;

    public PlayerController(Player player, IPlayerInput playerInput)
    {
        Player = player;
        this.playerInput = playerInput;

        Player.Gun.Initialize();
    }

    public void SetPlayerPosition(Vector3 position)
    {
        Player.transform.position = position;
    }

    public void ActivatePlayer()
    {
        Player.gameObject.SetActive(true);
    }

    public void DeactivatePlayer()
    {
        Player.gameObject.SetActive(false);
        Player.Gun.Reset();
    }

    public void Update()
    {
        var inputMovementDirection = playerInput.GetMovementDirection();

        Player.Move(inputMovementDirection);

        if (playerInput.IsShootKeyDown())
        {
            Player.Gun.Shoot(Player.ID);
        }
    }
}
