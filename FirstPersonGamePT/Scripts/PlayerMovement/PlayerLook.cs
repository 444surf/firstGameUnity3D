using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //The processed Input
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    //The Input of the Mouse
    private float xMouse = 0.0f;
    private float yMouse = 0.0f;

    private float sensitivityX = 400.0f;
    private float sensitivityY = 400.0f;


    //RotateZ is for the LeaningScript
    private float rotateZ;

    private bool transitioning = false;
 

    //The Gameobject player
    private GameObject player;

    //isLookingAround is used to stop leaning when looking around
    public bool isLookingAround = false;


    //Raycast
    public RaycastHit LookAt
    {
        get
        {
            RaycastHit hit;

            Ray ray = new Ray(transform.position, transform.forward);

            Physics.Raycast(ray, out hit);

            return hit;
        }
    }

    private void MoveCamera()
    {
        //Gets the Input
        yMouse = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivityY;
        xMouse = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivityX;
        //Processes the Input
        xRotation -= yMouse;
        yRotation += xMouse;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        yRotation = Mathf.Clamp(yRotation, -135, 135);

        //Rotates Camera around the x-Axis


        RaycastHit lookAt = LookAt;

        Debug.DrawRay(lookAt.point, lookAt.normal, Color.red);
    }

    private void HeadLook()
    {
        if (InputManager.instance.GetKeyUp("LookAround"))
        {
            transitioning = true;
            isLookingAround = false;
        }

        if (InputManager.instance.GetKey("LookAround"))
        {
            if (InputManager.instance.GetKeyDown("LookAround"))
            {
                yRotation = transform.localRotation.y;
                transitioning = false;
                isLookingAround = true;
            }
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

        }
        else
        {
            if (!transitioning)
            {
                transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            }
            else
            {
                const float speed = 10.0f;

                if (transform.localRotation.eulerAngles.y >= 180.0f)
                {
                    transform.localRotation = Quaternion.Euler(xRotation, -Mathf.Lerp(-transform.localRotation.eulerAngles.y + 360.0f, 0.0f, Time.deltaTime * speed), 0);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(xRotation, Mathf.Lerp(transform.localRotation.eulerAngles.y, 0.0f, Time.deltaTime * speed), 0);
                }
                if (transform.localRotation.eulerAngles.y < 0.01f)
                {
                    transitioning = false;
                    transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, 0.0f, transform.localRotation.eulerAngles.z));
                }
            }
            player.transform.Rotate(player.transform.up * xMouse);
        }
    }

    private void Update()
    {
        MoveCamera();
        HeadLook();
    }


    private void Start()
    {
        player = GameObject.Find("Player");
    }
}
