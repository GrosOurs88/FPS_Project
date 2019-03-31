using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsCollectibleScript : MonoBehaviour
{
    float rotateSpeed = 100f;

    public int amountOfBullets;  // Nombre de balles ramassées en récupérant le collectible

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);   
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // Ajoute le nombre de munitions du collectible dans la reserve de l'avatar
            other.GetComponentInChildren<Weapon>().carriedAmmo += amountOfBullets;

            // Si le nouveau montant de la reserve est supérieur au montant maximal de la réserve
            if(other.GetComponentInChildren<Weapon>().carriedAmmo > other.GetComponentInChildren<Weapon>().maxCarriedAmmo)
            {
                // Redescend le montant de la reserve à son montant maximal
                other.GetComponentInChildren<Weapon>().carriedAmmo = other.GetComponentInChildren<Weapon>().maxCarriedAmmo;
            }
            // Met à jour le nombre de balles en réserve dans l'UI
            GameObject.Find("MasterUI").GetComponent<UIScript>().ShowReserveAmmunitions();

            // Détruit le collectible
            Destroy(gameObject);
        }
    }
}
