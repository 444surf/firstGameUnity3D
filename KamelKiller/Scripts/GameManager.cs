using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //SpawnRange
    private float Spawn1 = 15;
    private float Spawn2 = 85;
    //Ganeobject to spawn
    public GameObject enemy;
    //The SpawnNumber
    private int spawnNumber = 0;
    //Counts the Enemies in the Game
    private int enemyCount;

    private int currentWave = 0;

    //Bool which stops Update from creating thousands of enemy because of the delay
    private bool isNewWave;

    public TextMeshProUGUI waveText;

    public PlayerMovement playerMovement;




    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Hit>().Length;
        if (enemyCount == 0 && isNewWave == false && !playerMovement.isGameOver)
        {
            StartCoroutine("nextSpawnWave");
            currentWave++;
            waveText.text = "Wave: " + currentWave;
        }
    }

    //Creates a random Spawn
    private Vector3 GenerateSpawnPosition()
    {
        float SpawnPosX = Random.Range(Spawn1, Spawn2);
        float SpawnPosZ = Random.Range(Spawn1, Spawn2);

        Vector3 randomPos = new Vector3(SpawnPosX, 0, SpawnPosZ);

        return randomPos;
    }

    void SpawnEnemyWave (int spawnNumber)
    {
        for (int i = 0; i < spawnNumber; i++)
        {
            Instantiate(enemy, GenerateSpawnPosition(), enemy.transform.rotation);
        }
        
    }


    private IEnumerator nextSpawnWave()
    {
        spawnNumber++;
        isNewWave = true;
        yield return new WaitForSeconds(3);
        SpawnEnemyWave(spawnNumber);
        isNewWave = false;
    }
}
