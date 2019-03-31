using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarHealthScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR L'AVATAR **********

    public int maxHealth;
    [HideInInspector]
    public int actualHealth;

    private void Start()
    {
        // Le nombre de points de vie de base est au maxium en début de partie
        actualHealth = maxHealth;
    }

    void Die()
    {
        // Quand points de vie =< 0
        // Meurt
        // Respawn
        // Reset les points de vie
        // Reset munitions
    }


}
