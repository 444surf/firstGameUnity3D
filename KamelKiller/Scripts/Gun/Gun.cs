using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{

    // The Damage and Range for the Gun
    public float damage = 10f;
    public float range = 1000000;
    public float fireRate = 0.5f;
    public int ammunition = 5;
    public int magazine = 5;

    private AudioSource enemyAudio;
    public AudioClip camelDeath;
    public AudioClip gunShot;
    public AudioClip reload;

    public TextMeshProUGUI ammunitiontext;


    public bool currentlyReloading = false;

    private float nextTimetoFire = 0;

    private Animator animator;

    // The Object to Shoot from
    public GameObject shootFrom;

    public PlayerMovement playerMovement;
    

    private void Start()
    {
        enemyAudio = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        

    }
    // Update is called once per frame
    void Update()
    {
        // if leftmouse was clicked
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimetoFire && ammunition > 0 && currentlyReloading == false)
        {
            animator.SetTrigger("isShooting");
            nextTimetoFire = Time.time + 1f / fireRate;
            Shoot();
            enemyAudio.PlayOneShot(gunShot, 0.5f);
            ammunition--;
        }
        StartCoroutine(Reload());

        TextUpdate();

        GunRemove();
    }

    //
    void Shoot ()
    {
        //Stores information what got hit with the ray
        RaycastHit hit;
        

        if (Physics.Raycast(shootFrom.transform.position, shootFrom.transform.forward, out hit, range) && currentlyReloading == false)
        {

            

            Hit target = hit.transform.GetComponent<Hit>();
            if (target != null)
            {
                if (target.CompareTag("Enemy"))
                {
                    enemyAudio.PlayOneShot(camelDeath, 0.2f);
                    target.Death(damage);
                }
                
            }
        }
    }

    public IEnumerator Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && ammunition < magazine && currentlyReloading == false)
        {
            animator.SetBool("isReloading", true);
            currentlyReloading = true;
            enemyAudio.PlayOneShot(reload, 0.6f);
            yield return new WaitForSeconds(1.4f);
            ammunition += (magazine - ammunition);
            currentlyReloading = false;
            animator.SetBool("isReloading", false);
        }
    }

    void TextUpdate()
    {
        ammunitiontext.text = ammunition.ToString() + "\n" + "-" + "\n" + "5";
    }

    void GunRemove()
    {
        if (playerMovement.isGameOver)
        {
            gameObject.SetActive(false);
        }
    }
}
