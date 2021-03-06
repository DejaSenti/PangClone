using System;
using System.IO;
using UnityEngine;

public static class BuildHelpers
{
    public static int GetLevelCount()
    {
        string AssetsFolderPath = Application.dataPath;
        string levelFolder = AssetsFolderPath + "/Resources/Prefabs/Levels";

        DirectoryInfo dir = new DirectoryInfo(levelFolder);
        FileInfo[] info = dir.GetFiles("*.prefab");
        int fileCount = info.Length;

        Array.Clear(info, 0, info.Length);

        return fileCount;
    }
}