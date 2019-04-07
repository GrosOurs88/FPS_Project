using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CollectibleScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR CHAQUE COLLECTIBLE **********

    private float rotateSpeed = 100f;   // Vitesse de rotation du collectible

    public bool bulletsCollectible;     // Le ecollectible ajoute des munitions ?
    public int amountOfBullets;         // Nombre de munitions ramassées en récupérant le collectible

    public bool healCollectible;      // Le collectible ajoute des PV ?
    public int amountOfHealth;          // Nombre de points de vie rendus en récupérant le collectible
      

    void Update()
    {
        Rotate();   // Rotate le GameObject
    }

    // Rotate le GameObject
    public void Rotate()
    {
        // Rotate le GameObject autour de l'axe Y
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    // Ramassé quand Avatar passe dessus
    private void OnTriggerEnter(Collider other)
    {
        // Si l'avatar passe dessus
        if (other.tag == "Player")
        {
            // Vérifie l'état des booléens de cet objet pour attribuer les bonus adéquat
            // Si le booleen de munitions est coché
            if (bulletsCollectible)
            {
                // Ajoute le nombre de munitions du collectible dans la reserve de l'avatar
                other.GetComponentInChildren<Weapon>().carriedAmmo += amountOfBullets;

                // Si le nouveau montant de la reserve est supérieur au montant maximal de la réserve
                if (other.GetComponentInChildren<Weapon>().carriedAmmo > other.GetComponentInChildren<Weapon>().maxCarriedAmmo)
                {
                    // Redescend le montant de la reserve à son montant maximal
                    other.GetComponentInChildren<Weapon>().carriedAmmo = other.GetComponentInChildren<Weapon>().maxCarriedAmmo;
                }

                // Met à jour le nombre de balles en réserve dans l'UI
                GameObject.Find("MasterUI").GetComponent<UIScript>().ShowReserveAmmunitions();
            }

            // Si le booleen de soin est coché
            if (healCollectible)
            {
                // Ajoute le nombre de Points de vie du collectible à l'avatar
                other.GetComponentInChildren<AvatarHealthScript>().actualHealth += amountOfHealth;

                // Si le nouveau montant de PV est supérieur au montant maximal de PV de l'avatar
                if (other.GetComponentInChildren<AvatarHealthScript>().actualHealth > other.GetComponentInChildren<AvatarHealthScript>().maxHealth)
                {
                    // Redescend les PV au montant max de l'avatar
                    other.GetComponentInChildren<AvatarHealthScript>().actualHealth = other.GetComponentInChildren<AvatarHealthScript>().maxHealth;
                }
                // Met à jour les PV de l'avatar dans l'UI
                GameObject.Find("MasterUI").GetComponent<UIScript>().ShowHealth();
            }                       

            // Détruit le collectible
            Destroy(gameObject);
        }
    }   
}

// Permet de rentrer les valeurs des collectibles de façon plus propre
[CustomEditor(typeof(CollectibleScript))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        // Prend ce component comme référence 
        var myScript = target as CollectibleScript;

        // Le booleen bulletsCollectible apparait dans l'editeur avec le titre "Bullets"
        myScript.bulletsCollectible = GUILayout.Toggle(myScript.bulletsCollectible, "Bullets");
        
        // Si le booléen de munitions est coché
        if (myScript.bulletsCollectible)
        {
            // On affiche le champ du nombre de munitions que donne le collectible à l'avatar
            myScript.amountOfBullets = EditorGUILayout.IntField("Bullets collected", myScript.amountOfBullets);
        }

        // Le booleen healCollectible apparait dans l'editeur avec le titre "Heal"
        myScript.healCollectible = GUILayout.Toggle(myScript.healCollectible, "Heal");

        // Si le booléen de heal est coché
        if (myScript.healCollectible)
        {
            // On affiche le champ du nombre de PV que donne le collectible à l'avatar
            myScript.amountOfHealth = EditorGUILayout.IntField("Health recovered", myScript.amountOfHealth);
        }

        // Si les valeurs de l'inspecteur ont changées
        if (GUI.changed)
        {
            // Les valeurs de l'asset sont conservées
            EditorUtility.SetDirty(myScript);
        }
    }
}
