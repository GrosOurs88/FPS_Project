using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //private float bulletDamage;
    //private int magazineSize; //Taille du chargeur
    //private int magazineAmmo; //nombre de balles restantes dans le chargeur
    //private int carriedAmmo; //nombre de balles au total
    
    //private GameObject bullet;

    void HitScanShot ()
    {
        //RayCast 
        //if hit peux prendre des dégats
        //transmettre à l'objet le nombre de points de vie correspondant (bulletDamage) // L'objet touché s'occupera de compter les degats etc.. 
    }

    void PhysicShot ()
    {
        //Instantier une bullet de la position de la caméra dans la direction de la caméra
        //Enlever une balle du chargeur
    }

    void Reload ()
    {
        //if (carriedAmmo > 0)
        //Recharche le chargeur mdeir
    }
}
