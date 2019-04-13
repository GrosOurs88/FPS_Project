using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour
{
    private GameObject Avatar;
    public GameObject energyNeeded;                    // GameObject contenant tous les blocs d'energy de la porte
    [HideInInspector]
    public List<Transform> energyBlocs;                // tableau blocs porte
    private int energyBlocsActivated;                  // nombre blocs a activer lors d'une alimentation
    public int energyBalls;                            // Nombre de blocs d'energyball dans la porte
    [HideInInspector]
    public int EnergyBallRemain;                       // Nombre de blocs d'energyball dans la porte restants
    [HideInInspector]
    public int actualEnergyBlockToActivate = 0;        // Block actuel à activer sur la porte
    [HideInInspector]
    public TextMesh textNumber;                        // TextAMount
    public Material materialEnergyBlockActivated;      // Materiau de bloc activé
    public Material materialEnergyBlockDeactivated;    // Materiau de bloc désactivé
    AvatarMovementScript AMS;
    public GameObject textEnergyBallsNeeded;           // Le texte de la porte
    public  float distanceBetweenAvatarAndDoorToLaunchEnergyBalls;
    private bool avatarActuallySendingEnergyToDoor = false;
    private bool activateBlocks = false;
    private GameObject DownDoor;
    private GameObject UpDoor;
    Vector3 DownDoorEnd;
    Vector3 UpDoorEnd;

    void Start()
    {
        // Va chercehr l'avatar
        Avatar = GameObject.Find("Avatar");

        // Va chercher le script AvatarMovementScript dans l'avatar 
        AMS = GameObject.Find("Avatar").GetComponent<AvatarMovementScript>();

        // Va chercher le texte du nombre de blocs à alimenter
        textNumber = GameObject.Find("3DTextNumber").GetComponent<TextMesh>();

        // Le chiffre à affficher est celui rentré comme EnergyBallNeeded
        textNumber.text = energyBalls.ToString();

        // On remplit le tableau avec tous les energyBlocs
        EnergyNeededArrayFilling();

        // Le nomlbre d'energyblock est égal à celui renseigné dans l'inspector (energyBalls)
        EnergyBallRemain = energyBalls;

        DownDoor = GameObject.Find("DownDoor");
        UpDoor = GameObject.Find("UpDoor");

        DownDoorEnd = new Vector3(DownDoor.transform.localPosition.x, DownDoor.transform.localPosition.y - 8.5f, DownDoor.transform.localPosition.z);
        UpDoorEnd = new Vector3(UpDoor.transform.localPosition.x, UpDoor.transform.localPosition.y + 8.5f, UpDoor.transform.localPosition.z);
    }

    private void Update()
    {
        // Si l'avatar est suffisamment proche de la porte
        if (Vector3.Distance(Avatar.transform.position, transform.position) < distanceBetweenAvatarAndDoorToLaunchEnergyBalls && !avatarActuallySendingEnergyToDoor)
        {
            // S'il reste encore des blocks à donner à la porte pour l'ouvrir
            if (EnergyBallRemain > 0)
            {
                // On bloque l'envoie de nouvelles coroutine d'nevoi d'energie à la porte
                avatarActuallySendingEnergyToDoor = true;

                // On envoie des blocs à la porte 
                StartCoroutine("DoorAlimentation");
            }
        }

        if (activateBlocks == true)
        {
            DownDoor.transform.localPosition = Vector3.Lerp(DownDoor.transform.localPosition, DownDoorEnd, 2f * Time.deltaTime);
            UpDoor.transform.localPosition = Vector3.Lerp(UpDoor.transform.localPosition, UpDoorEnd, 2f * Time.deltaTime);

            if(UpDoor.transform.localPosition == UpDoorEnd)
            {
                activateBlocks = false;
            }
        }
    }


    // On active le nombre de cubes voulus pour ouvrir la porte
    public void EnergyNeededArrayFilling()
    {
        //Si le chiffre renseigné est entre 1 et 15
        if (energyBalls > 0 && energyBalls < 16)
        {
            // On ajoute tous les Objets contenus dans le GameObject "energyNeeded" dans un tableau
            foreach (Transform child in energyNeeded.transform)
            {
                energyBlocs.Add(child);
            }

            // On active et on change la couleur du nombre de blocs voulu pour ouvrir la porte
            for (int i = 0; i < energyBalls; i++)
            {
                energyBlocs[i].gameObject.SetActive(true);

                energyBlocs[i].transform.GetComponent<Renderer>().material = materialEnergyBlockDeactivated;
            }
        }

        // Sinon, on affiche un message d'erreur
        else
        {
            Debug.LogError("The Number of needed energball of the door " + transform.name + " is not well settuped !");
        }
    }   

    //// Si un objet rentre dans le trigger de la porte
    //private void OnTriggerEnter(Collider other)
    //{
    //    // Si c'est l'avatar qui entre dans le trigger et qu'il reste encore des blocks à donner à la porte pour l'ouvrir
    //    if (other.tag == "Player" && EnergyBallRemain > 0)
    //    {
    //        // On envoie des blocs à la porte 
    //        StartCoroutine ("DoorAlimentation");
    //    }
    //}

    // On envoie des blocks d'energie à la porte
    public IEnumerator DoorAlimentation()
    {
        // Tant que l'avatar possède des blocs d'energie
        while (AMS.numberOfEnergyBlocs > 0)
        {
            // Si la porte à encore besoin de blocs d'energie pour être ouverte
            if (EnergyBallRemain > 0)
            {
                // GameObject clone = Instantiate(bullet, fireAmmoPosition.position, fireAmmoPosition.rotation); //Old Instantiation
                GameObject clone = Instantiate(Resources.Load("EnergyBlock/EnergyBlock"), Avatar.transform.localPosition, Avatar.transform.localRotation) as GameObject;
                // Le block ne peut pas être ramasser par l'avatar (puisque c'est l'avatar qui l'envoie)
                clone.GetComponent<EnergyBlockScript>().isSupposedToBeCatchByTheAvatar = false;
                // La target du block est cette porte
                clone.GetComponent<EnergyBlockScript>().target = gameObject;

                // On fait descendre de 1 le nombre de blocs nécessaires à l'ouverture de la porte
                EnergyBallRemain--;
            }
            // On attend un certain temps 
            yield return new WaitForSeconds(0.5f);
        }
        // L'avatar n'envoie plus de blocs à la porte
        avatarActuallySendingEnergyToDoor = false;
        yield return null;
    }


    //public void DoorAlimentation()
    //{
    //    // Tant que l'avatar possède des blocs d'energie
    //    for (int i = AMS.numberOfEnergyBlocs; i > 0; i--)
    //    {
    //        // Si la porte à encore besoin de blocs d'energie pour être ouverte
    //        if (EnergyBallRemain > 0)
    //        {
    //            // On lance un bloc à la porte
    //            Invoke("SendAlimentationBlockOnDoor", 0.5f);

    //            // On fait descendre de 1 le nombre de blocs nécessaires à l'ouverture de la porte
    //            EnergyBallRemain--;
    //        }
    //    }
    //}

    //public void SendAlimentationBlockOnDoor()
    //{
    //    print("On créé un energy block");
    //    // GameObject clone = Instantiate(bullet, fireAmmoPosition.position, fireAmmoPosition.rotation); //Old Instantiation
    //    GameObject clone = Instantiate(Resources.Load("EnergyBlock/EnergyBlock"), Avatar.transform.localPosition, Avatar.transform.localRotation) as GameObject;
    //    // Le block ne peut pas être ramasser par l'avatar (puisque c'est l'avatar qui l'envoie)
    //    clone.GetComponent<EnergyBlockScript>().isSupposedToBeCatchByTheAvatar = false;
    //    // La target du block est cette porte
    //    clone.GetComponent<EnergyBlockScript>().target = gameObject;
    //}

    public void ActivateBlocks()
    {
        energyBlocs[actualEnergyBlockToActivate].transform.GetComponent<Renderer>().material = materialEnergyBlockActivated;

        if (actualEnergyBlockToActivate == energyBalls)
        {
            // StartCoroutine("OpenDoor"); OLD
            textEnergyBallsNeeded.SetActive(false);

            activateBlocks = true;
        }
    }

    //public IEnumerator OpenDoor()
    //{
    //    print("B");

    //    textEnergyBallsNeeded.SetActive(false);

    //    GameObject DownDoor = GameObject.Find("DownDoor");
    //    GameObject UpDoor = GameObject.Find("UpDoor");
        
    //    for (int i = 0; i<5; i++)
    //    {
    //        print("C");
    //        DownDoor.transform.Translate(DownDoor.transform.position.x - 0.07f, DownDoor.transform.position.y, DownDoor.transform.position.z);
    //        UpDoor.transform.Translate(UpDoor.transform.position.x - 0.07f, UpDoor.transform.position.y, UpDoor.transform.position.z);
    //    }
    //    yield return new WaitForEndOfFrame();
    //}
}
