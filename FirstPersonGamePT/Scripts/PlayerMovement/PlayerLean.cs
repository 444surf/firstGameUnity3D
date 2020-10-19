using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLean : MonoBehaviour
{
    //Decides if to use Pro or Normal LeaningOption
    //true is CheckLeaningHold and false is CheckLeaningPress
    public bool LeanOption
    {
        get
        {
            return leanOption;
        }
        set
        {
            leanOption = value;
        }
    }
    private bool leanOption = false;

    [SerializeField] private Transform cameraTransform;

    //Original position to go back to when leaning back
    private Vector3 initPos;
    private Quaternion initRot;

    //The bools to check if player is leaned left or right
    private bool isLeanedLeft = false;
    private bool isLeanedRight = false;

    //The Amount to rotate on the Z-Axis and the Speed
    private float leanRotAmount = 10f;
    private float rotSpeed = 5.0f;

    //The Amount to move on the X-Axis and the Speed
    private float leanTransAmount = 1f;
    private float transSpeed = 5.0f;


    //Used to stop Leaning when LookingAround
    private PlayerLook playerLook;
    public PlayerMovement playerMovement;

    //The Object to move and rotate
    private Quaternion cameraTargetRot = Quaternion.Euler(Vector3.zero);
    private Vector3 cameraTargetTrans = Vector3.zero;

    //The Variables used to rotate and move 
    float rotateZ = 0.0f;
    float translateX = 0.0f;

    //This Bool checks if Leaning is allowed
    private bool allowLeaning = true;

    //There are 2 Types of leaning. You can hold the button or you can press the button once to stay leaned and press it again to unlean

    //This is Type 1, where you have to hold the button to lean
    private void CheckLeaningHold()
    {
        //Sets the LeanAmount to the according to the button pressed, if no button is pressed it goes back to the initial position
        if (InputManager.instance.GetKey("LeanLeft") && allowLeaning)
        {
            SetLean(leanRotAmount, rotSpeed, -leanTransAmount, transSpeed);

        }
        else if (InputManager.instance.GetKey("LeanRight") && allowLeaning)
        {
            SetLean(-leanRotAmount, rotSpeed, leanTransAmount, transSpeed);
        }
        else
        {
            SetLean(initRot.z, rotSpeed, initPos.x, transSpeed);
        }

    }

    //This is Type 2, where you have to press the button once to stay leaned and press it again to unlean
    private void CheckLeaningPress()
    {
        //Asks if the button is pressed and if isLeanedLeft (for leaning left) or isLeanedRight (for leaning Right) is pressed
        //If the button is pressed and the corresponding boolean is false, it sets it to true and the other one to false
        //If the button is pressed and the corresponding boolean is true, it sets it to false and the other on to false
        //To prevent any problems when both are true, the other bool is always set to false

        if (InputManager.instance.GetKeyDown("LeanLeft") && isLeanedLeft == false)
        {
            isLeanedLeft = true;
            isLeanedRight = false;
        }
        else if (InputManager.instance.GetKeyDown("LeanLeft") && isLeanedLeft == true)
        {
            isLeanedLeft = false;
            isLeanedRight = false;
        }

        if (InputManager.instance.GetKeyDown("LeanRight") && isLeanedRight == false)
        {
            isLeanedRight = true;
            isLeanedLeft = false;
        }
        else if (InputManager.instance.GetKeyDown("LeanRight") && isLeanedRight == true)
        {
            isLeanedRight = false;
            isLeanedLeft = false;
        }
        

        //The Lean is set according to the bool
        if (isLeanedLeft == true)
        {
            SetLean(leanRotAmount, rotSpeed, -leanTransAmount, transSpeed);
        }
        else if (isLeanedRight == true)
        {
            SetLean(-leanRotAmount, rotSpeed, leanTransAmount, transSpeed);
        }
        else if (isLeanedLeft == false && isLeanedRight == false)
        {
            SetLean(initRot.z, rotSpeed, initPos.x, transSpeed);
        }
    }

    //The Method used to Lean
    public void SetLean(float rotationValue, float rotationSpeed, float transformValue, float transformSpeed)
    {
        //The to Variables used to rotate and move
        rotateZ = rotationValue;
        translateX = transformValue;

        //This Lerps the cameraTargetRot to the rotation set by the Methods CheckLeanPress and CheckLeanHold()
        Quaternion leanedRot = Quaternion.Euler(cameraTargetRot.eulerAngles.x, cameraTargetRot.eulerAngles.y, rotateZ);
        cameraTargetRot = Quaternion.Lerp(cameraTargetRot, leanedRot, Time.deltaTime * rotationSpeed);


        //This Lerps the cameraTargetTrans to the position set by the Methods CheckLeanPress() and CheckLeanHold()
        cameraTargetTrans = Vector3.Lerp(cameraTargetTrans, new Vector3(translateX, cameraTargetTrans.y, cameraTargetTrans.z), Time.deltaTime * transformSpeed);

        //The localPositon and localRotation are set to the Lean
        transform.localPosition = cameraTargetTrans;
        transform.localRotation = cameraTargetRot;

    }

    private void AllowLeaning()
    {
        if (playerLook.isLookingAround || playerMovement.isSprinting)
        {
            allowLeaning = false;
            isLeanedLeft = false;
            isLeanedRight = false;
            SetLean(initRot.z, rotSpeed, initPos.x, transSpeed);
        }
        else
        {
            allowLeaning = true;
        }
    }

    private void Update()
    {
        AllowLeaning();

        //Decides beetween the 2 LeanOptions
        if (LeanOption && allowLeaning)
        {
            CheckLeaningPress();
        }
        else if (!LeanOption && allowLeaning)
        {
            CheckLeaningHold();
        }
    }

    private void Start()
    {
        //Gets the initial localPosition and localRotatation at the Start
        initPos = cameraTransform.localPosition;
        initRot = cameraTransform.localRotation;

        playerLook = GameObject.Find("FirstPersonCamera").GetComponent<PlayerLook>();
    }
}
