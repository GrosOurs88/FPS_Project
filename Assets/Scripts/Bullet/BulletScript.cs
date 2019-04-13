using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Target")
        {
            if (collision.transform != null && collision.transform.GetComponent<TargetScript>() != null)
            {
                collision.transform.GetComponent<TargetScript>().Die();
            }
        }
        Destroy(gameObject);
    }
}
