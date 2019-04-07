using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlatformScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR CHAQUE TELEPORTEUR **********

    // Si l'avatar quitte le collider
    public void OnTriggerExit(Collider other)
    {
        // Si c'est l'avatar qui se tient sur le téléporteur
        if (other.tag == "Player")
        {
            // On réactive le collider central (qui permet la téléportation)
            transform.Find("TeleportZone").gameObject.GetComponent<SphereCollider>().enabled = true;            
        }
    }
}