using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float RPS = 10f; //RPS (Rounds per Minute)

    private float bulletDamage = 42;
    private int magazineSize; //Taille du chargeur
    private int magazineAmmo; //nombre de balles restantes dans le chargeur
    private int carriedAmmo; //nombre de balles au total
    private float maxDistanceHitScanShot = 100f;


    //Accuracy Calculator Variables . WIP
    public float accuracy = 0;
    private Vector3 origin;
    private Vector3 direction;
    
    private GameObject bullet;

    //VFX
    public ParticleSystem fire;
    public GameObject impactEffect;
    public GameObject bulletEffect;


    //Camera
    public GameObject cam;

    //Raycasting
    private RaycastHit hit;

    //Damageable
    private Damageable damageable;

    void Start ()
    {
        
    }

    public void HitScanShot ()
    {
        //VFX
        fire.Play();
        //bulletEffect.transform.LookAt(cam.transform.forward);

        //RayCast 
        origin = new Vector3 (cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistanceHitScanShot))
        {
            /*
            hitps.transform.position = hit.point;
            hitps.Play();
            //Kind Of Work
            //hitps.transform.rotation = hit.transform.rotation;
            hitps.transform.Rotate (hit.normal.x, hit.normal.y, hit.normal.z, Space.World);*/

            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

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

    public void PhysicShot ()
    {
        //Instantier une bullet de la position de la caméra dans la direction de la caméra
        //Enlever une balle du chargeur
    }

    public void Reload ()
    {
        //if (carriedAmmo > 0)
        //Recharche le chargeur mdeir
    }
}
