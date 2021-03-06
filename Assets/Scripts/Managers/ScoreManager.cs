using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private HUD view;

    private Dictionary<PlayerID, int> playerScoresByID;

    public void Initialize(int numPlayers)
    {
        playerScoresByID = new Dictionary<PlayerID, int>();

        for (int i = 0; i < numPlayers; i++)
        {
            var playerID = (PlayerID)(i + 1);
            playerScoresByID[playerID] = 0;

            UpdateScore(playerID, playerScoresByID[playerID]);
        }

        BallManager.ScoreEvent.AddListener(OnScoreEvent);
    }

    private void OnScoreEvent(IScorable scoreable, PlayerID playerID)
    {
        int addedScore = 0;

        if (scoreable is Ball)
        {
            addedScore = GameData.BALL_SCORE * GameController.Level;
        }

        UpdateScore(playerID, addedScore);
    }

    private void UpdateScore(PlayerID playerID, int score)
    {
        playerScoresByID[playerID] += score;
        view.UpdateScore(playerID, playerScoresByID[playerID]);
    }

    public void Terminate()
    {
        BallManager.ScoreEvent.RemoveListener(OnScoreEvent);
        Destroy(gameObject);
    }
}