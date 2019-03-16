using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    AvatarMovementScript AMS;

    private void Start()
    {
        AMS = GameObject.Find("Avatar").GetComponent<AvatarMovementScript>();
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
