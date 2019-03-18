using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADD THIS CLASS TO EVERYTHINGS THAT CAN TAKE DAMAGES

public class Damageable : MonoBehaviour
{
    public float multiplier = 1;
    //public float damageTaken;

    public Health health;
    
    public void Damaged (float damageTaken)
    {
        damageTaken = damageTaken * multiplier;
        print (damageTaken);
        health.damage = damageTaken;

    }

}
