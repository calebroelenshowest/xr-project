using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    // This class is used to load saveData and manipulate SavedData
    private static int GetUnlockedCount()
    {
        return 0;
    }

    public static string[] GetAllUnlockedElementNames()
    {
        // Load the data
        string rawData = PlayerPrefs.GetString("UnlockedElements");
        if (rawData is null)
        {
            return null;
        }
        // Split the data
        string[] splitData = rawData.Split(':');
        return splitData;
    }

    public static void AddUnlock(string elementName)
    {
        bool isUnlocked = IsElementUnlocked(elementName);
        if (isUnlocked)
        {
            PlayerPrefs.SetString("UnlockedElements", PlayerPrefs.GetString("UnlockedElements") + $"{elementName}:");       
        }
    }

    public static bool IsElementUnlocked(string elementName)
    {
        string[] unlockedElementNames = GetAllUnlockedElementNames();
        for (int i = 0; i < unlockedElementNames.Length; i++)
        {
            if (unlockedElementNames[i] == elementName)
            {
                // Already unlocked
                return true;
            }
        }
        return false;
    }
}
