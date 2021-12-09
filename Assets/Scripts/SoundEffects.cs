using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] public bool glassSoundEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor") && glassSoundEnabled)
        {
            AudioSource sound = GetComponent<AudioSource>();
            sound.Play();
        }
    }
}
