using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    //The Components for Movement
    public Rigidbody playerRig;
    public CapsuleCollider playerCollider;

    public float Gravity
    {
        get
        {
            return gravity;
        }
        set
        {
            gravity = value;
        }
    }

    public float Drag
    {
        get
        {
            return drag;
        }
        set
        {
            if (value != 0)
            {
                drag = value;
            }
            else
            {
                drag = float.MinValue;
            }
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public float JumpHeight
    {
        get
        {
            return jumpHeight;
        }
        set
        {
            jumpHeight = value;
        }
    }

    public bool OnGround
    {
        get
        {
            return Physics.CheckSphere(transform.position - (transform.up * (playerCollider.height / 2 - 0.2f)), 0.25f);
        }
    }

    public float Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    private float gravity = -0.081f;
    private float drag = 10.0f;

    private float speed = 1.5f;

    private float walkSpeed = 1f;
    private float sprintSpeedMultiplier = 1.6f;
    private float sprintFOV = 10.0f;

    private float jumpHeight = 360.0f;

    private float velocity = 0.0f;

    private float maxSpeed = 10.0f;

    private float maxSlope = 30.0f;

    Vector3 vel = Vector3.zero;

    public bool isSprinting = false;

    private bool sprintOption = true;

    private float cameraFOV;
    public float cameraFOVMultiplier = 1.2f;
    private bool isFOVTransitioning = false;
    private float fovTarget;

    private void MovePlayer()
    {
        //Gets the Inputs from WASD
        if (checkAngle(0.7f, transform.forward))
        {
            vel += transform.forward * (InputManager.instance.GetKey("Forward") ? 1 : 0);
        }
        if (checkAngle(0.7f, -transform.forward))
        {
            vel += -transform.forward * (InputManager.instance.GetKey("Back") ? 1 : 0);
        }
        if (checkAngle(0.7f, transform.right))
        {
            vel += transform.right * (InputManager.instance.GetKey("Right") ? 1 : 0);
        }
        if (checkAngle(0.7f, -transform.right))
        {
            vel += -transform.right * (InputManager.instance.GetKey("Left") ? 1 : 0);
        }
    }

    //WIP
    public void FixedJump()
    {
        if (OnGround)
        {
            if (InputManager.instance.GetKey("Jump"))
            {
                playerRig.AddForce(transform.up * Time.deltaTime * jumpHeight, ForceMode.VelocityChange);
            }
        }
    } 

    private void FixedMove()
    {
        //The Position to move to
        Vector3 movePos = vel * speed * Time.fixedDeltaTime + playerRig.position;

        //Debug.Log(checkAngle(0.7f));

        //Checks if you allowed to move by checking the Angle you are walking towards and ???
        if (!Physics.CheckSphere(movePos, 0.1f))// && checkAngle(0.7f, vel))
        {
            playerRig.MovePosition(movePos);
        }

        //???
        vel = Vector3.zero;
    }

    //WIP
    //Checks the Angle with a Raycast
    private bool checkAngle(float max, Vector3 direction)
    {
        RaycastHit hit1;
        RaycastHit hit2;

        Ray ray = new Ray(playerRig.position + (-Vector3.up * playerCollider.height * 0.5f), direction);

        bool hitSuccess1 = Physics.Raycast(ray, out hit1, 0.55f);

        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        ray.origin += Vector3.up * 0.01f;

        bool hitSuccess2 = Physics.Raycast(ray, out hit2);

        Debug.DrawRay(ray.origin, ray.direction, Color.grey);

        float dot = Vector3.Dot(ray.direction.normalized, (hit2.point - hit1.point).normalized);

        if (Mathf.Abs(dot) > max || !hitSuccess1 || !hitSuccess2)
        {
            Debug.DrawRay(hit1.point, hit1.normal, Color.green);
            return true;
        }

        //p < 0.7
        Debug.DrawRay(hit1.point, hit1.normal, Color.red);
        return false;
    }

    private void CheckSprintHold()
    {
        

        if (InputManager.instance.GetKeyDown("Sprint") && InputManager.instance.GetKey("Forward"))
        {
            SetSprintOn();
            isSprinting = true;
        }

        if (InputManager.instance.GetKeyUp("Sprint"))
        {
            SetSprintOff();
            isSprinting = false;
        }
        if (InputManager.instance.GetKeyUp("Forward") && isSprinting)
        {
            SetSprintOff();
            isSprinting = false;
        }
        ChangeFOV();
    }

    private void CheckSprintPress()
    {
        if (InputManager.instance.GetKeyDown("Sprint") && !isSprinting && InputManager.instance.GetKey("Forward"))
        {
            
            SetSprintOn();
            isSprinting = true;
        }else if (InputManager.instance.GetKeyDown("Sprint") && isSprinting)
        {
            SetSprintOff();
            isSprinting = false;
        }
        if (InputManager.instance.GetKeyUp("Forward") && isSprinting)
        {
            SetSprintOff();
            isSprinting = false;
        }
        ChangeFOV();
    }

    
    private void ChangeFOV()
    {
      
        if (isFOVTransitioning)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fovTarget, Time.deltaTime * 10.0f);
            if (Mathf.Abs(Camera.main.fieldOfView - fovTarget) < 0.01f)
            {
                isFOVTransitioning = false;
            }
        }
    }

    private void SetSprintOn()
    {
        cameraFOV = Camera.main.fieldOfView;
        fovTarget = cameraFOV * cameraFOVMultiplier;   
        speed *= sprintSpeedMultiplier;
        isFOVTransitioning = true;
    }

    private void SetSprintOff()
    {
        fovTarget = cameraFOV;
        speed /= sprintSpeedMultiplier;
        isFOVTransitioning = true;
    }

    private void FixedUpdate()
    {
        FixedMove();
        FixedJump();
    }

    void Update()
    {
        MovePlayer();

        if (sprintOption)
        {
            CheckSprintPress();
        }else if (!sprintOption)
        {
            CheckSprintHold();
        }
        
        
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerRig = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        
    }
}
