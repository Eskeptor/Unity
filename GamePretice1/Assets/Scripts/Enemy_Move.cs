using UnityEngine;
using System.Collections;

public class Enemy_Move : MonoBehaviour {
    Transform player;
    NavMeshAgent nav;
    Player_Health playerHealth;
    Enemy_Health enemyHealth;
	
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<Player_Health>();
        enemyHealth = GetComponent<Enemy_Health>();
        nav = GetComponent<NavMeshAgent>();
    }
	
	void Update ()
    {
        if (enemyHealth.CurrentHealth > 0 && playerHealth.CurrentHealth > 0)
        {
            nav.SetDestination(player.position);
        }
        else
        {
            nav.enabled = false;
        }
	}
}
