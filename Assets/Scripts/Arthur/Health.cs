using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR LES MANNEQUINS D'ENTRAINEMENT **********

    //***POINTS DE VIE***
    private float health;           // Points de vie actuels
    public float maxHealth;         // Points de vie max

    //***DEGATS***
    public float damage;            // Dégâts prit
    public Text damageText;         // Texte des dégâts prit

    //***UI***

    private float temp;             // Elements de texte a afficher
    private float fadeOut = 255;    // Valeur de fadeOut
    private float fadeOutTime = 0;  // Durée du fadeOut

    
    void Start()
    {
        // Set les points de vie max
        health = maxHealth;
    }

    // Prend des dégâts
    void HealthLoss (float damage)
    {
        // Perd la valeur de dégâts adéquate
        health -= damage;
    }

    // Meurt
    void Die ()
    {
        // Objet détruit
        Destroy(this);
    }

    void Update()
    {
        // Si les dégâts sont différent de zéro 
        if (damage != 0)
        {
            // Prend des dégâts
            HealthLoss(damage);

            // Text Edit
            temp = float.Parse(damageText.text) + damage;
            damageText.text = temp.ToString(); 
            damageText.color = new Color (0,0,0,255);

            // Appelle FadeOut() afin d'effacer le texte. 
            StartCoroutine(FadeOut());      
        }        
    }

    // Efface le texte
    IEnumerator FadeOut ()
    {
        // Reset de variable si la fonction est appelée plusieurs fois.
        fadeOut = 1;
        fadeOutTime = 0;

        // Tant que FadeOut est SUPERIEUR à ZERO
        // L'alpha du text tend vers 0 (transparent)
        while (fadeOut > 0)
        {
            fadeOutTime += 0.5f * Time.deltaTime;
            fadeOut = Mathf.Lerp(1, 0, fadeOutTime);
            damageText.color = new Color(0, 0, 0, fadeOut);

            // Affiche la nouvelle couleur du texte
            if (fadeOut <= 0)
            {
                damageText.text = 0.ToString();
            }
            // Retourne la nouvelle couleur du texte
            yield return damageText.color;
        }
    }
}
