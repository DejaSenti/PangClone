using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Timer))]
public class OverlayController : MonoBehaviour
{
    public UnityEvent AnnouncementOverEvent;

    [SerializeField]
    private PauseOverlay pauseOverlay;
    [SerializeField]
    private AnnouncementOverlay announcementOverlay;
    [SerializeField]
    private Timer timer;

    private float savedTimeScale;

    private KeyCode pauseButton = KeyCode.Escape;
    private bool isPauseDown { get => Input.GetKeyDown(pauseButton); }

    private void Awake()
    {
        if (AnnouncementOverEvent == null)
        {
            AnnouncementOverEvent = new UnityEvent();
        }
    }

    private void Update()
    {
        if (isPauseDown)
        {
            OnPauseButton();
        }
    }

    public void AnnounceGameStart()
    {
        var text = string.Format(GameAnnouncements.LEVEL_START, GameManager.Level);
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
        announcementOverlay.gameObject.SetActive(true);
        announcementOverlay.Announcement.text = text;

        timer.StartTimer(GameData.ANNOUNCEMENT_TIME);
        timer.TimerElapsedEvent.AddListener(OnAnnouncementOver);
    }

    private void OnAnnouncementOver()
    {
        announcementOverlay.Announcement.text = string.Empty;
        announcementOverlay.gameObject.SetActive(false);

        timer.TimerElapsedEvent.RemoveListener(OnAnnouncementOver);

        AnnouncementOverEvent.Invoke();
    }

    private void OnPauseButton()
    {
        if (GameManager.IsGameRunning)
        {
            savedTimeScale = Time.timeScale;
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    private void PauseGame()
    {
        TogglePause();
        ShowPause();
    }

    private void UnpauseGame()
    {
        TogglePause();
        HidePause();
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
        GameManager.IsGameRunning = !GameManager.IsGameRunning;
        Time.timeScale = Time.timeScale == 0 ? savedTimeScale : 0;
    }

    private void OnExitClick()
    {
        SceneManager.LoadScene(GameScenes.MAIN_MENU);
    }

    private void OnResumeClick()
    {
        UnpauseGame();
    }
}