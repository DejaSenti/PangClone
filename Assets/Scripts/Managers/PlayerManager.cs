using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public UnityEvent PlayerDeathEvent;

    public IPlayerInput[] PlayerInputs;

    [SerializeField]
    private UIPlayerInput UIPlayerInput;
    [SerializeField]
    private Canvas canvas;

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
        PlayerInputs = new IPlayerInput[numPlayers];
        playerControllersByID = new Dictionary<PlayerID, PlayerController>();
        playerLivesByID = new Dictionary<PlayerID, int>();

        for (int i = 0; i < numPlayers; i++)
        {
            var playerID = (PlayerID)(i + 1);

#if UNITY_STANDALONE_WIN
            var playerInput = GetPlayerInput(playerID);
            if (playerInput == null)
                continue;
#endif

#if UNITY_ANDROID
            var playerInput = GetPlayerInput();
#endif

            playerInput.SetEnabled(true);
            PlayerInputs[i] = playerInput;

            var playerGO = Instantiate(playerPrefab);
            var player = playerGO.GetComponent<Player>();

            player.Initialize(playerID, PlayerData.PLAYER_COLORS[i]);

            var playerController = new PlayerController(player, playerInput);

            playerController.DeactivatePlayer();

            playerControllersByID[playerID] = playerController;
            playerLivesByID[playerID] = PlayerData.MAX_PLAYER_LIVES;

            view.UpdateLives(playerID, playerLivesByID[playerID]);
        }
    }

#if UNITY_STANDALONE_WIN
    private IPlayerInput GetPlayerInput(PlayerID playerID)
    {
        var inputConfig = Resources.Load(MainAssetPaths.INPUTS + playerID) as InputConfig;

        if (inputConfig == null)
        {
            Debug.LogError("No valid input config file for Player" + playerID);
            return null;
        }

        var result = new KeyboardPlayerInput(inputConfig);
        return result;
    }
#endif

#if UNITY_ANDROID
    private IPlayerInput GetPlayerInput()
    {
        var result = Instantiate(UIPlayerInput, canvas.transform);
        result.transform.SetAsFirstSibling();
        return result;
    }
#endif

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