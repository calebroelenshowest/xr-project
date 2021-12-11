using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
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
        // Update the assigned screen.
        // Get the count
        PlayerPrefs.Save(); // Save all writing operation to the register.
        int elementCount = SaveData.GetAllUnlockedElementNames().Length;
        unlocksTextField.SetText($"Unlocked: {elementCount}");
        
    }
}
