using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerScript : MonoBehaviour
{
    public Weapon weapon;
    public GameObject weaponGO;

    private float timeFireRate;


    void Start()
    {
        weapon = GetComponentInChildren(typeof(Weapon)) as Weapon;
    }
 
    void Update()
    {
        // Si on appuye sur l'input de tir //GETBUTTON DOWN !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! (must be GetButton())
        if (Input.GetButton("Fire") && timeFireRate >= 1 / weapon.RPS)
        {
            weapon.HitScanShot();
            timeFireRate = 0;


            // *** Code pour tirer une bullet au centre de l'écran, quel que soit la distance à laquelle se situe la cible ***

            // 1 - Lancer un Raycast qui part du centre de la caméra, droit devant
            // 2 - Récupérer le point d'impact du raycast (cible)
            // 3 - Faire partir une balle depuis le canon de l'arme, jusqu'à la cible

            // *** Fin du code ***

            // On joue le son de tir de l'arme
            FMODUnity.RuntimeManager.PlayOneShot("event:/Weapons/GunShot", transform.localPosition);
        }
        if (timeFireRate < 1 / weapon.RPS)
            timeFireRate += Time.deltaTime;
    }
}
