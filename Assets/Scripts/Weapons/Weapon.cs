using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR CHAQUE ARME **********

    public float RPS = 10f;                               // RPS (Rounds per Minute)

    private float bulletDamage = 42;                      // Dégâts de base de l'arme
    public int magazineSize;                              // Taille du chargeur
    [HideInInspector]
    public int magazineAmmo;                              // nombre de balles restantes dans le chargeur
    [HideInInspector]
    public int carriedAmmo;                               // nombre de balles en réserve
    public int carriedAmmoAtStart;                        // nombre de balles en réserve en début de partie
    public int maxCarriedAmmo;                            // nombre maximal de balles en reserve pouvant être portées par l'avatar
    private float maxDistanceHitScanShot = 1000f;         // Distance max des bullets

    public GameObject bulletEffet;

    //Accuracy Calculator Variables . WIP
    public float accuracy = 1;                            // Précision de base de l'arme            
    private Vector3 origin;
    private Vector3 direction;

    private GameObject bullet;                            // Prefab de balle

    //VFX
    public ParticleSystem fire;
    public GameObject impactEffect;


    public ParticleSystem impactEffectPart;
    private ParticleSystem.Particle[] impacts;
    private int impactCount;
    private int i = 0;

    private ParticleSystem.Particle impact;
    public GameObject bulletEffect;

    private ParticleSystem.Particle impactLast;

    //Aim 
    public float timeToSwitchBetweenNormalAndAimMode;          // Durée de transition de la visée 
    public float fovInAimMode;                                 // FOV de l'arme en mode visé
    public Vector3 weaponPositionAfterAim;                     // Position de l'arme en mode visé
    public float fovInNormalMode;                              // FOV de base de l'arme
    public Vector3 weaponPositionBeforeAim;                    // Position de base de l'arme
    public float multiplierCameraSensibilityWhenAim;           // Multiplicateur de sensibilité de la camera en mode visée

    //Reload
    public float timeToReloadWeapon;                           // Durée de recharge de l'arme
    public Vector3 weaponPositionDuringReload;                 // Position de l'arme pendant rechargement
    [HideInInspector]
    public bool isReloading = false;                           // L'arme est-elle en train d'être rechargée ?

    //Scripts
    UIScript UIS;                                              // Script de l'UI

    //Camera
    public GameObject cam;

    //Raycasting
    private RaycastHit hit;

    //Damageable
    private Damageable damageable;

    void Start()
    {
        impactEffectPart.Play();

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

    public void HitScanShot()
    {
        //VFX
        fire.Play();
        //bulletEffect.transform.LookAt(cam.transform.forward);

        //impacts = impactEffectPart.GetParticles(impactEffectPart);
        impacts = new ParticleSystem.Particle[impactEffectPart.particleCount];

        //RayCast 
        origin = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);

        // Enleve une balle du chargeur
        magazineAmmo -= 1;
        // Met à jour le nombre de balles restantes
        UIS.ShowAmmunitions();

        if (Physics.Raycast(origin, cam.transform.forward, out hit, maxDistanceHitScanShot))
        {
            //Récupère le nombre de particules alive et copie le tableau de particles dans impacts.
            impactCount = impactEffectPart.GetParticles(impacts);

            Quaternion rotation = Quaternion.LookRotation (-hit.normal, Vector3.up);
            Instantiate (impactEffectPart, new Vector3 (hit.point.x + hit.normal.x / 100, hit.point.y + hit.normal.y / 100,hit.point.z + hit.normal.z / 100), rotation);
            impactEffectPart.Play();

            //IMPACT LAST DOESNT WORK 
            /*
            if (impacts[0].remainingLifetime == 10)
                impactLast = impacts[0];
            

            //Prend la plus jeune particule
            for(int i = 0; i < impactCount; i++)
            {   
                if (impactLast.remainingLifetime < impacts[i].remainingLifetime)
                {
                    impactLast = impacts[i];
                }
                //Set la position de la dernière particule
                //impactLast.rotation3D = new Vector3 
                
            }*/

            //Line below is working
            //impacts[0].position = new Vector3 (impacts[0].position.x + 1, impacts[0].position.y, impacts[0].position.z);

            //ROTATE THE EMITTER CAN'T WORK WITH THE POOLING SYSTEM
            //Quaternion rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            //impactEffectPart.transform.rotation = rotation;
            
            //Below Work
            //impacts[i].position = new Vector3(hit.point.x + hit.normal.x / 100, hit.point.y + hit.normal.y / 100, hit.point.z + hit.normal.z / 100);
            //impacts[i].rotation3D = new Vector3(hit.normal.x * 90, hit.normal.y * 90, hit.normal.z * 90);
            
            
            //ANOTHER TRY DON'T WORK
            /*
            float rotationValue = 0;
            if (hit.normal.x != 0)
            {
                impacts[i].axisOfRotation = Vector3.up;
                rotationValue = hit.normal.x;
                impactEffectPart.SetParticles (impacts, 100000,i);
            }
            else if (hit.normal.y != 0)
            {
                impacts[i].axisOfRotation = Vector3.right;
                rotationValue = hit.normal.y;
                impactEffectPart.SetParticles (impacts, 100000,i);
            }
            else //(hit.normal.z != 0)
            {
                impacts[i].axisOfRotation = Vector3.up;
                rotationValue = hit.normal.z;
                impactEffectPart.SetParticles (impacts, 100000,i);
            }
            
            impacts[i].rotation = rotationValue;
            */

/*         print(hit.normal);

            //impactLast.position = new Vector3 (hit.point.x + hit.normal.x, hit.point.y + hit.normal.y, hit.point.z + hit.normal.z);           
            impactEffectPart.SetParticles(impacts, 100000, i);
            print(impacts[i].rotation3D);
            // /!\ Counter jusqu'à l'infini /!\
            i++;

*/


            /*
                        impactEffectPart.Play();
                        //impacts = new ParticleSystem.Particle[impactEffectPart.particleCount];





                        print (impactEffectPart.particleCount);

                        if (impacts[0].remainingLifetime == 3)
                            impactLast = impacts[0];


                        for(int i = 0; i <= impactCount; i++)
                        {   

                            if (impactLast.remainingLifetime < impacts[i].remainingLifetime)
                            {
                                print (impactLast);
                                impactLast = impacts[i];
                            }
                            impactLast.position = hit.point;
                            impactEffectPart.SetParticles (impacts, 1, i);
                        }
                */
            //print (hit.point + " AND " + impacts[0].position);
            /*
            if (impacts[0] != null)
                impact = impacts[0];
            if (impact != null)
            {
                print ("IMPACT");
                impact.position = hit.position;
                
            }*/
            /*
            hitps.transform.position = hit.point;
            hitps.Play();
            //Kind Of Work
            //hitps.transform.rotation = hit.transform.rotation;
            hitps.transform.Rotate (hit.normal.x, hit.normal.y, hit.normal.z, Space.World);*/

            //Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            damageable = hit.transform.GetComponent<Damageable>();
            if (damageable != null)
            {
                //damageable.damageTaken = bulletDamage;
                damageable.Damaged(bulletDamage);
            }
            //if hit peux prendre des dégats
            //transmettre à l'objet le nombre de points de vie correspondant (bulletDamage) // L'objet touché s'occupera de compter les degats etc.. 
        }
    }

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
