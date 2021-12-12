using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawnSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y += 1.25f;
        transform.position = currentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
