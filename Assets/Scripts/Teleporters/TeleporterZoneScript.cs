using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterZoneScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR CHAQUE TELEPORTEUR **********

    public GameObject elementToTeleportAt;
    private GameObject zone;                        // Zone de la zone de teleportation au centre du téléporteur 
    private SphereCollider teleportZone;            // Collider de la zone de teleportation au centre du téléporteur 

    private void Start()
    {
        teleportZone = GetComponentInChildren<SphereCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        // Si c'est l'avatar qui se tient sur le téléporteur
        if (other.tag == "Player")
        {
            // Si le téléporteur est bien lié à un autre téléporteur
            if (elementToTeleportAt.tag == "Teleporter")
            {
                // On désactive le sphere collider de la zone de teleport du téléporteur que l'on va atteindre
                zone = elementToTeleportAt.transform.Find("TeleportZone").gameObject;
                zone.GetComponent<SphereCollider>().enabled = false;

                // L'avatar est téléporté sur l'autre téléporteur et récupère sa rotation
                other.transform.position = new Vector3(elementToTeleportAt.transform.position.x, elementToTeleportAt.transform.position.y + 1, elementToTeleportAt.transform.position.z);
                other.transform.rotation = elementToTeleportAt.transform.rotation;
            }

            // Si le téléporteur n'est pas relié à un autre téléporteur
            else
            {
                // Message d'erreur
                Debug.LogError("The elementToTeleportAt of " + gameObject.name + "must be another Teleporter !");
            }
        }
    }
   
}
