using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterStop : MonoBehaviour
{

    ParticleSystem partSystem;
    // Start is called before the first frame update
    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (partSystem.time > 100)
            Destroy (this);
    }
}
