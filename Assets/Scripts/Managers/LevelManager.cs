using UnityEngine;

public class LevelManager
{
    public GameObject CurrentLevel { get; private set; }
    public Object CurrentLevelGO { get; private set; }

    public bool LoadLevelObject(int level)
    {
        string levelPath = MainAssetPaths.LEVELS + level;

        CurrentLevelGO = Resources.Load(levelPath);

        if (CurrentLevelGO == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void LoadCurrentLevel()
    {
        CurrentLevel = Object.Instantiate(CurrentLevelGO) as GameObject;
        CurrentLevel.transform.position = Vector3.zero;
    }

    public void UnloadCurrentLevel()
    {
        Object.Destroy(CurrentLevel);
    }
}