using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterStop : MonoBehaviour
{

    ParticleSystem partSystem;
    GameObject partGo;
    float i = 0;
    // Start is called before the first frame update
    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
        partGo = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        i += Time.deltaTime;
        if (i > 1)
            Destroy (partGo);
    }
}
