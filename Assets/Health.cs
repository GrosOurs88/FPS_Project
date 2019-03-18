using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private float health;
    private float maxHealth;

    public float damage;

    public Text damageText;

    
    void Start()
    {
        health = maxHealth;
    }


    void HealthLoss (float damage)
    {
        health -= damage;
    }

    void Die ()
    {
        Destroy(this);
    }

    void Update()
    {
        if (damage != 0)
        {
            HealthLoss (damage);
            damageText.text = damage.ToString();
            damage = 0;
        }
        
    }
}
