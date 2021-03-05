using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Timer))]
public class GameManager : MonoBehaviour
{
    public static int Level;
    public static bool IsGameRunning;

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
        IsGameRunning = false;

        Level = 1;

        InitializeLevel(Level);

        AnnounceAndWaitForStartLevel();
    }

    private void InitializeLevel(int level)
    {
        string levelPath = MainAssetPaths.LEVELS + level;

        var levelGO = Resources.Load(levelPath);

        if (levelGO == null)
        {
            overlayController.AnnounceGameOver();
            overlayController.AnnouncementOverEvent.AddListener(OnGameOverAnnouncementOver);
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

        AddListeners();

        IsGameRunning = true;
    }

    private void OnTimerElapsed()
    {
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

    private void OnLivesOver()
    {
        EndCurrentLevel();

        overlayController.AnnounceGameOver();
        overlayController.AnnouncementOverEvent.AddListener(OnGameOverAnnouncementOver);
    }

    private void OnAllBallsDestroyed()
    {
        Level++;
        EndCurrentLevel();

        overlayController.AnnounceLevelClear();
        overlayController.AnnouncementOverEvent.AddListener(OnRoundOverAnnouncementOver);

        InitializeLevel(Level);
    }

    private void OnRoundOverAnnouncementOver()
    {
        overlayController.AnnouncementOverEvent.RemoveListener(OnRoundOverAnnouncementOver);
        AnnounceAndWaitForStartLevel();
    }

    private void OnGameOverAnnouncementOver()
    {
        overlayController.AnnouncementOverEvent.RemoveListener(OnGameOverAnnouncementOver);
        SceneManager.LoadScene(GameScenes.MAIN_MENU);
    }

    private void EndCurrentLevel()
    {
        IsGameRunning = false;

        RemoveListeners();

        playerManager.EndLevel();
        ballManager.EndLevel();
    }

    private void AddListeners()
    {
        timer.TimerElapsedEvent.AddListener(OnTimerElapsed);
        playerManager.LivesOverEvent.AddListener(OnLivesOver);
        playerManager.PlayerDeathEvent.AddListener(OnPlayerDeath);
        ballManager.AllBallsDestroyedEvent.AddListener(OnAllBallsDestroyed);
    }

    private void RemoveListeners()
    {
        timer.TimerElapsedEvent.RemoveListener(OnTimerElapsed);
        playerManager.LivesOverEvent.RemoveListener(OnLivesOver);
        playerManager.PlayerDeathEvent.RemoveListener(OnPlayerDeath);
        ballManager.AllBallsDestroyedEvent.RemoveListener(OnAllBallsDestroyed);
    }

    private void Update()
    {
        view.UpdateTimer(timer.RoundTimeLeft);
    }

    [ContextMenu("Test Game")]
    public void TestGame()
    {
        Initialize(2);
        StartNewGame();
    }
}
