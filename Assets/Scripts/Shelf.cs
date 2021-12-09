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
        // Populate the shelf with elements
        // Use the children of the shelf as spawn positions.
        // Step 1: Get children (spawn positions)
        var childCount = transform.childCount;
        Debug.Log($"Found {childCount} spawnpositions.");
        if (childCount == 0)
        {
            // Spawnpositions are missing
            throw new Exception("Spawnpositions are missing");
        }
        // How many elements are added to the shelf?
        var shelfElementCount = Elements.Length;
        // Are all knows elements filled? No empty list items?
        // Is the element unlocked already?

        string[] unlockedElementNames = SaveData.GetAllUnlockedElementNames();
        
        for (int i = 0; i < Elements.Length; i++)
        {
            if (Elements[i] == default(Element))
            {
                // This is an empty element
                Elements = Elements.Except(new[] {Elements[i]}).ToArray();
                // Removed from the Elements list.
                Debug.LogWarning("There is an empty element in this shelf! Please remove it to avoid future problems.");
                continue;
            }
            
            // Unlock mechanism
            bool unlocked = false;
            
            for(int j = 0; j < unlockedElementNames.Length; j++)
            {
                string unlockedElementName = unlockedElementNames[j];
                string checkingElementName = GetElementName(Elements[i].element);
                unlocked = unlockedElementName == checkingElementName;
            }

            if (!unlocked)
            {
                // The element has not been unlocked yet. Remove.
                Elements = Elements.Except(new[] {Elements[i]}).ToArray();
            }

        }
        // Done cleaning up the Elements array. Try spawning in the elements.
        var elementsCount = Elements.Length;
        
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
    
}
