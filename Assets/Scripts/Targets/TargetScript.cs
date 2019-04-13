using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR CHAQUE ENNEMI **********
    
    //***COLLIDERS***
    public Collider colliderLethal;                       // Collider lethal de l'ennemi (contact)

    //***POINTS DE VIE***
    public int maxHealth;                                 // Points de vie max de l'ennemi
    [HideInInspector]
    public int actualHealth;                              // Points de vie actuels de l'ennemi

    private GameObject Avatar;

    UIScript UIS;
    AvatarMovementScript AMS;

    public float avatarSpeedIncreasingWhenDestroyed;

    void Start()
    {       
        // Le nombre de points de vie de base est au maxium en début de partie
        actualHealth = maxHealth;

        Avatar = GameObject.Find("Avatar");

        // Va chercher le script UIScript dans le MasterUI
        UIS = GameObject.Find("MasterUI").GetComponent<UIScript>();
        // Va chercher le script AvatarMovementScript dans l'Avatar
        AMS = GameObject.Find("Avatar").GetComponent<AvatarMovementScript>();
    }    


    // L'ennemi meurt
    public void Die()
    {
        // GameObject clone = Instantiate(bullet, fireAmmoPosition.position, fireAmmoPosition.rotation); //Old Instantiation
        GameObject clone = Instantiate(Resources.Load("EnergyBlock/EnergyBlock"), transform.localPosition, transform.localRotation) as GameObject;

        // La target du block est cette porte
        clone.GetComponent<EnergyBlockScript>().target = Avatar;

        // la vitesse de l'avatar est augmentée
        // AMS.speed = AMS.speed + avatarSpeedIncreasingWhenDestroyed;

        // On détruit la target
        Destroy(gameObject);
    }
}
