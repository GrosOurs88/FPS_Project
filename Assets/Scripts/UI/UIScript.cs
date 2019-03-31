using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR MASTERUI **********

    AvatarMovementScript AMS;   // Script AvatarMovementScript (placé sur l'avatar)
    AvatarHealthScript AHS;     // Script AvatarHealthScript (placé sur l'avatar)
    Weapon W;                   // Script Weapon (placé sur l'arme actuellement en main)

    // AMMOS
    Canvas canvasAmmo;          // Le Canvas qui montre les munitions de l'arme
    Text magazineAmmoAmount;    // Le texte qui affiche le nombre de balles restantes dans l'arme
    Text carriedAmmoAmount;     // Le texte qui affiche le nombre de balles en réserve de l'avatar (hors chargeur actuel)

    // HEALTH
    Canvas canvasHealth;        // Le Canvas qui montre l'état de la santé de l'avatar
    Text actualHealth;          // Le texte qui affiche le nombre de point de vie restants de l'avatar
    Text maximumHealth;         // Le texte qui affiche le nombre de point de vie maximum de l'avatar
    Slider healthBar;           // Le Slider de la barre de vie de l'avatar

    private void Start()
    {
        // Récupère le script "AvatarMovementScript" sur l'avatar
        AMS = GameObject.Find("Avatar").GetComponent<AvatarMovementScript>();

        // Récupère le script "AvatarHealthScript" sur l'avatar
        AHS = GameObject.Find("Avatar").GetComponent<AvatarHealthScript>();

        // Récupère le script "Weapon" depuis l'avatar
        W = GameObject.Find("Avatar").GetComponentInChildren<Weapon>();
        
        // ***AMMO***
        // Va chercher le Canvas Ammo
        canvasAmmo = GameObject.Find("Canvas_Ammo").GetComponent<Canvas>();

        // Va chercher le texte qui affiche le nombre de munitions restantes et le nombre de munitions en reserve, depuis le Canvas Health
        magazineAmmoAmount = canvasAmmo.transform.Find("MagazineAmmos").GetComponent<Text>();
        carriedAmmoAmount = canvasAmmo.transform.Find("CarriedAmmos").GetComponent<Text>();
        
        // Le nombre de munitions affiché au départ est le nombre max de munitions de l'arme
        magazineAmmoAmount.text = W.magazineSize.ToString();
        carriedAmmoAmount.text = W.carriedAmmoAtStart.ToString();


        // ***HEALTH***
        // Va chercher le Canvas Health
        canvasHealth = GameObject.Find("Canvas_Health").GetComponent<Canvas>();
        
        // Va chercher le texte qui affiche le nombre de points de vie restants et le nombre de points de vie max, depuis le Canvas Health
        actualHealth = canvasHealth.transform.Find("ActualHealth").GetComponent<Text>();
        maximumHealth = canvasHealth.transform.Find("MaxHealth").GetComponent<Text>();

        // Va chercher le Slider des points de vie et le set au maximum
        healthBar = canvasHealth.transform.Find("HealthBar").GetComponent<Slider>();
        healthBar.value = healthBar.maxValue;

        // Le nombre de points de vie max affiché est celui des points de vie max de l'AvatarHealthScript
        maximumHealth.text = AHS.maxHealth.ToString();

        // Le nombre de points de vie affiché au départ est le nombre de point de vie max
        actualHealth.text = maximumHealth.text;


        // Désactive le pointeur de la souris
        Cursor.visible = false;
    }

    // Met à jour l'UI de la santé actuelle (chiffre + Slider)
    public void ShowHealth()
    {
        // Montre le nombre de munitions actuelles (va chercher le nombre de points de vie restants dans le script "AvatarHealthScript" de l'avatar)
        healthBar.value = (float)AHS.actualHealth / AHS.maxHealth; //(float --> le Slider va de 0 à 1)
        actualHealth.text = AHS.actualHealth.ToString();

        // Change la couleur du slider en fonction des points de vie retsants
        if (healthBar.value >= 0.5f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.green;
        }
        else if (healthBar.value < 0.5f && healthBar.value >= 0.25f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.red;
        }
    }

    // Met à jour l'UI des munitions dans le chargeur actuel
    public void ShowAmmunitions()
    {
        // Montre le nombre de munitions actuelles (va chercher le nombre de munitions restantes dans le script "Weapon" de l'arme)
        magazineAmmoAmount.text = W.magazineAmmo.ToString();       
    }

    // Met à jour l'UI des munitions en reserve (hors chargeur actuel)
    public void ShowReserveAmmunitions()
    {
        // Montre le nombre de munitions en reserve actuelles (va chercher le nombre de munitions restantes dans le script "Weapon" de l'arme)
        carriedAmmoAmount.text = W.carriedAmmo.ToString();
    }





    public void Quit()
    {
        Application.Quit();
    }

    public void Return()
    {
        // désactive canvas pause
        AMS.gamePaused = false;
        AMS.canvasQuit.GetComponent<Canvas>().enabled = false;

        // Désactive le freeze de la pause
        Time.timeScale = 1;

        // Joue le son de pause (pas encore intégré)
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Events/Pause");
    }
}
