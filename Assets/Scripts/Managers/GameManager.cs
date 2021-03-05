using UnityEngine;

[RequireComponent(typeof(Timer))]
public class GameManager : MonoBehaviour
{
    public static int Level;

    private GameObject currentLevel;

    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private BallManager ballManager;
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private View view;
    [SerializeField]
    private Timer timer;

    public void Initialize(int numPlayers)
    {
        playerManager.Initialize(timer, numPlayers);
        scoreManager.Initialize(numPlayers);
    }

    public void StartNewGame()
    {
        Level = 1;

        InitializeLevel(Level);

        StartLevel();
    }

    private void Update()
    {
        view.UpdateTimer(timer.RoundTimeLeft);
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

        Level++;
        EndCurrentLevel();
        InitializeLevel(Level);
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
        Initialize(2);
        StartNewGame();
    }
}