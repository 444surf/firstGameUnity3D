using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //Gets the CharacterController Component
    public CharacterController controller;

    //Movement Speed
    public float movementSpeed = 12f;

    //The gravitational pull
    public float gravityPull = -9.81f;

    //Vector 3 for Gravity over Time 
    Vector3 gravityVelocity;

    //The PlayerModel
    public GameObject playerModelBlue;

    public bool isGameOver = false;


    //Everything to check if Player is on Ground (dont understand)
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    //Jumpheight to define how high to jump
    public float jumpHeight = 3f;

    //checking if crouching
    private bool isCrouching;

    //CanvasBullshit
    public GameObject stats;

    //Map Gameobject
    public GameObject desert;

    //GameObjects of Boundaries
    public GameObject xBoundary;
    public GameObject zBoundary;
    public GameObject xBoundary2;
    public GameObject zBoundary2;

    //Player Life
    public int health = 3;

    //CamelSound
    public AudioClip camelsound;

    //Hitsound
    public AudioClip Hitsound;

    //AudioSource
    private AudioSource playerAudio;

    //Health Canvas
    public TextMeshProUGUI healthBar;



    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {

        healthBar.text = "Health: " + health;
        Deactive();

        GroundChecker();
        CheckingGround();


        // Gets X and Z Input (W-A-S-D)
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        if (!isGameOver)
        {
            //Jumps
            Jump();

            //Crouches
            Crouch();

            //Sprints
            Sprint();

            //Vector 3 which takes the direction of the Player
            Vector3 move = transform.right * xMovement + transform.forward * zMovement;

            //Tells the CharacterController to Move with the Vector3 move above and the Movementspeed and Deltatime
            controller.Move(move * movementSpeed * Time.deltaTime);
        }

        




        

        //Gravitationalpull
        GravitationalPull();

        

        //Boundary
        Boundaries();

        //Checking Death
        Death();
    }

    //Checks if hes on Ground
    bool GroundChecker()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        return isGrounded;
    }

    //Sets the Velocity overtime to 0
    void CheckingGround()
    {
        if (GroundChecker() && gravityVelocity.y < 0)
        {
            gravityVelocity.y = -2f;
        }
    }

    //Gravitational Pull equasion an use it
    void GravitationalPull()
    {

        gravityVelocity.y += gravityPull * (Time.deltaTime * Time.deltaTime);
        controller.Move(gravityVelocity);
    }

    //Jump
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && GroundChecker())
        {
            gravityVelocity.y = Mathf.Sqrt((jumpHeight * -2 * gravityPull) / 2000);
            Debug.Log(gravityVelocity.y);
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = 1f;
            playerModelBlue.transform.localScale = new Vector3(playerModelBlue.transform.localScale.x, 0.5f, playerModelBlue.transform.localScale.z);
            isCrouching = true;
            movementSpeed = 8f;
            jumpHeight = 5f;
        }
        else
        {
            controller.height = 2f;
            playerModelBlue.transform.localScale = new Vector3(playerModelBlue.transform.localScale.x, 1f, playerModelBlue.transform.localScale.z);
            isCrouching = false;
            movementSpeed = 12f;
            jumpHeight = 3f;
        }
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isCrouching == false)
        {
            movementSpeed = 20f;
            jumpHeight = 1.5f;

        }
        else if (isCrouching == false)
        {
            movementSpeed = 12f;
            jumpHeight = 3f;
        }
    }

    void Boundaries()
    {
        //Boundarys are 100 and 0 brings player back to boundary if they cross
        if (transform.position.x < xBoundary.transform.position.x)
        {
            transform.position = new Vector3(xBoundary.transform.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.z < zBoundary.transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBoundary.transform.position.z);
        }
        else if (transform.position.x > xBoundary2.transform.position.x)
        {
            transform.position = new Vector3(xBoundary2.transform.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.z > zBoundary2.transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBoundary2.transform.position.z);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            health -= 1;
            Destroy(other.gameObject);
            playerAudio.PlayOneShot(Hitsound, 0.7f);
        }
    }

    private void Death()
    {
        if (health == 0)
        {
            isGameOver = true;
        }
    }

    private void Deactive()
    {
        if (isGameOver == true)
        {
            stats.SetActive(false);
        }
    }

}
