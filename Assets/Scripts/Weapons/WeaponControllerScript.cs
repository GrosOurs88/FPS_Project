using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR L'AVATAR **********

    public Weapon weapon;                                      // Classe "Weapon" des armes
    public GameObject weaponGO;                                // L'arme actuellement en main  

    private float timeFireRate;                                // Chrono pour FireRate de l'arme

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
        // Si on appuye sur l'input de tir et que le fireRate a été atteind et que l'arme a encore des munitions et qu'elle n'est pas en train d'être rechargée
        if (Input.GetButton("Fire") && timeFireRate >= 1 / weapon.RPS && weapon.magazineAmmo > 0 && !weapon.isReloading)
        {
            weapon.HitScanShot();
            timeFireRate = 0;

            // On joue le son de tir de l'arme
            FMODUnity.RuntimeManager.PlayOneShot("event:/Weapons/GunShot", transform.localPosition);
        }
        // On incrémente la valeur du fireRate jusqu'au coup de feu suivant
        if (timeFireRate < 1 / weapon.RPS)
            timeFireRate += Time.deltaTime;


        // Si clic droit souris enfoncé et que l'arme n'est pas en train d'être rechargée
        if (Input.GetMouseButtonDown(1) && !weapon.isReloading)
        {
            // Zoom avec paramètres de zoom max de l'arme
            MoveFOV(paramIn);

            // Modifie la sensibilité de la caméra
            GameObject.Find("Main Camera").GetComponent<CameraControlScript>().sensitivityX = GameObject.Find("Main Camera").GetComponent<CameraControlScript>().sensitivityX / weapon.multiplierCameraSensibilityWhenAim;
            GameObject.Find("Main Camera").GetComponent<CameraControlScript>().sensitivityY = GameObject.Find("Main Camera").GetComponent<CameraControlScript>().sensitivityY / weapon.multiplierCameraSensibilityWhenAim;
        }

        // Si clic droit souris relâché
        if (Input.GetMouseButtonUp(1))
        {
            // Zoom reviens vers valeurs initiales (de base) de l'arme
            MoveFOV(paramOut);

            // Reset la sensibilité de la caméra à la normale
            GameObject.Find("Main Camera").GetComponent<CameraControlScript>().sensitivityX = GameObject.Find("Main Camera").GetComponent<CameraControlScript>().sensitivityX * weapon.multiplierCameraSensibilityWhenAim;
            GameObject.Find("Main Camera").GetComponent<CameraControlScript>().sensitivityY = GameObject.Find("Main Camera").GetComponent<CameraControlScript>().sensitivityY * weapon.multiplierCameraSensibilityWhenAim;
        }

        // Si touche "R" enfoncée et que le chargeur n'est pas déjà plein et que l'arme n'est pas déjà en train d'être rechargée et qu'il reste des munitions en reserve et que l'arme n'est pas en position de visée
        if (Input.GetKeyDown(KeyCode.R) && weapon.magazineAmmo != weapon.magazineSize && !weapon.isReloading && weapon.carriedAmmo > 0 && !Input.GetMouseButton(1))
        {
            // Recharge l'arme
            StartCoroutine(weapon.Reload());
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