using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICounter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public TextMeshPro unlocksTextField;
    void Start()
    {
        Shelf.onShelfUpdateEvent += UpdateUI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        Debug.Log("UI: Update to counter");
        // Update the assigned screen.
        // Get the count
        PlayerPrefs.Save(); // Save all writing operation to the register.
        int elementCount = SaveData.GetAllUnlockedElementNames().Length - 1;
        unlocksTextField.SetText($"Unlocked: {elementCount}");
        
    }
}
