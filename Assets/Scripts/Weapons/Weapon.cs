using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float bulletDamage = 42;
    private int magazineSize; //Taille du chargeur
    private int magazineAmmo; //nombre de balles restantes dans le chargeur
    private int carriedAmmo; //nombre de balles au total
    private float maxDistanceHitScanShot = 100f;
    
    private GameObject bullet;

    //Camera
    public GameObject cam;

    //Raycasting
    private RaycastHit hit;

    //Damageable
    private Damageable damageable;

    void Start ()
    {
        //cam = gameObject.GetComponentInChildren(typeof(Camera)) as Camera; The camera is parent of this object
    }

    public void HitScanShot ()
    {
        //RayCast 
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistanceHitScanShot))
        {
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
