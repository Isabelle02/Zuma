using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelManager
{
    private static readonly string LevelsFilePath = Path.Combine(Application.persistentDataPath, "Levels.txt");
    public static readonly LevelsConfig LevelsConfig = Resources.Load<LevelsConfig>("LevelsConfig");

    public static LevelData LoadLevel(int index)
    {
        return Storage.Load<LevelsConfig>(LevelsFilePath).Levels[index];
    }

    public static List<LevelData> LoadLevels()
    {
        return Storage.Load<LevelsConfig>(LevelsFilePath).Levels;
    }

    public static void SaveLevels()
    {
        Storage.Save(LevelsFilePath, LevelsConfig);
    }
}