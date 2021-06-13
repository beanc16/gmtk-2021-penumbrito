using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaving : MonoBehaviour
{
    private static string highestLevelKey;
    private static int currentLevel = 1;



    public static int GetHighestLevel()
    {
        int highestLevel = PlayerPrefs.GetInt(highestLevelKey);

        if (highestLevel == null)
        {
            return 1;
        }

        return highestLevel;
    }

    public static void IncrementHighestSavedLevel()
    {
        int highestLevel = LevelSaving.GetHighestLevel();
        highestLevel++;
        
        PlayerPrefs.SetInt(highestLevelKey, highestLevel);
    }

    public static int GetCurrentLevel()
    {
        return LevelSaving.currentLevel;
    }

    public static bool NextLevelIsHighScore()
    {
        return LevelSaving.GetHighestLevel() <= LevelSaving.GetCurrentLevel();
    }
}
