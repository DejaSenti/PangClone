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
        play.onClick.AddListener(OnPlayClick);
        quit.onClick.AddListener(OnQuitClick);

#if UNITY_STANDALONE_WIN
        NumPlayers = playerSelect.value + 1;

        playerSelect.onValueChanged.AddListener(OnPlayerSelectClick);
#endif

#if UNITY_ANDROID
        NumPlayers = 1;
#endif
    }

    private void OnPlayClick()
    {
        SceneManager.LoadScene(GameScenes.GAME);
    }

    private void OnQuitClick()
    {
        Application.Quit();
    }

#if UNITY_STANDALONE_WIN
    private void OnPlayerSelectClick(int numPlayers)
    {
        NumPlayers = numPlayers + 1;
    }
#endif
}
