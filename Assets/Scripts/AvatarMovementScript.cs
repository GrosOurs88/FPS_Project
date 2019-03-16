using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovementScript : MonoBehaviour
{
    // VARIABLES GAMEPLAY -------------------------------------
    // Vitesse de déplacement / rotation avatar
    public float speed;
    public float rotateSpeedController;
    public float rotateSpeedMouse;
    public float jumpForce;

    // VARIABLES SON ------------------------------------------
    // Son de marche lent
    public string WalkLowPlayer_snd;
    [HideInInspector]
    public FMOD.Studio.EventInstance _WalkLowPlayer_snd;

    // Le son est-il déjà en train d'être joué ?
    private bool soundWalkingAlreadyPlaying = false;

    // Le jeu est-il en pause ?
    [HideInInspector]
    public bool gamePaused = false;

    // L'avatar a-t-il déjà sauté ?
    private bool avatarAlreadyJumped = false;

    // Canvas a afficher quand on appuye sur start
    public Canvas canvasQuit;
        
    private void Start()
    {
        // Référencement du son de marche
        _WalkLowPlayer_snd = FMODUnity.RuntimeManager.CreateInstance(WalkLowPlayer_snd);
        // Plug le son à la caméra (l'avatar)
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_WalkLowPlayer_snd, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    void Update()
    {
        //TODO - Sortir les inputs des fonctions. 
        Move ();
        Jump ();
        Pause ();
           

           // A mettre dans le script WeaponController

            // Si on appuye sur l'input de tir
            if (Input.GetButtonDown("Fire"))
            {
                // *** Code pour tirer une bullet au centre de l'écran, quel que soit la distance à laquelle se situe la cible ***
            
                // 1 - Lancer un Raycast qui part du centre de la caméra, droit devant
                // 2 - Récupérer le point d'impact du raycast (cible)
                // 3 - Faire partir une balle depuis le canon de l'arme, jusqu'à la cible

                // *** Fin du code ***

                // On joue le son de tir de pistolet
                FMODUnity.RuntimeManager.PlayOneShot("event:/Weapons/GunShot");
            }
        }     
        //FIN DE WEAPON CONTROLLER  

       

        

    void Pause ()
    {
        // Quand le jeu est en pause
        if (gamePaused)
        {
            // si appuye sur bouton action (jump)
            if (Input.GetButtonDown("Jump"))
            {
                Application.Quit();
            }

            // si appuye sur le bouton retour
            if (Input.GetButtonDown("Return"))
            {
                // désactive canvas pause
                gamePaused = false;
                canvasQuit.GetComponent<Canvas>().enabled = false;

                // Désactive le freeze de la pause
                Time.timeScale = 1;

                // Joue le son de pause (pas encore intégré)
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Events/Pause");
            }
        }

        // Quand on appuye sur start et que jeu n'est pas encore en pause
        if (Input.GetButtonDown("Start") && !gamePaused)
        {
            // Met le jeu en pause (freeze)
            Time.timeScale = 0;

            // active canvas pause
            gamePaused = true;
            canvasQuit.GetComponent<Canvas>().enabled = true;

            // Joue le son de pause (pas encore intégré)
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Event/Pause");
        }

        // Glue le son de marche au joueur (pour que le transform du son reste celui de l'avatar)
        _WalkLowPlayer_snd.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
    }


    void Jump ()
    {
        // Quand input de saut et que l'avatar n'est pas déjà en train de sauter
        if (Input.GetButtonDown(("Jump")) && !avatarAlreadyJumped)
        {
            // Impulsion en Y            
            transform.Translate(Vector3.up * jumpForce * Time.deltaTime, Space.World);

            // On joue le son de jump (pas encore intégré)
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Avatar/Jump");

            // On interdit à l'avatar de sauter de nouveau
            avatarAlreadyJumped = true;
        }      

    }

    void Move ()
    {
        // Si le jeu n'est pas en pause
        if (!gamePaused)
        {
            // Translations - clavier + manette (Left stick)
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
            }
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self);
            }
            if (Input.GetAxis("Vertical") > 0.1f)
            {
                gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
            }
            if (Input.GetAxis("Vertical") < -0.1f)
            {
                gameObject.transform.Translate(Vector3.back * speed * Time.deltaTime, Space.Self);
            }
            // Si le stick gauche n'est pas incliné
            if (Input.GetAxis("Horizontal") < 0.1f && Input.GetAxis("Horizontal") > -0.1f && Input.GetAxis("Vertical") < 0.1f && Input.GetAxis("Vertical") > -0.1f)
            {
                // la velocité de l'avatar est nulle
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                // on arrête le son de marche
                soundWalkingAlreadyPlaying = false;
                _WalkLowPlayer_snd.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            // Rotations - manette (Right stick) + souris
            if (Input.GetAxis("HorizontalD") > 0.1f)
            {
                gameObject.transform.Rotate(Vector3.up * Input.GetAxis("HorizontalD") * rotateSpeedController * Time.deltaTime);
            }
            else if ( Input.GetAxis("MouseX") > 0.1f)
            {
                gameObject.transform.Rotate(Vector3.up * Input.GetAxis("MouseX") * rotateSpeedMouse * Time.deltaTime);
            }

            if (Input.GetAxis("HorizontalD") < -0.1f)
            {
                gameObject.transform.Rotate(Vector3.down * -Input.GetAxis("HorizontalD") * rotateSpeedController * Time.deltaTime);
            }
            else if (Input.GetAxis("MouseX") < -0.1f)
            {
                gameObject.transform.Rotate(Vector3.down * Input.GetAxis("MouseX") * rotateSpeedMouse * Time.deltaTime);
            }

            if (Input.GetAxis("VerticalD") > 0.1f)
            {
                gameObject.transform.Rotate(Vector3.right * Input.GetAxis("VerticalD") * rotateSpeedController * Time.deltaTime);
            }
            else if (Input.GetAxis("MouseY") > 0.1f)
            {
                gameObject.transform.Rotate(Vector3.right * Input.GetAxis("MouseY") * rotateSpeedMouse * Time.deltaTime);
            }

            if (Input.GetAxis("VerticalD") < -0.1f)
            {
                gameObject.transform.Rotate(Vector3.left * -Input.GetAxis("VerticalD") * rotateSpeedController * Time.deltaTime);
            }
            else if (Input.GetAxis("MouseY") < -0.1f)
            {
                gameObject.transform.Rotate(Vector3.left * Input.GetAxis("MouseY") * rotateSpeedMouse * Time.deltaTime);
            }

            // Si au moins un des deux axes du stick gauche est incliné (avatar en mouvement)
            if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f || Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
            {
                // Si le son n'est pas déjà en train d'être joué (évite de lancer 50000 fois le son)
                if (!soundWalkingAlreadyPlaying)
                {
                    // Joue le son de marche
                    _WalkLowPlayer_snd.start();
                    soundWalkingAlreadyPlaying = true;
                }
            }

            // Si la souris / stick droit n'est pas incliné
            if (Input.GetAxis("HorizontalD") < 0.1f && Input.GetAxis("HorizontalD") > -0.1f && Input.GetAxis("VerticalD") < 0.1f && Input.GetAxis("VerticalD") > -0.1f)
            {
                // la velocité angulaire de l'avatar est nulle
                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }

    }




    
    private void OnCollisionEnter(Collision collision)
    {
        // Quand on touche un sol
        if(collision.transform.tag == "Floor")
        {
            // On reset le fait de pouvoir sauter
            avatarAlreadyJumped = false;
        }
    }

}
