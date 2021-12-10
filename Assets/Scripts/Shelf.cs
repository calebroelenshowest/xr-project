using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Shelf : MonoBehaviour
{
    [SerializeField] public BoxCollider ShelfArea;
    [SerializeField] public Vector3 defaultSize = new Vector3(0.3f, 0.3f, 0.3f);
    
    [System.Serializable]
    public class Element
    {
        [SerializeField] public GameObject element;
        [SerializeField] public string holoGraphicText;
        [SerializeField] public bool unlocked;

    }

    public int coolDownPeriod = 2;
    public bool onCoolDown = false;

    public Element[] Elements; // Maximum 6 elements
    void Start()
    {
        // Subscribe to events
        Combinations.NewElementCreation += UpdateElements;
        
        
        SaveData.LogSaveData("UnlockedElements");
        // Populate the shelf with elements
        // Use the children of the shelf as spawn positions.
        // Step 1: Get children (spawn positions)
        var childCount = transform.childCount;
        if (childCount == 0)
        {
            // Spawnpositions are missing
            throw new Exception("Spawnpositions are missing");
        }
        
        // Load save data
        var unlockedElementNames = SaveData.GetAllUnlockedElementNames();
        if (unlockedElementNames is null)
        {
            Debug.Log("No SaveData found!");
            // Initialize Save data with unlocked entries
            List<Element> unlockedElements = new List<Element>();
            foreach (Element element in Elements)
            {
                if(element.unlocked) unlockedElements.Add(element);
            }
            unlockedElementNames = SaveData.InitSaveData(unlockedElements.ToArray());
        }
        if (unlockedElementNames is null) return;
        // Are all knows elements filled? No empty list items?
        // Is the element unlocked already?
        Element[] markedForRemoval = new Element[Elements.Length];
        for (int i = 0; i < Elements.Length; i++)
        {
            if (Elements[i] == default(Element))
            {
                // This is an empty element
                markedForRemoval[i] = Elements[i];
                // Removed from the Elements list.
                Debug.LogWarning("There is an empty element in this shelf! Please remove it to avoid future problems.");
                continue;
            }
            
            // Unlock mechanism
            bool unlocked = false;
            string checkingElementName = GetElementName(Elements[i].element);
            for(int j = 0; j < unlockedElementNames.Length; j++)
            {
                string unlockedElementName = unlockedElementNames[j];
                unlocked = unlockedElementName == checkingElementName;
                if (unlocked) {
                    Debug.LogWarning($"This element will be added: {unlockedElementName}");
                    break; // Found the matching element: stop searching and break.
                }           
            }
            
            // If element has been unlocked
            if (unlocked) continue;
            // The element has not been unlocked yet. Remove.
            markedForRemoval[i] = Elements[i];
        }
        // Remove all elements marked for removal.
        for (int x = 0; x < markedForRemoval.Length; x++)
        {
            if(markedForRemoval[x] == default(Element)) continue;
            if(markedForRemoval[x] == null) continue;
            Elements = Elements.Except(new []{markedForRemoval[x]}).ToArray();
        }
        // Done cleaning up the Elements array. Try spawning in the elements.
        int elementsCount = Elements.Length;
        Debug.LogWarning($"Element count left: {elementsCount}");
        for (int i = 0; i < elementsCount; i++)
        {
            SpawnGameObject(Elements[i], transform.GetChild(i));
        }
    }
    
    void Update()
    {
        
    }

    IEnumerator OnCoolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(coolDownPeriod);
        onCoolDown = false;
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (onCoolDown) return;
        // When we exit the shelf, this code will trigger
        // First step: What element has left the shelf?
        GameObject exitObject = other.gameObject;
        // What is the element type of the exit object?
        for (int i = 0; i < exitObject.transform.childCount; i++)
        {
            string elementName = GetElementName(exitObject);
            // Now we have to respawn the element after 2 seconds. First enable the cooldown so we cant spawn.
            StartCoroutine(OnCoolDown());
            // Spawn the element.
            // Trigger the respawn system and send the name of the element to make it more memory efficient.
            RespawnElement(elementName);
            // End the loop
            break;
        }
    }

    private void SpawnGameObject(Element elementToSpawn, Transform spawnPos)
    {
        GameObject respawnGameObject = elementToSpawn.element;
        // Continue, now we can really spawn it back in.
        var spawnLocationPosition = spawnPos.position;
        Vector3 respawnObjectLocation =
            new Vector3(spawnLocationPosition.x, spawnLocationPosition.y, spawnLocationPosition.z);
        // Create the GameObject without any parent, a null rotation and a fixed new location.
        GameObject spawnedObject = Instantiate(respawnGameObject, spawnLocationPosition, Quaternion.identity, null);
        // Destroy the RigidBody so it stays in a fixed location. (aka Remove gravity)
    }

    private string GetElementName(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var selectedChild = gameObject.transform.GetChild(i);
            if (selectedChild.CompareTag("ElementMaterial"))
            {
                Material gameObjectMaterial = selectedChild.GetComponent<MeshRenderer>().sharedMaterial;
                return gameObjectMaterial.name;
            }
        }
        Debug.LogWarning($"This element is missing a ElementMaterial Tag! {gameObject}");
        return "";
    }

    private void RespawnElement(string elementName)
    {
        // Find the GameObject in the elements List
        Element elementToSpawn = default(Element);
        int elementToSpawnIndex = 0;
        for (int i = 0; i < Elements.Length; i++)
        {
            if (elementName == GetElementName(Elements[i].element))
            {
                elementToSpawn = Elements[i];
                elementToSpawnIndex = i;
                break;
            }
        }
        if (elementToSpawn == default(Element))
        {
            // Its empty! No element corresponded with the exiting element.
            return;
        }
        SpawnGameObject(elementToSpawn, transform.GetChild(elementToSpawnIndex));
    }

    public void UpdateElements()
    {
        // Element spawned event
        Debug.LogWarning("Element Update Event received!");
        // Go over the list and update the data!
        
    }
}
