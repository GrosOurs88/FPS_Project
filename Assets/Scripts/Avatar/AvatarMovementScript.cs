using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovementScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR L'AVATAR **********

    // VARIABLES GAMEPLAY -------------------------------------
    public float speed;                                   // Vitesse de déplacement de l'avatar
    public float speedMultiplier;                         // Multiplicateur de vitesse quand l'avatar court
    public float jumpForce;
    private Rigidbody rb;
    private float horizontalMove;
    private float verticalMove;
    private float distToGround;
    [HideInInspector]
    public int numberOfEnergyBlocs;                      // Nombre d'energy blocs détenus par l'avatar

    // VARIABLES SON ------------------------------------------
    public string WalkLowPlayer_snd;                      // Son de marche (string à renseigner)
    [HideInInspector]
    public FMOD.Studio.EventInstance _WalkLowPlayer_snd;  // Création de l'instance du son de marche

    // BOOLEENS -----------------------------------------------
    private bool soundWalkingAlreadyPlaying = false;      // Le son est-il déjà en train d'être joué ?
    private bool avatarAlreadyJumped = false;             // L'avatar a-t-il déjà sauté ?
    private bool avatarIsRunning = false;                 // L'avatar est-il en train de courrir ?
    [HideInInspector]
    public bool gamePaused = false;                       // Le jeu est-il en pause ?

    // CANVAS -------------------------------------------------
    public Canvas canvasQuit;                             // Canvas a afficher quand on appuye sur start

    // GAMEOBJECTS --------------------------------------------
    public GameObject mainCamera;                         // Main Camera (Avatar)

    void Start()
    {
        // Va chercher le Rigidbody du GameObject (avatar)        
        rb = GetComponent<Rigidbody>();
        
        // Référencement du son de marche
        _WalkLowPlayer_snd = FMODUnity.RuntimeManager.CreateInstance(WalkLowPlayer_snd);
        // Plug le son à la caméra (l'avatar)
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_WalkLowPlayer_snd, GetComponent<Transform>(), GetComponent<Rigidbody>());
        
        // Taille égale à la moitié de la taille du collider de l'avatar (.extents.y)
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {
        // Déplacement
        Move();

        // Saut
        Jump();

        // Accroupir
        Crouch();

        // Pause
        // Pause();
    }

    void Jump ()
    {
        // Si on touche le sol
        if (IsGrounded())
        {
            // On reset le fait de pouvoir sauter
            avatarAlreadyJumped = false;
        }

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

            transform.Translate(new Vector3((Input.GetAxis("Horizontal") * speed * Time.deltaTime), 0, Input.GetAxis("Vertical") * speed * Time.deltaTime));

            // Si aucun axe n'est actif (l'avatar n'est pas en mouvement) est que l'avatar est au sol
            if (Input.GetAxis("Horizontal") < 0.1f && Input.GetAxis("Horizontal") > -0.1f && Input.GetAxis("Vertical") < 0.1f && Input.GetAxis("Vertical") > -0.1f && IsGrounded())
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

            // Si on appuye sur la touche Maj en étant au sol 
            if (Input.GetKey(KeyCode.LeftShift) && IsGrounded() && !avatarIsRunning) 
            {
                avatarIsRunning = true;
                // On multiplie la valeur de déplacement
                speed = speed * speedMultiplier;
            }

            // Sinon si on relâche la touche Maj
            else if (Input.GetKeyUp(KeyCode.LeftShift) && avatarIsRunning)
            {
                avatarIsRunning = false;
                // On reprend une vitesse de marche normale
                speed = speed / speedMultiplier;
            }

            // Glue le son de marche au joueur (pour que le transform du son reste celui de l'avatar)
            _WalkLowPlayer_snd.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        }
    }

    // Accroupissement
    public void Crouch()
    {
        // Si on appuye sur Ctrl Left
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            // On modifie les propriétés du collider pour pouvoir s'abaisser
            GetComponent<CapsuleCollider>().height = 1f;
            GetComponent<CapsuleCollider>().center = new Vector3 (0f, 0.4f, 0f);
            // On se baisse (de la moitié de la hauteur de l'avatar)
            transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - GetComponent<Collider>().bounds.size.y, transform.localPosition.z);
        }

        // Sinon si on relâche Ctrl Left
        else if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            // On se redresse
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + GetComponent<Collider>().bounds.size.y, transform.localPosition.z);
            // On rétablit les propriétés de base du collider
            GetComponent<CapsuleCollider>().height = 2f;
            GetComponent<CapsuleCollider>().center = new Vector3(0f, 0f, 0f);
        }
    }

    // Vérifie si l'avatar est au sol (sur n'importe quel élément) ou pas
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    //void Pause ()
    //{
    //    // Quand le jeu est en pause
    //    if (gamePaused)
    //    {
    //        // si appuye sur bouton action (jump)
    //        if (Input.GetButtonDown("Jump"))
    //        {
    //            Application.Quit();
    //        }

    //        // si appuye sur le bouton retour
    //        if (Input.GetButtonDown("Return"))
    //        {
    //            // désactive canvas pause
    //            gamePaused = false;
    //            canvasQuit.GetComponent<Canvas>().enabled = false;

    //            // Désactive le freeze de la pause
    //            Time.timeScale = 1;

    //            // Joue le son de pause (pas encore intégré)
    //            //FMODUnity.RuntimeManager.PlayOneShot("event:/Events/Pause");
    //        }
    //    }

    //    // Quand on appuye sur start et que jeu n'est pas encore en pause
    //    if (Input.GetButtonDown("Start") && !gamePaused)
    //    {
    //        // Met le jeu en pause (freeze)
    //        Time.timeScale = 0;

    //        // active canvas pause
    //        gamePaused = true;
    //        canvasQuit.GetComponent<Canvas>().enabled = true;

    //        // Joue le son de pause (pas encore intégré)
    //        //FMODUnity.RuntimeManager.PlayOneShot("event:/Event/Pause");
    //    }

    //    
    //}
}
