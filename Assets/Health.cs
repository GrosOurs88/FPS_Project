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
    private float fadeOut = 0;
    private float fadeOutTime = 0.1f;

    
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
        while (fadeOut <= 25.5f)
        {
        fadeOut = Mathf.Lerp (0,255, fadeOutTime);
        damageText.color -= new Color(0,0,0,fadeOut);
        print (fadeOut);
        yield return null;
        }
        fadeOut = 0;
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

    private void Reset (float resetTime) 
    {
        float t = resetTime;
        while (t<=0)
        {
            t += Time.deltaTime;
        }
        damage = 0;
    }
}
