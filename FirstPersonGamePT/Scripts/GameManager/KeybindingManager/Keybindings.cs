using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    //All the Keys used in the Game (if a Key is missing, just add ít here and add a case)
    //https://www.youtube.com/watch?v=qP6BbUxFuRI
    public KeyCode forward, left, back, right, leanLeft, leanRight, reload, chat, interact, lookAround, sprint, crouch, prone, jump, drop, aim, shoot, pause;

    public KeyCode CheckKey(string key)
    {
        switch (key)
        {
            case "Forward":
                return forward;
            case "Left":
                return left;
            case "Back":
                return back;
            case "Right":
                return right;
            case "LeanLeft":
                return leanLeft;
            case "LeanRight":
                return leanRight;
            case "Reload":
                return reload;
            case "Chat":
                return chat;
            case "Interact":
                return interact;
            case "LookAround":
                return lookAround;
            case "Sprint":
                return sprint;
            case "Crouch":
                return crouch;
            case "Prone":
                return prone;
            case "Jump":
                return jump;
            case "Drop":
                return drop;
            case "Aim":
                return aim;
            case "Shoot":
                return shoot;
            case "Pause":
                return pause;

            default:
                throw new System.Exception("Invalid Key name");



        }
    }
}
