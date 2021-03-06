using UnityEngine;

public class LevelManager
{
    public GameObject CurrentLevel { get; private set; }
    public Object CurrentLevelGO { get; private set; }

    public int LevelCount;

    private AssetBundle levelBundle;

    public LevelManager()
    {
        levelBundle = AssetBundle.LoadFromFile(MainAssetPaths.LEVEL_ASSET_BUNDLE);
        LevelCount = levelBundle.GetAllAssetNames().Length;
    }

    public bool LoadLevelObject(int level)
    {
        string levelPath = MainAssetPaths.LEVEL + level;

        CurrentLevelGO = levelBundle.LoadAsset<GameObject>(levelPath);

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

    public void Terminate()
    {
        levelBundle.Unload(true);
    }
}