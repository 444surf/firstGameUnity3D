using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public float health = 1;


    

    private void Start()
    {
        
    }


    public void Death(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            
            Destroy(gameObject);

        }
    }




}
