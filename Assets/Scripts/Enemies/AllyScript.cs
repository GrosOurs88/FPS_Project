using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyScript : MonoBehaviour
{
    //********** SCRIPT A PLACER SUR CHAQUE ALLIE **********

    //***SONS***
    public string WalkLow_snd;                            // Son de marche lent
    [HideInInspector]
    public FMOD.Studio.EventInstance _WalkLow_snd;        // Instance du son de marche lent  
    public string WalkFast_snd;                           // Son de marche rapide
    [HideInInspector]
    public FMOD.Studio.EventInstance _WalkFast_snd;       // Instance du son de marche rapide
    public string Spotted_snd;                            // Son de rugissement (spotted)
    [HideInInspector]
    public FMOD.Studio.EventInstance _Spotted_snd;        // Instance du son de rugissement (spotted)

    //***DEPLACEMENT***
    public Transform[] listPositions = new Transform[4];  // Liste de positions à parcourir (pattern)
    public float normalSpeed;                             // vitesse de déplacement lent    
    public float runSpeed;                                // vitesse de déplacement rapide 
    [HideInInspector]
    public Vector3 target;                                // position du point actuel à atteindre
    [HideInInspector]
    public int index;                                     // index du point actuel à atteindre
    public NavMeshAgent navAgent;                         // NavMesh de la scène actuelle

    //***COLLIDERS***
    public Collider colliderLethal;                       // Collider lethal de l'ennemi (contact)
    public Collider colliderVision;                       // Collider de vision de l'ennemi (detection de l'avatar)

    //***DEGATS***
    public int Damage;                                    // Dégâts infligés par l'ennemi

    //***POINTS DE VIE***
    public int maxHealth;                                 // Points de vie max de l'ennemi
    [HideInInspector]
    public int actualHealth;                              // Points de vie actuels de l'ennemi
    

    void Start()
    {
        // Référencement du son de marche low
        _WalkLow_snd = FMODUnity.RuntimeManager.CreateInstance(WalkLow_snd);
        // Plug le son à l'ennemi
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_WalkLow_snd, GetComponent<Transform>(), GetComponent<Rigidbody>());

        // Référencement du son de marche high
        _WalkFast_snd = FMODUnity.RuntimeManager.CreateInstance(WalkFast_snd);
        // Plug le son à l'ennemi
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_WalkFast_snd, GetComponent<Transform>(), GetComponent<Rigidbody>());

        // Référencement du son de spotted by ennemi
        _Spotted_snd = FMODUnity.RuntimeManager.CreateInstance(Spotted_snd);
        // Plug le son à l'ennemi
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_Spotted_snd, GetComponent<Transform>(), GetComponent<Rigidbody>());

        // Le nombre de points de vie de base est au maxium en début de partie
        actualHealth = maxHealth;

        // Set la vitesse du NavAgent de l'ennemi
        navAgent.speed = normalSpeed;

        // set le premier point à atteindre
        index = 0;

        target = listPositions[index].position;
        // Joue le son de marche lent
        _WalkLow_snd.start();
    }

    void Update()
    {
        // Glue les sons à l'ennemi
        _WalkLow_snd.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        _WalkFast_snd.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        _Spotted_snd.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        // l'ennemi continue d'avancer de point en point
        float step = normalSpeed * Time.deltaTime;
        navAgent.SetDestination(target);
        
        // Tant que le point est plus loin que deux unités de distance Unity
        if (Vector3.Distance(transform.position, target) > 2f)
        {
            // L'ennemi regarde vers la prochaine target (point)
            transform.LookAt(target);
        }

        // quand le point est presque atteint
        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            // La vitesse redeviens normale (sécurité)
            navAgent.speed = normalSpeed;

            // Si la position atteinte n'est pas la dernière de la liste
            if (index != listPositions.Length-1)
            {
                // On passe à l'index suivant dans la liste de points
                index += 1;
            }

            // Sinon (la position atteinte est la dernière de la liste)
            else
            {
                // On revient à l'index du premier point
                index = 0;
            }

            // La prochaine target devient celle avec le nouvel index
            target = listPositions[index].position;
        }
    }

    public void TakeDamage(int DamageTaken)
    {
        // Réduit les points de vie de l'ennemi
        actualHealth -= DamageTaken;

        // Si les points de vie atteignent zéro
        if (actualHealth <= 0)
        {
            // L'ennemi meurt
            Die();
        }
    }

    // L'ennemi meurt
    void Die()
    {
        // Meurt
        Destroy(gameObject);
    }
}
