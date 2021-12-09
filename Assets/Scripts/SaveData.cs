using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    // This class is used to load saveData and manipulate SavedData
    public static int GetUnlockedCount()
    {
        return GetAllUnlockedElementNames().Length;
    }

    public static string[] GetAllUnlockedElementNames()
    {
        // Load the data
        string rawData = PlayerPrefs.GetString("UnlockedElements");
        if (rawData is "")
        {
            return null;
        }
        // Split the data
        string[] splitData = rawData.Split(':');
        return splitData;
    }

    public static void AddUnlock(string elementName)
    {
        Debug.Log($"Unlocking {elementName}");
        bool isUnlocked = IsElementUnlocked(elementName);
        if (!isUnlocked)
        {
            PlayerPrefs.SetString("UnlockedElements", PlayerPrefs.GetString("UnlockedElements") + $"{elementName}:");
            PlayerPrefs.Save();
        }
    }

    public static void AddUnlockByElement(Shelf.Element element)
    {
        string name = GetElementName(element.element);
        string originalData = PlayerPrefs.GetString("UnlockedElements");
        originalData += name + ":";
        PlayerPrefs.SetString("UnlockedElements", originalData);
        PlayerPrefs.Save();
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

    public static string[] InitSaveData(Shelf.Element[] elements)
    {
        // Loop over the elements and get their unlocked status
        Debug.Log($"SaveData: {elements.Length} elements pending for unlock approval.");
        PlayerPrefs.SetString("UnlockedElements", "");
        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].unlocked)
            {
                Debug.Log($"SaveData: Adding element {elements[i].element.name}");
                // This element should be unlocked.
                // Add it to the unlocks
                AddUnlockByElement(elements[i]);
                Debug.Log("SaveData: Done adding element");
            }
        }

        PlayerPrefs.Save();
        return GetAllUnlockedElementNames();
    }
    
    private static string GetElementName(GameObject gameObject)
    {
        // Get the name of an element by its gameobject meshrenderer material.
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var selectedChild = gameObject.transform.GetChild(i);
            if (selectedChild.CompareTag("ElementMaterial"))
            {
                Material gameObjectMaterial = selectedChild.GetComponent<MeshRenderer>().sharedMaterial;
                return gameObjectMaterial.name;
            }
        }
        return "";
    }

    public static void ClearSaveData()
    {
        // WARNING: THIS WILL CLEAR A L L DATA
        PlayerPrefs.DeleteAll();
    }

    public static void LogSaveData(string saveDataKey)
    {
        Debug.Log(PlayerPrefs.GetString(saveDataKey));
    }
}
