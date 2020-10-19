using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health = 100.0f;
    private float maxHealth = 100.0f;

    public Slider healthSlider;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            healthSlider.value = health;

            if (health <= 0)
            {
                healthSlider.value = maxHealth;
                Die();
            }
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            if (value > 0)
            {
                maxHealth = value;

                healthSlider.maxValue = maxHealth;

                if (maxHealth < health)
                {
                    health = maxHealth;
                    healthSlider.value = health;
                }
            }
        }
    }

    private GameObject player;
    private Rigidbody playerRig;

    public void Die()
    {
        playerRig.velocity = Vector3.zero;
        player.transform.position = new Vector3(0, 5, 0);
        player.transform.rotation = Quaternion.Euler(Vector3.zero);
        GetComponent<Camera>().transform.rotation = Quaternion.Euler(Vector3.zero);

        health = maxHealth;
    }

    private void Update()
    {
        player = GameObject.Find("Player");
        playerRig = player.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;
    }
}
