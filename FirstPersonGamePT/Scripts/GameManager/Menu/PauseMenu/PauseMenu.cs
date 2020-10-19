using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;

    [SerializeField] GameObject pauseMenuUI;

    //Asks if the PauseButton was pressed and then asks if the Game is Currently paused or not, if game is paused => Resume, else => Pause
    void Pause()
    {
        if (InputManager.instance.GetKeyDown("Pause"))
        {
            if (IsPaused)
            {
                ResumeGame();
            }else
            {
                PauseGame();
            }
        }
    }

    //Pauses the Game
    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        IsPaused = false;
    }

    //Resumes the Game
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        IsPaused = true;
    }

    private void Update()
    {
        Pause();
    }

    void Sensitivity()
    {

    }
}
