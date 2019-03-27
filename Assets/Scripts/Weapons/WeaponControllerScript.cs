using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR L'AVATAR **********

    public Weapon weapon;                                      // Classe "Weapon" des armes
    public GameObject weaponGO;                                // L'arme actuellement en main  

    private float timeFireRate;                                // FireRate de l'arme

    private void Start()
    {
        // Va cherche l'arme actuellement en main
        weapon = GetComponentInChildren(typeof(Weapon)) as Weapon;
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
            // FOV et emplacement de l'arme changent
            StartCoroutine(weapon.GetComponent<Weapon>().Aim(weapon.fovInAimMode, weapon.weaponPositionAfterAim, weapon.timeToSwitchBetweenNormalAndAimMode));
        }

        // Si Clic droit souris relâché
        if (Input.GetMouseButtonUp(1))
        {
            // FOV et emplacement de l'arme changent
            StartCoroutine(weapon.GetComponent<Weapon>().Aim(weapon.fovInNormalMode, weapon.weaponPositionBeforeAim, weapon.timeToSwitchBetweenNormalAndAimMode));
        }
    }   
}
