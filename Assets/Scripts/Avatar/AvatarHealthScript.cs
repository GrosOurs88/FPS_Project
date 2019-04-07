using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarHealthScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR L'AVATAR **********

    //***POINTS DE VIE****
    public int maxHealth;                   // Points de vie max de l'avatar
    [HideInInspector]
    public int actualHealth;                // Points de vie actuels de l'avatar
    
    //***BOOLEENS***
    private bool alreadyspotted = false;    // Bloque les sons spotted et course ennemi

    //***SCRIPTS***
    private UIScript UIS;                   // script UIScript (gère l'UI de l'arme)    
    private SavePointScript SPS;            // script SavePointScript (gère les savepoints)

    //***TRANSFORM***
    private Transform trans;                // Transform de l'avatar

    private void Start()
    {
        // Va chercher le Script de l'UI dans le MasterUI
        UIS = GameObject.Find("MasterUI").GetComponent<UIScript>();

        // Va chercher le Script de SavePoint dans le MasterSavePoints
        SPS = GameObject.Find("MasterSavePoints").GetComponent<SavePointScript>();

        // Le nombre de points de vie de base est au maxium en début de partie
        actualHealth = maxHealth;

        // Va chercher le Transform de l'avatar
        trans = GetComponent<Transform>();
    }

    public void TakeDamage(int DamageTaken)
    {
        // Réduit les points de vie de l'avatar
        actualHealth -= DamageTaken;

        // Actualise les PV sur l'UI
        UIS.ShowHealth();

        // Si les points de vie atteignent zéro
        if (actualHealth <= 0)
        {
            // L'avatar meurt
            Die();
        }
    }

    // L'avatar meurt
    void Die()
    {
        // L'avatar n'est plus spotted
        alreadyspotted = false;

        // Respawn à l'emplacement du dernier checkpoint
        trans = SPS.avatarCheckpoint;

        // Reset les points de vie
        actualHealth = maxHealth;
        
        // Reset munitions

    }

    // Quand le joueur est vu par un ennemi (trigger)
    private void OnTriggerStay(Collider other)
    {
        // Si l'element du collider touché contient un script EnnemiScript (ennemi qui a vu l'avatar)
        if (other.GetComponent<EnemiScript>())
        {
            // Si le collider de l'ennemi que l'on touche est le colliderVision (cône de vision de l'ennemi)
            if (other == other.GetComponent<EnemiScript>().colliderVision)
            {
                // L'avatar devient la nouvelle target de l'ennemi qui nous a vu
                other.GetComponent<EnemiScript>().target = transform.position;

                // Si l'avatar n'était pas encore spotted
                if (!alreadyspotted)
                {
                    // joue le son de spotted
                    other.gameObject.GetComponent<EnemiScript>()._Spotted_snd.start();

                    // Coupe le son de marche lent
                    other.gameObject.GetComponent<EnemiScript>()._WalkLow_snd.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                    // Démarre le son de marche rapide
                    other.gameObject.GetComponent<EnemiScript>()._WalkFast_snd.start();

                    // La vitesse de l'ennemi qui nous a vu est augmentée
                    other.gameObject.GetComponent<EnemiScript>().navAgent.speed = other.gameObject.GetComponent<EnemiScript>().runSpeed;

                    // now, spotted
                    alreadyspotted = true;
                }
            }
        }
    }

    // Quand le joueur est touché par un ennemi
    private void OnCollisionEnter(Collision col)
    {
        // Si l'element du collider touché contient un script EnnemiScript (ennemi)
        if (col.gameObject.GetComponent<EnemiScript>())
        {
            // Si le collider de l'ennemi est le colliderLethal
            if (col.collider == col.gameObject.GetComponent<EnemiScript>().colliderLethal)
            {
                // On joue le bruit de contact
                FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Punch");

                // On retire à l'avatar les points de vie infligés par le monstre
                TakeDamage(col.transform.GetComponent<EnemiScript>().Damage);


                // L'avatar respawn au dernier checkpoint
                transform.position = SPS.avatarCheckpoint.position;

                // L'ennemi qui a touché l'avatar reprend son pattern de déplacement normal
                col.gameObject.GetComponent<EnemiScript>().target = col.gameObject.GetComponent<EnemiScript>().listPositions[col.gameObject.GetComponent<EnemiScript>().index].position;
            }
        }
    }    

    // Quand le joueur sort du collider d'un ennemi (champ de vision)
    private void OnTriggerExit(Collider other)
    {
        // Si le collider de l'element dont on sort contient un script EnnemiScript (ennemi)
        if (other.GetComponent<EnemiScript>())
        {
            // Si le collider de l'ennemi est le colliderVision (champ de vision)
            if (other == other.GetComponent<EnemiScript>().colliderVision)
            {
                // Coupe le son de marche rapide
                other.gameObject.GetComponent<EnemiScript>()._WalkFast_snd.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                // Démarre le son de marche lent
                other.gameObject.GetComponent<EnemiScript>()._WalkLow_snd.start();

                // l'ennemi reprend sa vitesse de base
                other.gameObject.GetComponent<EnemiScript>().navAgent.speed = other.gameObject.GetComponent<EnemiScript>().normalSpeed;

                // no more spotted
                alreadyspotted = false;

                return;
            }
        }
    }
}
