using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Rendering;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[System.Serializable]
public class ElementCombination
{
    public GameObject elementPrefab;
    public GameObject resultPrefab;
}

public class Combinations : MonoBehaviour
{
    [NonSerialized]
    public bool onCoolDown = false;
    [SerializeField] 
    public List<ElementCombination> elementCombinations;
    private bool _holdingOjbect;
    [NonSerialized] 
    public bool onCollisionStayBehind = false;
    public bool onCollisionCode = false;

    public delegate void onNewElementCreation();
    public static event onNewElementCreation NewElementCreation;
    
    // Custom events

    void Start()
    {
        // I have been created!
        // Check if i have been unlocked before!
        // Load the SaveData
        var saveData = SaveData.GetAllUnlockedElementNames();
        if (saveData == null)
        {
            // If data is null --> Ignore start.
            return;
        }

        // Get the name of this object
        Material elementMaterial = GetGameObjectMaterial(this.gameObject);
        string elementMaterialName = elementMaterial.name;
        bool matched = false;
        for (int i = 0; i < saveData.Length; i++)
        {
            if (saveData[i] == elementMaterialName)
            {
                {
                    matched = true;
                    break;
                }
            }
        }

        if (!matched)
        {
            // Not been unlocked before!
            // Trigger unlock so the shelf can spawn it next time.
            NewElementCreation();

        }
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

        if (!gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 1.0f;
        }
    }

    // On collision
    private void OnCollisionEnter(Collision collisionElement)
    {
        // On cooldown ?
        if (onCoolDown) return;
        StartCoroutine(OnCoolDown());
        if (onCollisionCode) return;
        onCollisionCode = true;
        // If it doesn't have a Element Tag, don't bother running this code.
        if(!collisionElement.gameObject.CompareTag("Element"))
        {
            onCollisionCode = false;
            return;
        }
        // First step: Determine what element the other object has!
        GameObject collidedObject = collisionElement.gameObject;
        // Get the material associated with the object.
        var collidedObjectMaterial = GetGameObjectMaterial(collidedObject);
        // Cancel if material is null (a.k.a not found)
        if (collidedObjectMaterial is null) { onCollisionCode = false; return;}
        // Continue with code --> Basic checks complete.
        // Do the collided object have the same name?
        if (collidedObjectMaterial.name == GetGameObjectMaterial(gameObject).name) { onCollisionCode = false; return;}
        // The object have different inner element --> Check if they have a combination!
        // Loop over the combinations and compare them.
        GameObject resultGameObjectFound = null;
        for (int i = 0; i < elementCombinations.Count; i++)
        {
            var prefab = elementCombinations[i].elementPrefab;
            if(prefab is null) continue; // Entry is not filled.
            var combinationMaterialName = GetGameObjectMaterial(prefab);
            if (combinationMaterialName is null) continue; // Continue if null
            if (combinationMaterialName.name == collidedObjectMaterial.name)
            {
                // The element has a matching combination!
                // The other object also has a script. To remove only one, we will use a spatial variable.
                // Spatial chosen variable: y
                // If spatial y is the same -> Use x
                Vector3 collidedObjectVector = collidedObject.transform.position;
                Vector3 thisObjectVector = gameObject.transform.position;
                // Compare: What block has the highest position?
                if (collidedObjectVector.y > thisObjectVector.y)
                {
                    // Remove the collided object vector!
                    collisionElement.gameObject.GetComponent<Combinations>().onCollisionCode = true;
                    Destroy(collisionElement.gameObject, 1);
                    resultGameObjectFound = elementCombinations[i].resultPrefab;
                } 
                else if (collidedObjectVector.y == thisObjectVector.y)  
                {   // Comparing floats gives bad result, and can lead to unexpected situations due C# rounding.
                    // See: https://www.jetbrains.com/help/resharper/CompareOfFloatsByEqualityOperator.html
                    // Handle the function using spatial x
                    if (collidedObjectVector.x > thisObjectVector.x)
                    {
                        collisionElement.gameObject.GetComponent<Combinations>().onCollisionCode = true;
                        Destroy(collisionElement.gameObject, 1);
                        resultGameObjectFound = elementCombinations[i].resultPrefab;
                    }
                    onCollisionCode = false; 
                    return;
                } 
                else
                {
                    onCollisionCode = false; 
                    return;
                }
            }
            // Check if object is not null --> Unexpected behaviour.
            if(resultGameObjectFound is null)
            {
                Debug.LogWarning("!! Result game object at element combinations is null. Unexpected behaviour!");
                onCollisionCode = false; 
                return;
            }
            // Found the correct combination.
            // Executing combination code.
            // Replace the existing element with a new result element.  

            Vector3 resultGameObjectLocation = gameObject.transform.position;
            resultGameObjectLocation.z += 0.5f;
            Instantiate(resultGameObjectFound, resultGameObjectLocation, Quaternion.identity, null);
            Destroy(gameObject, 1);
            onCollisionCode = false;
            return;
        }
    }
    

    private Material GetGameObjectMaterial(GameObject gameObject)
    {
        // Loop over the children and retrieve the tagged element
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var selectedChild = gameObject.transform.GetChild(i);
            if (selectedChild.CompareTag("ElementMaterial"))
            {
                // Found the element!
                return selectedChild.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }
        return null;
    }

    IEnumerator OnCoolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(2);
        onCoolDown = false;
    }
    
}
