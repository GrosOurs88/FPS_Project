using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR CHAQUE ARME **********

    //***WEAPON STATS***
    public float RPS = 10f;                               // RPS (Rounds per Minute)
    public int bulletDamage = 10;                         // Dégâts de base de l'arme

    //***AMMOS STATS***
    private GameObject bullet;                            // Prefab de balle
    public int magazineSize;                              // Taille du chargeur
    [HideInInspector]
    public int magazineAmmo;                              // nombre de balles restantes dans le chargeur
    [HideInInspector]
    public int carriedAmmo;                               // nombre de balles en réserve
    public int carriedAmmoAtStart;                        // nombre de balles en réserve en début de partie
    public int maxCarriedAmmo;                            // nombre maximal de balles en reserve pouvant être portées par l'avatar
    private float maxDistanceHitScanShot = 1000f;         // Distance max des bullets
    public Vector3 origin;                                //Point de départ de la balle lors du tir

    //***VFX***
    public GameObject bulletEffet;                        // Prefab d'effet de balle
    public ParticleSystem fire;                           // Système de particule
    public GameObject impactEffect;                       // Prefab d'effet à l'impact
    public ParticleSystem impactEffectPart;               // ???
    private ParticleSystem.Particle impact;               // ???
    private ParticleSystem.Particle[] impacts;            // Tableau des effets dans la scène 
    private int impactCount;                              // Nombre d'impacts
    public GameObject bulletEffect;                       // ???
    private ParticleSystem.Particle impactLast;           // ???

    //***AIM***
    public float timeToSwitchBetweenNormalAndAimMode;          // Durée de transition de la visée 
    public float fovInAimMode;                                 // FOV de l'arme en mode visé
    public Vector3 weaponPositionAfterAim;                     // Position de l'arme en mode visé
    public float fovInNormalMode;                              // FOV de base de l'arme
    public Vector3 weaponPositionBeforeAim;                    // Position de base de l'arme
    public float multiplierCameraSensibilityWhenAim;           // Multiplicateur de sensibilité de la camera en mode visée

    //***RELOAD***
    public float timeToReloadWeapon;                           // Durée de recharge de l'arme
    public Vector3 weaponPositionDuringReload;                 // Position de l'arme pendant rechargement
    [HideInInspector]
    public bool isReloading = false;                           // L'arme est-elle en train d'être rechargée ?

    //***SCRIPTS***
    private UIScript UIS;                                      // Script de l'UI
    private AvatarHealthScript AHS;                            // Script de vie de l'avatar
    private EnemiScript ES;                                   // Script de vie des ennemis

    //***CAMERA***
    public GameObject cam;

    //***RAYCASTING***
    private RaycastHit hit;


    void Start()
    {
        // FOV de la camera au start (normal mode)
        fovInNormalMode = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView;

        // Position de base de l'arme
        weaponPositionBeforeAim = GetComponent<Transform>().localPosition;

        // Va chercher le script de l'UI dans le "MasterUI"
        UIS = GameObject.Find("MasterUI").GetComponent<UIScript>();

        // Le nombre de balles dans le chargeur est égal au nombre de balles max dans un chargeur (chargeur plein)
        magazineAmmo = magazineSize;

        // Le nombre de balles en réserve est set
        carriedAmmo = carriedAmmoAtStart;
    }

    // Tir
    public void HitScanShot()
    {
        // VFX
        fire.Play();
 
        // Met à jour le tableau des particules dans la scène
        impacts = new ParticleSystem.Particle[impactEffectPart.particleCount];

        // RayCast 
        origin = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);

        // Enleve une balle du chargeur
        magazineAmmo -= 1;

        // Met à jour le nombre de balles restantes
        UIS.ShowAmmunitions();

        if (Physics.Raycast(origin, cam.transform.forward, out hit, maxDistanceHitScanShot))
        {
            // Récupère le nombre de particules alive et copie le tableau de particles dans impacts.
            impactCount = impactEffectPart.GetParticles(impacts);

            // rotation = normale de la balle sur l'objet touché, sur l'axe Y
            Quaternion rotation = Quaternion.LookRotation (-hit.normal, Vector3.up);

            // Instancie un effet d'impact (particule) sur le point touché, dans le sens de la normale
            Instantiate(impactEffectPart, new Vector3(hit.point.x, hit.point.y, hit.point.z), rotation);
            // Instantiate (impactEffectPart, new Vector3 (hit.point.x + hit.normal.x / 100, hit.point.y + hit.normal.y / 100,hit.point.z + hit.normal.z / 100), rotation);

            // Joue le système de particule
            impactEffectPart.Play();            

            // Si l'élément touché contient un script AvatarHealthScript (l'avatar)
            if (hit.transform.GetComponent<AvatarHealthScript>() != null)
            {
                // Inflige les dégâts de l'arme qui a touché la cible à l'avatar
                hit.transform.GetComponent<AvatarHealthScript>().TakeDamage(bulletDamage);
            }
            // Sinon si l'élément touché contient un script EnnemiScript (un ennemi)
            else if (hit.transform.GetComponent<EnemiScript>() != null)
            {
                // Inflige les dégâts de l'arme qui a touché la cible à l'ennemi
                hit.transform.GetComponent<EnemiScript>().TakeDamage(bulletDamage);

                print("enleve " + bulletDamage + "a l'ennemi" + hit.transform.name);
                print("vie restante = " + hit.transform.GetComponent<EnemiScript>().actualHealth);

                // Si l'ennemi n'était pas déjà en train de prendre l'avatar pour cible
                if(hit.transform.GetComponent<EnemiScript>().target != transform.position)
                {
                    // L'ennemi prend l'avatar pour cible
                    hit.transform.GetComponent<EnemiScript>().target = transform.position;
                }
            }
        }
    }

    // Rechargement de l'arme
    public IEnumerator Reload()
    {
        // L'arme est en train d'être rechargée
        isReloading = true;

        // Joue le son de rechargement de l'arme
        FMODUnity.RuntimeManager.PlayOneShot("event:/Weapons/Reload");

        // Le nombre de munitions à remplacer = nombre de balles d'un chargeur - nombre de balles restantes dans le chargeur actuel
        int numberOfBulletsToExchange = magazineSize - magazineAmmo;

        // Position de recharge de l'arme
        GetComponent<Transform>().localPosition = weaponPositionDuringReload;
        
        // Attend le temps du rechargement de l'arme
        yield return new WaitForSeconds(timeToReloadWeapon);

        // Position de base de l'arme
        GetComponent<Transform>().localPosition = weaponPositionBeforeAim;

        // Si l'avatar a suffisamment de balles en reserve pour remplir un chargeur entier
        if (carriedAmmo >= (numberOfBulletsToExchange))
        {
            // on ajoute les balles au chargeur et on les enleve de la reserve
            magazineAmmo += numberOfBulletsToExchange;
            carriedAmmo -= numberOfBulletsToExchange;
        }
        // Sinon, on met le reste des balles en reserve dans le chargeur
        else
        {
            magazineAmmo += carriedAmmo;
            carriedAmmo -= carriedAmmo;
        }

        // Met à jour le nombre de balles restantes dans l'UI
        UIS.ShowAmmunitions();

        // Met à jour le nombre de balles en réserve dans l'UI
        UIS.ShowReserveAmmunitions();

        // L'arme n'est plus en train d'être rechargée
        isReloading = false;
    }

    // Zoom 
    // Zoom In = Bouge la position de l'arme devant la camera et baisse le FOV pour faire un zoom
    // Zoom Out = Replace l'arme à sa position initiale et reset le FOV initial
    public IEnumerator Aim(AimParameters param)
    {
        // Position actuelle de l'arme
        Vector3 currentPos = GetComponent<Transform>().localPosition;

        // FOV actuel de l'arme
        float actualFOV = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView;

        // Set une timeline
        float t = 0f;
        while (t < 1)
        {
            // Bouge dans la timeline entre 0 et la valeur totale de l'action (timeToSwitchBetweenNormalAndAimMode)
            t += Time.deltaTime / timeToSwitchBetweenNormalAndAimMode;

            // Zoom = Lerp le FOV entre la valeur actuelle et la valeur à atteindre
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView = Mathf.Lerp(actualFOV, param.FOVToReach, t);

            // Arme se déplace entre sa position actuelle et la position à atteindre
            GetComponent<Transform>().localPosition = Vector3.Lerp(currentPos, param.positionToReach, t);

            yield return null;
        }
    }

    // Struct consacrée au Zoom (deux paramètres : un FOV et un Vector3)
    public struct AimParameters
    {
        public float FOVToReach;
        public Vector3 positionToReach;

        public AimParameters (float _FOVToReach, Vector3 _positionToreach)
        {
            FOVToReach = _FOVToReach;
            positionToReach = _positionToreach;
        }
    }
}
