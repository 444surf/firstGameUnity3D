using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{

    private PlayerMovement playerMovement;

    public GameObject afterDeath;



    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        SetDeathScreen();
    }

    public void RestartThis()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SetDeathScreen()
    {
        if (playerMovement.isGameOver)
        {
            afterDeath.SetActive(true);
        }
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("StartScreen");  
    }
}
