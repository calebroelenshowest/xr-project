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

    [SerializeField] 
    public Material hoverMaterial;

    private Material normalMaterial;
    // Start is called before the first frame update
    void Start()
    {
        normalMaterial = this.gameObject.GetComponent<Renderer>().materials[0];
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
        ToggleHoverEffect(true);
    }

    public void OnObjectHoverExit()
    {
        TargetedOnObject = false;
        ToggleHoverEffect(false);
    }

    public void ToggleHoverEffect(bool on)
    {
        if (on)
        {
            gameObject.GetComponent<Renderer>().material = hoverMaterial;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = normalMaterial;
        }
    }
}

