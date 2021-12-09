using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ParticleEmitter : MonoBehaviour
{
    public string[] tagFields;
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
        if (other.gameObject.CompareTag("Floor"))
        {
            // Play the particle system
            ParticleSystem particles = GetComponent<ParticleSystem>();
            particles.Play();
        }
    }
}
