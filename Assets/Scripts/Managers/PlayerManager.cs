using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public UnityEvent PlayerDeathEvent;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private HUD view;

    private PlayerSpawnPoint[] playerSpawnPoints;
    private Dictionary<PlayerID, PlayerController> playerControllersByID;
    private Dictionary<PlayerID, int> playerLivesByID;

    private void Awake()
    {
        if (PlayerDeathEvent == null)
        {
            PlayerDeathEvent = new UnityEvent();
        }
    }

    public void Initialize(int numPlayers)
    {
        playerControllersByID = new Dictionary<PlayerID, PlayerController>();
        playerLivesByID = new Dictionary<PlayerID, int>();

        for (int i = 0; i < numPlayers; i++)
        {
            var playerID = (PlayerID)(i + 1);

            var inputConfig = Resources.Load(MainAssetPaths.INPUTS + playerID) as InputConfig;

            if (inputConfig == null)
            {
                Debug.LogError("No valid input config file for Player" + playerID);
                continue;
            }

            var playerGO = Instantiate(playerPrefab);
            var player = playerGO.GetComponent<Player>();

            player.ID = playerID;

            var playerInput = new KeyboardPlayerInput(inputConfig);

            var playerController = new PlayerController(player, playerInput);

            playerController.DeactivatePlayer();

            playerControllersByID[playerID] = playerController;
            playerLivesByID[playerID] = PlayerData.MAX_PLAYER_LIVES;

            view.UpdateLives(playerID, playerLivesByID[playerID]);
        }
    }

    public void InitializeLevel(GameObject level)
    {
        playerSpawnPoints = level.GetComponentsInChildren<PlayerSpawnPoint>();
    }

    public void StartLevel()
    {
        foreach (PlayerSpawnPoint spawnPoint in playerSpawnPoints)
        {
            if (!playerControllersByID.ContainsKey(spawnPoint.PlayerID) || playerLivesByID[spawnPoint.PlayerID] == 0)
                continue;

            var controller = playerControllersByID[spawnPoint.PlayerID];
            controller.SetPlayerPosition(spawnPoint.transform.position);
            controller.ActivatePlayer();

            controller.Player.HitEvent.AddListener(OnPlayerHit);
        }
    }

    private void Update()
    {
        foreach (PlayerController controller in playerControllersByID.Values)
        {
            controller.Update();
        }
    }

    private void OnPlayerHit(PlayerID playerID)
    {
        DecrementPlayerLives(playerID);

        PlayerDeathEvent.Invoke();
    }

    private void DecrementPlayerLives(PlayerID playerID)
    {
        if (playerLivesByID[playerID] > 0)
            playerLivesByID[playerID]--;

        view.UpdateLives(playerID, playerLivesByID[playerID]);
    }

    public void DecrementLivesFromAll()
    {
        var keys = new List<PlayerID>(playerLivesByID.Keys);
        foreach (var key in keys)
        {
            DecrementPlayerLives(key);
        }
    }

    public bool EndLevel()
    {
        bool isGameOver = true;

        foreach (var key in playerControllersByID.Keys)
        {
            playerControllersByID[key].Player.HitEvent.RemoveListener(OnPlayerHit);
            playerControllersByID[key].DeactivatePlayer();

            if (playerLivesByID[key] > 0)
                isGameOver = false;
        }

        return !isGameOver;
    }

    public void Terminate()
    {
        foreach (var value in playerControllersByID.Values)
        {
            value.Terminate();
        }

        PlayerDeathEvent.RemoveAllListeners();

        Destroy(gameObject);
    }
}