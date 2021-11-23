using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;


[SerializeField]
public class ElementSpawnerLocations
{
    public GameObject gameObjectToSpawn;
    public bool unlocked;
}


public class ElementSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public ElementSpawnerLocations[] Locations;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
