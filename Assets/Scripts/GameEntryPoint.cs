using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    private void Start()
    {
        gameController.Initialize(UIMainMenu.NumPlayers);
        gameController.StartNewGame();
    }
}
