using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    public GameObject player;

    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        KillAll();
    }

    void KillAll()
    {
        if (playerMovement.isGameOver)
        {
            Destroy(gameObject);
        }
    }
}
