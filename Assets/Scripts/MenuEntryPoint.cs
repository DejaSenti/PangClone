using UnityEngine;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField]
    private UIMainMenu UIMainMenu;

    private void Start()
    {
        UIMainMenu.Initialize();
    }
}