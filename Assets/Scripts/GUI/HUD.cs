using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    private static string PLAYER_LIVES_DISPLAY = "{0} Lives: {1}";
    private static string PLAYER_SCORE_DISPLAY = "{0} Score: {1}";
    private static string TIMER_DISPLAY = "Time Left: {0}";

    [SerializeField]
    private TMP_Text[] lives;
    [SerializeField]
    private TMP_Text[] score;
    [SerializeField]
    private TMP_Text timer;

    public void UpdateLives(PlayerID playerID, int newValue)
    {
        int index = (int)playerID - 1;
        lives[index].text = string.Format(PLAYER_LIVES_DISPLAY, playerID, newValue);
    }

    public void UpdateScore(PlayerID playerID, int newValue)
    {
        int index = (int)playerID - 1;
        score[index].text = string.Format(PLAYER_SCORE_DISPLAY, playerID, newValue);
    }

    public void UpdateTimer(int newValue)
    {
        timer.text = string.Format(TIMER_DISPLAY, newValue);
    }
}
