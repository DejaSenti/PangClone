using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "ScriptableObjects/PlayerInput")]
public class InputConfig : ScriptableObject
{
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode ShootKey;
    public KeyCode PauseKey;
}