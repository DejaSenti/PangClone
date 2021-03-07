using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Timer))]
public class GameController : MonoBehaviour
{
    public static int Level;

    public bool IsGameRunning;

    private LevelManager levelManager;

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
        levelManager = new LevelManager();
        playerManager.Initialize(numPlayers);
        scoreManager.Initialize(numPlayers);
        overlayController.Initialize(playerManager.PlayerInputs);
    }

    public void StartNewGame()
    {
        IsGameRunning = true;

        Level = 1;
        LoadNewLevel();

        AddListeners();

        AnnounceAndWaitForStartLevel();
    }

    private void LoadNewLevel()
    {
        bool isLevelLoaded = levelManager.LoadLevelObject(Level);

        if (!isLevelLoaded)
        {
            IsGameRunning = false;
            AnnounceAndWaitForGameOver();
            return;
        }

        levelManager.LoadCurrentLevel();

        InitializeLevel();
    }

    private void InitializeLevel()
    {
        ballManager.InitializeLevel(levelManager.CurrentLevel);
        playerManager.InitializeLevel(levelManager.CurrentLevel);
    }

    private void AnnounceAndWaitForStartLevel()
    {
        overlayController.AnnounceGameStart();
        overlayController.AnnouncementOverEvent.AddListener(OnLevelStartAnnouncementOver);
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

        RestartCurrentLevel();

        overlayController.AnnounceTimeUp();
        overlayController.AnnouncementOverEvent.AddListener(OnRoundOverAnnouncementOver);
    }

    private void OnPlayerDeath()
    {
        RestartCurrentLevel();

        overlayController.AnnouncePlayerDeath();
        overlayController.AnnouncementOverEvent.AddListener(OnRoundOverAnnouncementOver);
    }

    private void RestartCurrentLevel()
    {
        EndCurrentLevel();

        levelManager.LoadCurrentLevel();
        InitializeLevel();
    }

    private void OnAllBallsDestroyed()
    {
        EndCurrentLevel();

        Level++;

        overlayController.AnnounceLevelClear();
        overlayController.AnnouncementOverEvent.AddListener(OnRoundOverAnnouncementOver);

        if (Level > levelManager.LevelCount)
        {
            IsGameRunning = false;
            return;
        }

        LoadNewLevel();
    }

    private void EndCurrentLevel()
    {
        timer.ResetTimer();

        ballManager.EndLevel();

        IsGameRunning = playerManager.EndLevel();

        levelManager.UnloadCurrentLevel();
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

    private void AnnounceAndWaitForGameOver()
    {
        overlayController.AnnounceGameOver();
        overlayController.AnnouncementOverEvent.AddListener(OnGameOverAnnouncementOver);
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
        levelManager.Terminate();

        SceneManager.LoadScene(GameScenes.MAIN_MENU);
    }
}