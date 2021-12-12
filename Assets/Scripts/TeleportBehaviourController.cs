using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class TeleportBehaviourController : MonoBehaviour
{
    [NonSerialized]
    public bool TargetedOnObject = false;
    [SerializeField]
    public Camera teleportObject;

    [SerializeField] public Material hoverMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetedOnObject)
        {
            
            if (Input.GetMouseButtonDown(0)) // Testing using mouse button left.
            {
                // Teleport to the object if its an accepted object.
                Vector3 newPosition = transform.position;
                newPosition.y += 1;
                teleportObject.gameObject.transform.position = newPosition;
            }
            
        }
    }

    public void OnObjectHoverEnter()
    {
        TargetedOnObject = true;
    }

    public void OnObjectHoverExit()
    {
        TargetedOnObject = false;
    }

    public void ToggleHoverEffect()
    {
        
    }
}

