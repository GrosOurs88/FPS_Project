using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovementScript : MonoBehaviour
{
    // VARIABLES GAMEPLAY -------------------------------------
    public float speed;                              // Vitesse de déplacement / rotation avatar
    public float jumpForce;
    private Rigidbody rb;
    private float horizontalMove;
    private float verticalMove;

    // VARIABLES SON ------------------------------------------
    public string WalkLowPlayer_snd;                 // Son de marche lent
    [HideInInspector]
    public FMOD.Studio.EventInstance _WalkLowPlayer_snd;

    // BOOLEENS -----------------------------------------------
    private bool soundWalkingAlreadyPlaying = false; // Le son est-il déjà en train d'être joué ?
    private bool avatarAlreadyJumped = false;        // L'avatar a-t-il déjà sauté ?
    [HideInInspector]
    public bool gamePaused = false;                  // Le jeu est-il en pause ?

    // CANVAS -------------------------------------------------
    public Canvas canvasQuit;                        // Canvas a afficher quand on appuye sur start

    // GAMEOBJECTS --------------------------------------------
    public GameObject mainCamera;

    private void Start()
    {
        // Référencement du son de marche
        _WalkLowPlayer_snd = FMODUnity.RuntimeManager.CreateInstance(WalkLowPlayer_snd);
        // Plug le son à la caméra (l'avatar)
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_WalkLowPlayer_snd, GetComponent<Transform>(), GetComponent<Rigidbody>());
        // Va chercher le Rigidbody du GameObject (avatar)
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //TODO - Sortir les inputs des fonctions. 
        Move();
        Jump();
        Pause();

    }

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
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

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
            // Translations
            horizontalMove = Input.GetAxis("Horizontal");
            verticalMove = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontalMove, 0f, verticalMove);
            rb.AddRelativeForce(movement * speed * Time.deltaTime);

            // Si aucun axe n'est actif (l'avatar n'est pas en mouvement)
            if (Input.GetAxis("Horizontal") < 0.1f && Input.GetAxis("Horizontal") > -0.1f && Input.GetAxis("Vertical") < 0.1f && Input.GetAxis("Vertical") > -0.1f)
            {
                // la velocité de l'avatar est nulle
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                // on arrête le son de marche
                soundWalkingAlreadyPlaying = false;
                _WalkLowPlayer_snd.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            // Si au moins un des deux axes est actif (l'avatar est en mouvement)
            if (Input.GetAxis("Horizontal") > 0.1f || Input.GetAxis("Horizontal") < -0.1f || Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f)
            {
                // Si le son n'est pas déjà en train d'être joué (évite de lancer plusieurs fois le son)
                if (!soundWalkingAlreadyPlaying)
                {
                    // Joue le son de marche
                    _WalkLowPlayer_snd.start();
                    soundWalkingAlreadyPlaying = true;
                }
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
