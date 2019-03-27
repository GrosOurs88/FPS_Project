using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    AvatarMovementScript AMS;   // Script AvatarMovementScript (placé sur l'avatar)
    Weapon W;                   // Script AvatarMovementScript (placé sur l'arme actuellement en main)

    Canvas canvasAmmo;

    private void Start()
    {
        // Récupère le script "AvatarMovementScript" sur l'avatar
        AMS = GameObject.Find("Avatar").GetComponent<AvatarMovementScript>();

        canvasAmmo = GameObject.Find("Canvas_Ammo").GetComponent<Canvas>();


        // Désactive le pointeur de la souris
        Cursor.visible = false;
    }


    private void Update()
    {
        ShowAmmunitions();


    }

    public void ShowAmmunitions()
    {
        // Montre le nombre de munitions actuel (Trouve le script "Weapon" dans les enfants de l'avatar et va chercher le nombre de munitions dans le script)


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
