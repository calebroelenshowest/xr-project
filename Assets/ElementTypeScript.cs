using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementTypeScript : MonoBehaviour
{
    [SerializeField] public Material elementMaterial;
    [SerializeField] public Material lockedMaterial;
    [SerializeField] public bool unlocked;
    [SerializeField] public bool isFinal;
    [SerializeField] public Material combinationMaterial;
    [SerializeField] public Material resultMaterial;

    // Start is called before the first frame update
    void Start()
    {
        GameObject child = this.gameObject.transform.GetChild(0).gameObject;
        
        if (!unlocked)
        { 
            child.GetComponent<Renderer>().material = lockedMaterial; 
        }
        else
        {
            child.GetComponent<Renderer>().material = elementMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
