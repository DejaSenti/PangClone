using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public UnityEvent PlayerDeathEvent;
    public UnityEvent LivesOverEvent;

    [SerializeField]
    private GameObject playerPrefab;

    // debug only!!!
    [SerializeField]
    private GameObject level;
    [SerializeField]
    private Timer timer;

    private PlayerSpawnPoint[] playerSpawnPoints;
    private Dictionary<PlayerID, PlayerController> playerControllersByID;
    private Dictionary<PlayerID, int> playerLivesByID;

    private Timer gameTimer;

    private void Awake()
    {
        if(PlayerDeathEvent == null)
        {
            PlayerDeathEvent = new UnityEvent();
        }

        if(LivesOverEvent == null)
        {
            LivesOverEvent = new UnityEvent();
        }
    }

    public void Initialize(Timer gameTimer, int numPlayers)
    {
        playerControllersByID = new Dictionary<PlayerID, PlayerController>();
        playerLivesByID = new Dictionary<PlayerID, int>();

        for (int i = 0; i < numPlayers; i++)
        {
            var ID = (PlayerID)(i + 1);

            var inputConfig = Resources.Load(MainAssetPaths.INPUTS + ID) as InputConfig;

            if (inputConfig == null)
            {
                Debug.LogError("No valid input config file for Player" + ID);
                continue;
            }

            var playerGO = Instantiate(playerPrefab);
            var player = playerGO.GetComponent<Player>();

            player.PlayerID = ID;

            var playerInput = new KeyboardPlayerInput(inputConfig);

            var playerController = new PlayerController(player, playerInput);

            playerControllersByID[ID] = playerController;
            playerLivesByID[ID] = PlayerData.MAX_PLAYER_LIVES;
        }

        this.gameTimer = gameTimer;
    }

    public void StartLevel(GameObject level)
    {
        playerSpawnPoints = level.GetComponentsInChildren<PlayerSpawnPoint>();

        foreach(PlayerSpawnPoint spawnPoint in playerSpawnPoints)
        {
            if (!playerControllersByID.ContainsKey(spawnPoint.PlayerID) || playerLivesByID[spawnPoint.PlayerID] == 0)
                continue;

            var controller = playerControllersByID[spawnPoint.PlayerID];
            controller.SetPlayerPosition(spawnPoint.transform.position);
            controller.ActivatePlayer();

            controller.Player.HitEvent.AddListener(OnPlayerHit);
        }

        gameTimer.TimerElapsedEvent.AddListener(OnTimerElapsed);
    }

    private void Update()
    {
        foreach (PlayerController controller in playerControllersByID.Values)
        {
            controller.Update();
        }
    }

    private void OnTimerElapsed()
    {
        var keys = new List<PlayerID>(playerLivesByID.Keys);
        foreach(var key in keys)
        {
            playerLivesByID[key]--;
        }

        EndLevel();
    }

    private void OnPlayerHit(PlayerID ID)
    {
        playerLivesByID[ID]--;

        EndLevel();
    }

    private void EndLevel()
    {
        gameTimer.TimerElapsedEvent.RemoveListener(OnTimerElapsed);

        bool gameOver = true;

        foreach (var key in playerControllersByID.Keys)
        {
            playerControllersByID[key].Player.HitEvent.RemoveListener(OnPlayerHit);
            playerControllersByID[key].DeactivatePlayer();

            if (playerLivesByID[key] > 0)
                gameOver = false;
        }

        if (gameOver)
        {
            LivesOverEvent.Invoke();
        }
        else
        {
            PlayerDeathEvent.Invoke();
        }
    }

    [ContextMenu("Test Level")]
    public void TestLevel()
    {
        Initialize(timer, 4);
        timer.StartTimer(100);
        StartLevel(level);
    }
}