using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR L'AVATAR **********

    public Weapon weapon;                                      // Classe "Weapon" des armes
    public GameObject weaponGO;                                // L'arme actuellement en main  

    private float timeFireRate;                                // FireRate de l'arme

    Weapon.AimParameters paramIn;
    Weapon.AimParameters paramOut;

    Coroutine coroutineAim;

    private void Start()
    {
        // Va cherche l'arme actuellement en main
        weapon = GetComponentInChildren(typeof(Weapon)) as Weapon;

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
        if (timeFireRate < 1 / weapon.RPS)
            timeFireRate += Time.deltaTime;

        // Si Clic droit souris enfoncé
        if (Input.GetMouseButtonDown(1))
        {
            MoveFOV(paramIn);           
        }

        // Si Clic droit souris relâché
        if (Input.GetMouseButtonUp(1))
        {
            MoveFOV(paramOut);
        }
    }   

    public void MoveFOV(Weapon.AimParameters MoveFOVParam)
    {
        if(coroutineAim != null)
        {
            StopCoroutine(coroutineAim);
        }
        coroutineAim = StartCoroutine(weapon.Aim(MoveFOVParam));
    }
}