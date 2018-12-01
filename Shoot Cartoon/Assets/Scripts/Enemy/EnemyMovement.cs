using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth=player.GetComponent<PlayerHealth>();
        enemyHealth=GetComponent<EnemyHealth>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(enemyHealth.currentHealth>0&&playerHealth.currentHealth>0)
        {
            nav.SetDestination(player.position);
        }
        else
        {
            nav.enabled=false;
        }
    }
}
