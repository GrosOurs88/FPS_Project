using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR CHAQUE ARME **********

    public float RPS = 10f;                               // RPS (Rounds per Minute)

    private float bulletDamage = 42;                      // Dégâts de base de l'arme
    private int magazineSize;                             // Taille du chargeur
    private int magazineAmmo;                             // nombre de balles restantes dans le chargeur
    private int carriedAmmo;                              // nombre de balles au total
    private float maxDistanceHitScanShot = 1000f;         // Distance max des bullets

    //Accuracy Calculator Variables . WIP
    public float accuracy = 0;                            // Précision de base de l'arme            
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
    [HideInInspector]
    public float fovInNormalMode;                              // FOV de base de l'arme
    [HideInInspector]
    public Vector3 weaponPositionBeforeAim;                    // Position de base de l'arme

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


        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistanceHitScanShot))
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

    public void PhysicShot()
    {
        //Instantier une bullet de la position de la caméra dans la direction de la caméra
        //Enlever une balle du chargeur
    }

    public void Reload()
    {
        //if (carriedAmmo > 0)
        //Recharche le chargeur mdeir
    }

    // Bouge la position de l'arme lors de la visée et baisse le FOV pour zoomer sur la cible
    public IEnumerator Aim(float _fovToReach, Vector3 _positionToReach, float _timeToMove)
    {
        Vector3 currentPos = GetComponent<Transform>().localPosition;

        float actualFOV = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView;

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / _timeToMove;

            // Zoom
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView = Mathf.Lerp(actualFOV, _fovToReach, t);

            // Position arme
            GetComponent<Transform>().localPosition = Vector3.Lerp(currentPos, _positionToReach, t);

            yield return null;
        }
    }
}
