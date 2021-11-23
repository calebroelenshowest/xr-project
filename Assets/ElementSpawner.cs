using System.Collections;
using System.Collections.Generic;
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
                Instantiate(location.gameObjectToSpawn, location.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}