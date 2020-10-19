using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public Keybindings keybindings;

    private void Awake()
    {
        //This Awakemethod destroys the InputManager if there are more of one in here
        
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this) 
        {
            Destroy(this);
        }
        
        DontDestroyOnLoad(this);
    }



    /* If you pass in a string, the method is going to check if
    the key part of the switch statement in the Keybindings.cs script
    and it is going to return the respective key assigned to the Keycode*/

    public bool GetKeyDown(string key)
    {
        return Input.GetKeyDown(keybindings.CheckKey(key));
    }

    public bool GetKeyUp(string key)
    {
        return Input.GetKeyUp(keybindings.CheckKey(key));
    }

    public float GetAxis(string key1, string key2)
    {
        float returnVal = 0;

        returnVal += GetKeyDown(key1) ? 1 : 0;

        returnVal += GetKeyDown(key2) ? -1 : 0;

        return returnVal;
    }

    //Both GetKey and GetKeyDown are viable methods
    public bool GetKey(string key)
    {
        if (Input.GetKey(keybindings.CheckKey(key)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //To use the new KeyCode Method, type "InputManager.instance.KeyDown("x")
}
