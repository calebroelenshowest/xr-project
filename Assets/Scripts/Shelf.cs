using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public float coolDownPeriod = 2.0f;
    public float timeSinceCoolDown = 0.0f;

    public Element[] Elements; // Maximum 6 elements
    void Start()
    {
        // Get all spawn postions
        Transform[] spawnPositions = new Transform[6];
        for (int i = 0; i <= 5; i++)
        {
            spawnPositions[i] = gameObject.transform.GetChild(i).transform;
        }
        
        int index = 0;
        if (Elements.Length == 0) return;
        foreach (Element element in Elements)
        {
            if(element == new Element()) continue;
            GameObject spawned = Instantiate(element.element, spawnPositions[index]);
            spawned.transform.parent = null;
            spawned.transform.localScale = defaultSize;
            Destroy(spawned.GetComponent<Rigidbody>());
            index++;
        }
    }
    
    void Update()
    {
        timeSinceCoolDown += Time.deltaTime;
    }
    
    public void OnTriggerExit(Collider other)
    {
        
        Transform[] spawnPostions = new Transform[6];
        for (int i = 0; i <= 5; i++)
        {
            spawnPostions[i] = gameObject.transform.GetChild(i);
        }
        
        // What gameobject is leaving the collison area?
        // Is it an object that was spawned by this shelf?
        bool foundResult = false;       // Found matching shelf object?
        Element respawnElement = null;  // Element to respawn?
        
        int index = 0;
        foreach (Element element in Elements)
        {
            if (other.gameObject.transform.childCount == 0) continue;
            Material collisionGameObjectMaterial = other.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial;
            Debug.Log($"Collided with: {collisionGameObjectMaterial.name}");
            Material elementGameObjectMaterial = element.element.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial;
            Debug.Log($"Checking if it is element {elementGameObjectMaterial.name}");
            
            if (collisionGameObjectMaterial == elementGameObjectMaterial)
            {
                Debug.Log("Its da same collision element bro its good!");
                // Found a matching object
                foundResult = true;
                respawnElement = element;
                
                // Respawn on the spot
                GameObject spawned = Instantiate(respawnElement.element, spawnPostions[index]);
                spawned.transform.parent = null;
                spawned.transform.localScale = defaultSize;
                break;
            }
            index++;
        }
        // No result : Cancel event
        if (!foundResult)
        {
            Debug.Log("Didnt find any matching object to respawn!");
            return;
        }
    }
}
