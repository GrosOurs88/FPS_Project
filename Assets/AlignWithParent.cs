using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithParent : MonoBehaviour
{
    Camera cam;
    public GameObject Ava;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInParent<Camera>();
        print (cam.gameObject.name);
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.rotation = Ava.transform.rotation;
    }
}
