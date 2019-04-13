using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBlockScript : MonoBehaviour
{
    private GameObject Avatar;
    [HideInInspector]
    public GameObject target;                            // La porte vers laquelle va se diriger le block pour lui donner de l'energie

    UIScript UIS;

    [HideInInspector]
    public bool isSupposedToBeCatchByTheAvatar = true;   // L'avatar peut-il ramasser l'objet ?

    public float speedOfBlockMovingFromAvatarToDoor;     // Vitesse de déplacement des blocs d'energie de l'avatar vers la porte

    public ParticleSystem particle;

    void Start()
    {
        // Va chercher le script UIScript dans le MasterUI
        UIS = GameObject.Find("MasterUI").GetComponent<UIScript>();

        // Va chercher l'avatar
        Avatar = GameObject.Find("Avatar");
    }

    void Update()
    {
        // Si une target est assigné au block
        if (target != null)
        {
            // Le block se déplace vers la target actuelle
            float step = speedOfBlockMovingFromAvatarToDoor * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si le block touche la porte et que l'avatar n'est pas supposé le ramasser (va de l'avatar vers la porte)
        if (other.name == "Door" && !isSupposedToBeCatchByTheAvatar)
        {
            // On incrémente la valeur de l'index du cube à activer
            target.GetComponent<DoorScript>().actualEnergyBlockToActivate += 1;
            // On decrémente le nombre de cubes restant pour ouvrir la porte
            target.GetComponent<DoorScript>().EnergyBallRemain-- ;
            // On met à jour la valeur de la porte des blocs restant pour ouvrir la porte
            target.GetComponent<DoorScript>().textNumber.text = target.GetComponent<DoorScript>().EnergyBallRemain.ToString();
            // On décrémente le nombre de blocks que possède l'avatar 
            Avatar.GetComponent<AvatarMovementScript>().numberOfEnergyBlocs-- ;
            // On lance la fonction de la doorScript
            target.GetComponent<DoorScript>().ActivateBlocks();
            // On détruit le système de particule de ce block
            Destroy(particle);
            // On détruit ce block
            Destroy(gameObject);
        }

        // Si le block touche l'avatar et que l'avatar est supposé le ramasser (va d'une target vers l'avatar)
        if (other.name == "Avatar" && isSupposedToBeCatchByTheAvatar)
        {
            // On incrémente le nombre de blocks que possède l'avatar 
            Avatar.GetComponent<AvatarMovementScript>().numberOfEnergyBlocs++;
            // On met à jour le score
            UIS.Score();
            // On détruit ce block
            Destroy(gameObject);
        }
    }
}
