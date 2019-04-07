using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR MASTERSAVEPOINTS **********

    // Transform du dernier checkpoint touché
    [HideInInspector]
    public Transform avatarCheckpoint;

    // Canvas Save
    public Canvas saveCanvas;

    private void Start()
    {
        // Le premier emplacement de checkpoint est la position de départ de l'avatar
        avatarCheckpoint = GameObject.Find("Avatar").transform;
    }

    // Quand l'avatar touche un checkpoint qui n'est pas déjà le checkpoint actuel
    private void OnTriggerEnter(Collider other)
    {
        // Si l'avatar touche le checkpoint et que ce checkpoint est différent du dernier sauvegardé
        if (other.tag == "Player" && avatarCheckpoint.position != gameObject.GetComponent<Transform>().position)
        {
            // On récupère la position et rotation du checkpoint (ce checkpoint devient le nouveau checkpoint)
            avatarCheckpoint = gameObject.GetComponent<Transform>();

            // On affiche le canvas de sauvegarde
            StartCoroutine("SaveCanvas");
        }
    }

    // Affiche "game saved" pendant 1.5 secondes
    public IEnumerator SaveCanvas()
    {
        // Active le canvas ppendant 90 frames
        saveCanvas.enabled = true;
        for (int i = 0; i < 90; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        // désactive le canvas
        saveCanvas.enabled = false;
        yield return null;
    }
}
