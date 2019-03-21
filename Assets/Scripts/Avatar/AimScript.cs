using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    public float timeToSwitchBetweenNormalAndAimMode;
    private float fovInNormalMode;
    public float fovInAimMode;
    private Vector3 weaponPositionBeforeAim;
    public Vector3 weaponPositionAfterAim;
    float t;                                              // Durée de transition de la visée 

    private void Start()
    {
        // FOV de la camera au start (normal mode)
        fovInNormalMode = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView;

        // Position de base de l'arme
        weaponPositionBeforeAim = GameObject.Find("WeaponHandler").GetComponent<Transform>().localPosition;
    }

    void Update()
    {
        // Si Clic droit souris enfoncé
        if (Input.GetMouseButtonDown(1))
        {
            // FOV change
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView = Mathf.Lerp(fovInNormalMode, fovInAimMode, timeToSwitchBetweenNormalAndAimMode);

            StartCoroutine(MoveFOV(fovInNormalMode, fovInAimMode, timeToSwitchBetweenNormalAndAimMode));

           // Emplacement arme change
           StartCoroutine(MoveWeaponToPosition(GameObject.Find("WeaponHandler").GetComponent<Transform>(), weaponPositionAfterAim, timeToSwitchBetweenNormalAndAimMode));
        }

        // Si Clic droit souris relâché
        if (Input.GetMouseButtonUp(1))
        {
            // FOV redeviens normal
            // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView = Mathf.Lerp(fovInAimMode, fovInNormalMode, timeToSwitchBetweenNormalAndAimMode);
            StartCoroutine(MoveFOV(fovInAimMode, fovInNormalMode, timeToSwitchBetweenNormalAndAimMode));

            // Emplacement arme revient à sa position normale
            StartCoroutine(MoveWeaponToPosition(GameObject.Find("WeaponHandler").GetComponent<Transform>(), weaponPositionBeforeAim, timeToSwitchBetweenNormalAndAimMode));
        }
    }

    public IEnumerator MoveFOV(float fovActual, float fovToReach, float timeToMove)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().fieldOfView = Mathf.Lerp(fovActual, fovToReach, t);
            yield return null;
        }
    }

    // Bouge la position de l'arme lors de la visée
    public IEnumerator MoveWeaponToPosition(Transform weaponTransform, Vector3 positionToReach, float timeToMove)
    {
        var currentPos = weaponTransform.localPosition;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            weaponTransform.localPosition = Vector3.Lerp(currentPos, positionToReach, t);
            yield return null;
        }
    }
}
