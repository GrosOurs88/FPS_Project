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


    private float temp;
    private float fadeOut = 255;
    private float fadeOutTime = 0;

    
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

            //Text Edit
            temp = float.Parse(damageText.text) + damage;
            damageText.text = temp.ToString(); 
            damageText.color = new Color (0,0,0,255);

            //Appelle FadeOut() afin d'effacer le texte. 
            StartCoroutine(FadeOut()); 
            damage = 0;       
        }
        
    }

    IEnumerator FadeOut ()
    {
        // Reset de variable si la fonction est appelée plusieurs fois.
        fadeOut = 1;
        fadeOutTime = 0;
        

        //Tant que FadeOut est SUPERIEUR à ZERO
        //L'alpha du text tend vers 0 (transparent)
        while (fadeOut > 0)
        {
        fadeOutTime += 0.5f * Time.deltaTime;
        fadeOut = Mathf.Lerp (1,0, fadeOutTime);
        damageText.color = new Color(0,0,0,fadeOut);
        //Affiche la nouvelle couleur du texte
        if (fadeOut <= 0)
            damageText.text = 0.ToString();
        yield return damageText.color;
        }


    }

}
