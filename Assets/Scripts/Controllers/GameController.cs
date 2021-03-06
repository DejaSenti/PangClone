using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Timer))]
public class GameController : MonoBehaviour
{
    public static int Level;
    public bool IsGameRunning;

    private GameObject currentLevel;

    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    private BallManager ballManager;
    [SerializeField]
    private ScoreManager scoreManager;

    [SerializeField]
    private OverlayController overlayController;

    [SerializeField]
    private HUD view;

    [SerializeField]
    private Timer timer;

    public void Initialize(int numPlayers)
    {
        playerManager.Initialize(numPlayers);
        scoreManager.Initialize(numPlayers);
    }

    public void StartNewGame()
    {
        IsGameRunning = true;

        Level = 1;
        InitializeLevel();

        AddListeners();

        AnnounceAndWaitForStartLevel();
    }

    private void InitializeLevel()
    {
        string levelPath = MainAssetPaths.LEVELS + Level;

        var levelGO = Resources.Load(levelPath);

        if (levelGO == null)
        {
            IsGameRunning = false;
            AnnounceAndWaitForGameOver();
            return;
        }

        currentLevel = (GameObject)Instantiate(levelGO);
        currentLevel.transform.position = Vector3.zero;

        ballManager.InitializeLevel(currentLevel);
        playerManager.InitializeLevel(currentLevel);
    }

    private void AnnounceAndWaitForStartLevel()
    {
        overlayController.AnnounceGameStart();
        overlayController.AnnouncementOverEvent.AddListener(OnLevelStartAnnouncementOver);
    }

    private void AnnounceAndWaitForGameOver()
    {
        overlayController.AnnounceGameOver();
        overlayController.AnnouncementOverEvent.AddListener(OnGameOverAnnouncementOver);
    }

    private void OnLevelStartAnnouncementOver()
    {
        overlayController.AnnouncementOverEvent.RemoveListener(OnLevelStartAnnouncementOver);

        StartLevel();
    }

    private void StartLevel()
    {
        ballManager.StartLevel();
        playerManager.StartLevel();

        timer.StartTimer(GameData.MAX_GAME_TIME);
    }

    private void OnTimerElapsed()
    {
        playerManager.DecrementLivesFromAll();

        EndCurrentLevel();

        overlayController.AnnounceTimeUp();
        overlayController.AnnouncementOverEvent.AddListener(OnRoundOverAnnouncementOver);
    }

    private void OnPlayerDeath()
    {
        EndCurrentLevel();

        overlayController.AnnouncePlayerDeath();
        overlayController.AnnouncementOverEvent.AddListener(OnRoundOverAnnouncementOver);
    }

    private void OnAllBallsDestroyed()
    {
        EndCurrentLevel();

        Level++;

        overlayController.AnnounceLevelClear();
        overlayController.AnnouncementOverEvent.AddListener(OnRoundOverAnnouncementOver);

        if (Level > GameEntryPoint.LevelCount)
        {
            IsGameRunning = false;
            return;
        }

        InitializeLevel();
    }

    private void EndCurrentLevel()
    {
        timer.ResetTimer();

        ballManager.EndLevel();

        IsGameRunning = playerManager.EndLevel();
    }

    private void OnRoundOverAnnouncementOver()
    {
        overlayController.AnnouncementOverEvent.RemoveListener(OnRoundOverAnnouncementOver);

        if (IsGameRunning)
        {
            AnnounceAndWaitForStartLevel();
        }
        else
        {
            AnnounceAndWaitForGameOver();
        }
    }

    private void OnGameOverAnnouncementOver()
    {
        overlayController.AnnouncementOverEvent.RemoveListener(OnGameOverAnnouncementOver);
        Terminate();
    }

    private void AddListeners()
    {
        timer.TimerElapsedEvent.AddListener(OnTimerElapsed);
        playerManager.PlayerDeathEvent.AddListener(OnPlayerDeath);
        ballManager.AllBallsDestroyedEvent.AddListener(OnAllBallsDestroyed);
    }

    private void RemoveListeners()
    {
        timer.TimerElapsedEvent.RemoveListener(OnTimerElapsed);
        playerManager.PlayerDeathEvent.RemoveListener(OnPlayerDeath);
        ballManager.AllBallsDestroyedEvent.RemoveListener(OnAllBallsDestroyed);
    }

    private void Update()
    {
        if (timer.isActiveAndEnabled)
            view.UpdateTimer(timer.RoundTimeLeft);
    }

    public void Terminate()
    {
        RemoveListeners();
        playerManager.Terminate();
        ballManager.Terminate();
        scoreManager.Terminate();
        overlayController.Terminate();

        SceneManager.LoadScene(GameScenes.MAIN_MENU);
    }

    [ContextMenu("Test Game")]
    public void TestGame()
    {
        Initialize(2);
        StartNewGame();
    }
}
