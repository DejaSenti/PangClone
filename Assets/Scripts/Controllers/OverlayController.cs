using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Timer))]
public class OverlayController : MonoBehaviour
{
    public UnityEvent AnnouncementOverEvent;

    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private PauseOverlay pauseOverlay;
    [SerializeField]
    private AnnouncementOverlay announcementOverlay;
    [SerializeField]
    private Timer timer;

    private IPlayerInput[] playerInputs;

    private float savedTimeScale;

    private bool isDuringAnnouncement;

    private void Awake()
    {
        if (AnnouncementOverEvent == null)
        {
            AnnouncementOverEvent = new UnityEvent();
        }
    }

    public void Initialize(IPlayerInput[] playerInputs)
    {
        this.playerInputs = playerInputs;
    }

    private void Update()
    {
        foreach (IPlayerInput input in playerInputs)
        {
            if (input.GetEnabled() && input.IsPauseKeyDown())
            {
                OnPauseButton();
                return;
            }
        }
    }

    public void AnnounceGameStart()
    {
        var text = string.Format(GameAnnouncements.LEVEL_START, GameController.Level);
        Announce(text);
    }

    public void AnnounceLevelClear()
    {
        Announce(GameAnnouncements.LEVEL_CLEAR);
    }

    public void AnnouncePlayerDeath()
    {
        Announce(GameAnnouncements.PLAYER_DEATH);
    }

    public void AnnounceTimeUp()
    {
        Announce(GameAnnouncements.TIMER_ELAPSED);
    }

    public void AnnounceGameOver()
    {
        Announce(GameAnnouncements.GAME_OVER);
    }

    private void Announce(string text)
    {
        isDuringAnnouncement = true;

        announcementOverlay.gameObject.SetActive(true);
        announcementOverlay.Announcement.text = text;

        timer.StartTimer(GameData.ANNOUNCEMENT_TIME);
        timer.TimerElapsedEvent.AddListener(OnAnnouncementOver);
    }

    private void OnAnnouncementOver()
    {
        isDuringAnnouncement = false;

        announcementOverlay.Announcement.text = string.Empty;
        announcementOverlay.gameObject.SetActive(false);

        timer.TimerElapsedEvent.RemoveListener(OnAnnouncementOver);

        AnnouncementOverEvent.Invoke();
    }

    private void OnPauseButton()
    {
        if (!isDuringAnnouncement)
        {
            if (gameController.IsGameRunning)
            {
                savedTimeScale = Time.timeScale;
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    private void PauseGame()
    {
        TogglePause();
        ShowPause();

        foreach (IPlayerInput playerInput in playerInputs)
        {
            playerInput.SetEnabled(false);
        }
    }

    private void UnpauseGame()
    {
        TogglePause();
        HidePause();

        foreach (IPlayerInput playerInput in playerInputs)
        {
            playerInput.SetEnabled(true);
        }
    }

    private void ShowPause()
    {
        pauseOverlay.gameObject.SetActive(true);
        pauseOverlay.Resume.onClick.AddListener(OnResumeClick);
        pauseOverlay.Exit.onClick.AddListener(OnExitClick);
    }

    private void HidePause()
    {
        pauseOverlay.gameObject.SetActive(false);
        pauseOverlay.Resume.onClick.RemoveListener(OnResumeClick);
        pauseOverlay.Exit.onClick.RemoveListener(OnExitClick);
    }

    private void TogglePause()
    {
        gameController.IsGameRunning = !gameController.IsGameRunning;
        Time.timeScale = Time.timeScale == 0 ? savedTimeScale : 0;
    }

    private void OnResumeClick()
    {
        UnpauseGame();
    }

    private void OnExitClick()
    {
        UnpauseGame();
        gameController.Terminate();
    }

    public void Terminate()
    {
        AnnouncementOverEvent.RemoveAllListeners();
        Destroy(pauseOverlay.gameObject);
        Destroy(announcementOverlay.gameObject);
        Destroy(gameObject);
    }
}