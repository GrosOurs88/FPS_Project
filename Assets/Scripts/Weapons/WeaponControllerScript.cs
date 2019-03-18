using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerScript : MonoBehaviour
{
    public Weapon weapon;
    public GameObject weaponGO;


    void Start()
    {
        weapon = GetComponentInChildren(typeof(Weapon)) as Weapon;
    }
 
    void Update()
    {
        // Si on appuye sur l'input de tir //GETBUTTON DOWN !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! (must be GetButton())
        if (Input.GetButtonDown("Fire"))
        {
            weapon.HitScanShot();


            // *** Code pour tirer une bullet au centre de l'écran, quel que soit la distance à laquelle se situe la cible ***

            // 1 - Lancer un Raycast qui part du centre de la caméra, droit devant
            // 2 - Récupérer le point d'impact du raycast (cible)
            // 3 - Faire partir une balle depuis le canon de l'arme, jusqu'à la cible

            // *** Fin du code ***

            // On joue le son de tir de pistolet
            FMODUnity.RuntimeManager.PlayOneShot("event:/Weapons/GunShot");
        }
    }
}
