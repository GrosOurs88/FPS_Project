using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithParent : MonoBehaviour
{
    public GameObject Ava;

    ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        ParticleSystem.ShapeModule sm = ps.shape;
        sm.rotation = Ava.transform.localEulerAngles;
    }
}