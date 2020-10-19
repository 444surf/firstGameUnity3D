using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // THis is the Mouse Sensitivity
    public float mouseSensitivity = 50.0f;

    //This gets the Transform Position of the Player
    public Transform playerBody;

    //This float is for the Rotation around the Y (left to right)
    private float xRotation = 0f;

    //Where to clamp the rotation
    public float clampRotation = 90;

    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        //Removes the Cursor from the Screen
        Cursor.lockState = CursorLockMode.Locked;

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // This gets the Input of X- and Y-Axis of the Mouse multiplied with the Mouse Sensitivity and the Deltatime
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        if (!playerMovement.isGameOver)
        {
            

            //This gets makes the Body of the Player rotate when ever we move the Mouse
            playerBody.Rotate(Vector3.up * mouseX);

            //The Rotation to turn around and clamp the Rotation (DE: clamp = klemmen d.h. zwischen 90 und -90 sperren)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -clampRotation, clampRotation);

            //Returns a rotation that rotates z degrees around the z axis, x degrees around the x axis, and y degrees around the y axis
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
        
        if (playerMovement.isGameOver)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        
    }
}
