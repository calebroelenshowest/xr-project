using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using TMPro.EditorUtilities;
using UnityEngine;

public class ElementTypeScript : MonoBehaviour
{
    [SerializeField] public Material elementMaterial;
    [SerializeField] public Material lockedMaterial;
    [SerializeField] public bool unlocked;
    [SerializeField] public bool isFinal;
    [SerializeField] public Material combinationMaterial;
    [SerializeField] public Material resultMaterial;
    [SerializeField] public bool holoHolding = false;
    public bool onCollisionStayBehind;

    // Start is called before the first frame update
    void Start()
    {
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        
        if (!unlocked)
        { 
            child.GetComponent<Renderer>().material = lockedMaterial; 
        }
        else
        {
            child.GetComponent<Renderer>().material = elementMaterial;
        }
    }

    public void HoldObject()
    {
        Debug.Log("Changed holoHolding state to true");
        holoHolding = true;
    }

    public void ReleaseObject()
    {
        Debug.Log("Changed holoHolding state to false");
        holoHolding = false;
    }

    public void OnCollisionStay(Collision other)
    {
        if (!other.gameObject.CompareTag(this.gameObject.tag))
        {
            return;
        }
        if (holoHolding)
        {
            Debug.Log("Holding");
            // Do nothing: Only combine elements when released
        }
        else
        {
            Debug.Log("Not holding");
            // Combine elements
            if (onCollisionStayBehind) return; // Cancel destruction
            other.gameObject.GetComponent<ElementTypeScript>().onCollisionStayBehind = true;
            Destroy(other.gameObject);
            gameObject.GetComponent<Collider>().enabled = false;
            GameObject newElement = Instantiate(gameObject);
            ElementTypeScript script = newElement.gameObject.GetComponent<ElementTypeScript>();
            script.unlocked = true;
            script.elementMaterial = resultMaterial;
            script.isFinal = true;
            Destroy(gameObject);


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
