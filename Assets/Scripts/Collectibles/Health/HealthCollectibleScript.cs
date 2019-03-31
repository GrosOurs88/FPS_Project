using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectibleScript : MonoBehaviour
{
    float rotateSpeed = 100f;

    public int amountOfHealth;  // Nombre de points de vie rendus en récupérant le collectible

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Ajoute le nombre de munitions du collectible dans la reserve de l'avatar
            other.GetComponentInChildren<AvatarHealthScript>().actualHealth += amountOfHealth;

            // Si le nouveau montant de la reserve est supérieur au montant maximal de la réserve
            if (other.GetComponentInChildren<AvatarHealthScript>().actualHealth > other.GetComponentInChildren<AvatarHealthScript>().maxHealth)
            {
                // Redescend le montant de la reserve à son montant maximal
                other.GetComponentInChildren<AvatarHealthScript>().actualHealth = other.GetComponentInChildren<AvatarHealthScript>().maxHealth;
            }
            // Met à jour le nombre de balles en réserve dans l'UI
            GameObject.Find("MasterUI").GetComponent<UIScript>().ShowHealth();

            // Détruit le collectible
            Destroy(gameObject);
        }
    }
}
