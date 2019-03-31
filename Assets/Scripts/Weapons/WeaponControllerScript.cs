using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR L'AVATAR **********

    public Weapon weapon;                                      // Classe "Weapon" des armes
    public GameObject weaponGO;                                // L'arme actuellement en main  

    private float timeFireRate;                                // FireRate de l'arme

    Weapon.AimParameters paramIn;                              // Parametres de zoom max, de la Struct 'AimParameters' contenue dans l'arme
    Weapon.AimParameters paramOut;                             // Parametres de zoom min (de base), de la Struct 'AimParameters' contenue dans l'arme

    Coroutine coroutineAim;                                    // Stockage de la coroutine de zoom (pour ne pas en lancer une nouvelle à chaque StartCoroutine)

    private void Start()
    {
        // Va cherche l'arme actuellement en main
        weapon = GetComponentInChildren(typeof(Weapon)) as Weapon;

        // Parametres de zoom de l'arme (valeurs à atteindre)
        paramIn = new Weapon.AimParameters(weapon.fovInAimMode, weapon.weaponPositionAfterAim);
        paramOut = new Weapon.AimParameters(weapon.fovInNormalMode, weapon.weaponPositionBeforeAim);        
    }

    void Update()
    {
        // Si on appuye sur l'input de tir
        if (Input.GetButton("Fire") && timeFireRate >= 1 / weapon.RPS)
        {
            weapon.HitScanShot();
            timeFireRate = 0;

            // On joue le son de tir de l'arme
            FMODUnity.RuntimeManager.PlayOneShot("event:/Weapons/GunShot", transform.localPosition);
        }
        // On incrémente la valeur du fireRate jusqu'au coup de feu suivant
        if (timeFireRate < 1 / weapon.RPS)
            timeFireRate += Time.deltaTime;

        // Si Clic droit souris enfoncé
        if (Input.GetMouseButtonDown(1))
        {
            // Zoom avec paramètres de zoom max de l'arme
            MoveFOV(paramIn);           
        }

        // Si Clic droit souris relâché
        if (Input.GetMouseButtonUp(1))
        {
            // Zoom reviens vers valeurs initiales (de base) de l'arme
            MoveFOV(paramOut);
        }
    }   

    // Permet de stopper la coroutine de zoom pendant son execution et d'en lancer une autre
    public void MoveFOV(Weapon.AimParameters MoveFOVParam)
    {
        // Si la coroutine est en train de s'executer
        if(coroutineAim != null)
        {
            // On la stoppe
            StopCoroutine(coroutineAim);
        }
        // On redémarre la coroutine avec de nouvelles valeurs
        coroutineAim = StartCoroutine(weapon.Aim(MoveFOVParam));
    }
}