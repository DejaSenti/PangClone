using UnityEngine;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject menuCanvasWinGO;
    [SerializeField]
    private GameObject menuCanvasAndroidGO;

    private void Start()
    {
#if UNITY_STANDALONE_WIN
        var menuInstance = Instantiate(menuCanvasWinGO);
#endif

#if UNITY_ANDROID
        var menuInstance = Instantiate(menuCanvasAndroidGO);
#endif

        var UIMainMenu = menuInstance.GetComponentInChildren<UIMainMenu>();
        UIMainMenu.Initialize();
    }
}