using UnityEngine;

[RequireComponent(typeof(Timer))]
public class GameManager : MonoBehaviour
{
    private int level;
    private GameObject currentLevel;

    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private BallManager ballManager;
    [SerializeField]
    private Timer timer;

    public void Initialize(int numPlayers)
    {
        playerManager.Initialize(timer, numPlayers);
    }

    public void StartNewGame()
    {
        level = 1;

        InitializeLevel(level);

        StartLevel();
    }

    private void InitializeLevel(int level)
    {
        string levelPath = MainAssetPaths.LEVELS + level;

        var levelGO = Resources.Load(levelPath);

        if (levelGO == null)
        {
            Debug.Log("Couldn't load level.");
            return;
        }

        currentLevel = (GameObject)Instantiate(levelGO);
        currentLevel.transform.position = Vector3.zero;

        ballManager.InitializeLevel(currentLevel);
        playerManager.InitializeLevel(currentLevel);
    }

    private void StartLevel()
    {
        ballManager.StartLevel();
        playerManager.StartLevel();

        timer.StartTimer(GameData.MAX_GAME_TIME);

        AddListeners();
    }

    private void OnPlayerDeath()
    {
        RemoveListeners();

        StartLevel();
    }

    private void OnLivesOver()
    {
        RemoveListeners();
        Debug.Log("Game Over.");
    }

    private void OnAllBallsDestroyed()
    {
        RemoveListeners();

        level++;
        EndCurrentLevel();
        InitializeLevel(level);
        StartLevel();
    }

    private void EndCurrentLevel()
    {
        playerManager.EndLevel();
        ballManager.EndLevel();
        Destroy(currentLevel);
    }

    private void AddListeners()
    {
        playerManager.LivesOverEvent.AddListener(OnLivesOver);
        playerManager.PlayerDeathEvent.AddListener(OnPlayerDeath);
        ballManager.AllBallsDestroyedEvent.AddListener(OnAllBallsDestroyed);
    }

    private void RemoveListeners()
    {
        playerManager.LivesOverEvent.RemoveListener(OnLivesOver);
        playerManager.PlayerDeathEvent.RemoveListener(OnPlayerDeath);
        ballManager.AllBallsDestroyedEvent.RemoveListener(OnAllBallsDestroyed);
    }

    [ContextMenu("Test Game")]
    public void TestGame()
    {
        Initialize(1);
        StartNewGame();
    }
}