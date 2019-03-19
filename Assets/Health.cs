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

            //Reset (1f); 
            StartCoroutine(FadeOut()); 
            damage = 0;       
        }
        
    }
    // I Don't know why it dosen't work !
    IEnumerator FadeOut ()
    {
        fadeOut = 1;
        fadeOutTime = 0;
        while (fadeOut > 0)
        {
        fadeOutTime += 0.5f * Time.deltaTime;
        fadeOut = Mathf.Lerp (1,0, fadeOutTime);
        damageText.color = new Color(0,0,0,fadeOut);
        print (fadeOutTime);
        yield return damageText.color;
        }

        damageText.text = 0.ToString();
        print ("STOP");
        
        /*
        while (fadeOut >= 0)
        {
            fadeOutTime *= Time.deltaTime;
            fadeOut = Mathf.Lerp (0,255, fadeOutTime);
            damageText.color -= new Color(0,0,0,fadeOut);

            print (fadeOutTime);
            yield return null;

            if (damage != 0)
                yield break;
        }
        fadeOutTime = 0;
        //yield return;*/
    }

}
