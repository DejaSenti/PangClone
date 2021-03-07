using UnityEngine;

public class PlayerController
{
    public Player Player { get; private set; }

    private IPlayerInput playerInput;

    public PlayerController(Player player, IPlayerInput playerInput)
    {
        Player = player;
        this.playerInput = playerInput;
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
        Player.Deactivate();
    }

    public void Update()
    {
        if (!Player.isActiveAndEnabled)
            return;

        var inputMovementDirection = playerInput.GetMovementDirection();

        Player.Move(inputMovementDirection);

        if (playerInput.IsShootKeyDown())
        {
            Player.Shoot();
        }
    }

    public void Terminate()
    {
        Player.Terminate();
        Player.HitEvent.RemoveAllListeners();
    }
}
