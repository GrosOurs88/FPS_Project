using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour
{
    // Sensibilité des axes
    public float sensitivityX;
    public float sensitivityY;

    // Angles max et min de l'axe Y (haut/bas)
    public float minimumY = -60F;
    public float maximumY = 60F;

    // Rotation en Y
    float rotationX = 0F;
    float rotationY = 0F;

    // Avatar
    public GameObject avatar;

    // Mouse Locker
    public CursorLockMode cursorMode;


    void Start ()
    {
            Cursor.visible = false;
            Cursor.lockState = cursorMode; //<- here works
    }

    void Update()
    {
        // Rotation en X (autour de l'avatar) --> angle autour de l'axe y (base) + valeur de la souris sur l'axe X (droite/gauche) * sensibilité
        rotationX = avatar.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        // Rotation en Y (haut/bas) --> valeur de la souris sur l'axe Y (haut/bas) * sensibilité
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

        // Clamp la valeur de Y entre les angles min et max
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        // rotation de l'avatar = rotation en X
        avatar.transform.localEulerAngles = new Vector3(0, rotationX, 0);

        // transform de la camera = rotations en Y
        transform.localEulerAngles = new Vector3(-rotationY, 0, 0);

        // la velocité de l'avatar est nulle
        avatar.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }
}
