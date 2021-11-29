using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Microsoft.MixedReality.Toolkit.UI;
using Newtonsoft.Json.Serialization;
using UnityEngine;


public class ElementSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ElementSpawnerLocation
    {
        [SerializeField] public GameObject gameObjectToSpawn;
        [SerializeField] public bool unlocked;
        [SerializeField] public Transform position;
    }
    // Start is called before the first frame update
    [SerializeField] public Vector3 DefaultSize;
    [SerializeField]
    public List<ElementSpawnerLocation> Locations;
    void Start()
    {
        foreach (ElementSpawnerLocation location in Locations)
        {
            // Check for state
            if (location.unlocked)
            {
                //Continue
                // --> Spawn this element !
                GameObject spawnedInstance = Instantiate(location.gameObjectToSpawn, location.position);
                // Make it a dead copy
                Rigidbody rigidBody = spawnedInstance.GetComponent<Rigidbody>();
                spawnedInstance.transform.parent = null;
                spawnedInstance.transform.localScale = DefaultSize;
                Destroy(rigidBody);
                
            }
        }
    }

    public void RespawnElement(ElementSpawnerLocation location, Vector3 size)
    {
        GameObject spawnedInstance = Instantiate(location.gameObjectToSpawn, location.position);
        // Make it a dead copy
        Rigidbody rigidBody = spawnedInstance.GetComponent<Rigidbody>();
        spawnedInstance.transform.parent = null;
        spawnedInstance.transform.localScale = size;
        Destroy(rigidBody);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}