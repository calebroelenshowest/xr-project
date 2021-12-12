using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    [SerializeField] public BoxCollider ScannerArea;
    [SerializeField] public TextMeshPro TxtScreenOne;
    [SerializeField] public TextMeshPro TxtScreenTwo;
    
    [System.Serializable]
    public class ElementInfo
    {
        [SerializeField] public string Material;
        [SerializeField] public string TxtFactOne;
        [SerializeField] public string TxtFactTwo;
        [SerializeField] public string TxtFactThree;
        [SerializeField] public Combinations[] CombinationsList;
    }
    
    [System.Serializable]
    public class Combinations
    {
        [SerializeField] public string CombinedMaterial;
        [SerializeField] public string CombinationResult;
    }

    public ElementInfo[] elementInfos;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        audioSource.Play();
        var UnlockedElements = SaveData.GetAllUnlockedElementNames(); 
        string ScannedElement = GetElementName(other.gameObject);
        foreach (var element in elementInfos)
        {
            if (element.Material == ScannedElement)
            {
                string txtscreen1 = $"The scanned element is: \n{element.Material}\nFacts:\n - {element.TxtFactOne}\n - {element.TxtFactTwo}\n - {element.TxtFactThree}";
                string txtcreen2 = GetUnlockedCombinations(UnlockedElements, element);
                TxtScreenOne.SetText(txtscreen1);
                TxtScreenTwo.SetText($"Unlocked combinations: \n{txtcreen2}");
            }
        }
    }

    public void OnCollisionExit(Collision other)
    {
        TxtScreenOne.SetText("Please place element on scanner");
        TxtScreenTwo.SetText("");
    }

    private string GetElementName(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++) //overloopt children van parent prefab
        {
            var selectedChild = gameObject.transform.GetChild(i);
            if (selectedChild.CompareTag("ElementMaterial")) // zoekt naar de prefab met tag elementmaterial
            {
                Material gameObjectMaterial = selectedChild.GetComponent<MeshRenderer>().sharedMaterial;
                return gameObjectMaterial.name;
            }
        }
        return "";
    }

    private string GetUnlockedCombinations(string[] unlockedelementsarr, ElementInfo elementinfo)
    {
        string StrUnlockedCombinations = "";
        foreach (var unlockedelement in unlockedelementsarr)
        {
            foreach (var combination in elementinfo.CombinationsList)
            {
                if (combination.CombinationResult == unlockedelement)
                {
                    StrUnlockedCombinations += elementinfo.Material + " + " + combination.CombinedMaterial + " = " + combination.CombinationResult + "\n" ;
                }
            }
        }
        return StrUnlockedCombinations;
    }
}


