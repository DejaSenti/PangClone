using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    public static int NumPlayers;

    [SerializeField]
    private Button play;
    [SerializeField]
    private Button quit;
    [SerializeField]
    private TMP_Dropdown playerSelect;

    public void Initialize()
    {
        NumPlayers = playerSelect.value + 1;

        play.onClick.AddListener(OnPlayClick);
        quit.onClick.AddListener(OnQuitClick);
        playerSelect.onValueChanged.AddListener(OnPlayerSelectClick);
    }

    private void OnPlayClick()
    {
        SceneManager.LoadScene(GameScenes.GAME);
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }

    private void OnPlayerSelectClick(int numPlayers)
    {
        NumPlayers = numPlayers + 1;
    }
}
