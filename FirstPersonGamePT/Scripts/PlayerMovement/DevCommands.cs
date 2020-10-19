using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevCommands : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void DeveloperCommands()
    {

        if (Input.GetKey(KeyCode.R))
        {
            playerHealth.Die();
        }
    }


    void Start()
    {
        
    }


    void Update()
    {
        DeveloperCommands();
    }
}
