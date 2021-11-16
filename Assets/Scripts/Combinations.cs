using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Rendering;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[System.Serializable]
public class ElementCombination
{
    public Material elementMaterial;
    public GameObject resultPrefab;
}

public class Combinations : MonoBehaviour
{
    [SerializeField]
    public List<ElementCombination> elementCombinations;

    private bool _holdingOjbect;
    [NonSerialized]
    public bool onCollisionStayBehind = false;
    void Start()
    {
    }
    
    void Update()
    {
        // Update method : Update once every frame
        
    }

    public void HoldObject()
    {
        Debug.Log("Holding this object with hand.");
        _holdingOjbect = true;
    }

    public void ReleaseObject()
    {
        Debug.Log("Released this object with hand. No longer holding this.");
        _holdingOjbect = false;
    }

    // On collision
    private void OnCollisionEnter(Collision collisionElement)
    {
        // Does the object have the same tag: Element
        if (collisionElement.gameObject.CompareTag(gameObject.tag)){
            
            // If still holding the object --> Do not combine
            if (_holdingOjbect)
            {
                Debug.Log("Invalid holding state");
                return;
            }
            // If not holding --> Combine both elements if possible
            Material collisionMaterial = collisionElement.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
            Debug.Log(collisionMaterial.name);
            // Cannot combine with itself.
            if (collisionMaterial == gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material)
            {
                Debug.Log("Same element collision"); 
                return; 
            } 
                
            if (elementCombinations.Count == 0)
            {
                Debug.Log("No elements counted");
                Debug.Log("Final element!");
                return;
            } // This is a final element --> No elements inside

            bool foundMatching = false;
            foreach(ElementCombination combination in elementCombinations)
            {
                Debug.Log($"{combination.elementMaterial.name} ?= {collisionMaterial.name}");
                if (combination.elementMaterial.mainTexture == collisionMaterial.mainTexture)
                {
                    Debug.Log("Triggered for loop");
                    foundMatching = true;
                    // Destroy colling object and remove existing collider on existing object.
                    if (onCollisionStayBehind) return;
                    collisionElement.gameObject.GetComponent<Combinations>().onCollisionStayBehind = true;
                    Destroy(collisionElement.gameObject.GetComponent<Combinations>());
                    Destroy(collisionElement.gameObject);
                    gameObject.GetComponent<Collider>().enabled = false;
                    // Spawn the resulting combination
                    int index = elementCombinations.IndexOf(combination);
                    Vector3 gameObjectPos = gameObject.transform.position;
                    Instantiate(elementCombinations[index].resultPrefab,
                        new Vector3(gameObjectPos.x, gameObjectPos.y + 1, gameObjectPos.z), Quaternion.identity);
                    // Remove the existing element
                    Destroy(gameObject);
                    break;
                }
            }

            // No matching material? Cancel!
            if (!foundMatching)
            {
                Debug.Log("No matching material found");
                return;
            }
                
            
        }
        else
        {
            // Combination of elements not allowed.
            Debug.Log("This collision is with da floor");
            return;
        }
    }
}
