using UnityEngine;
using System.Collections;

public class Enemy_Attack : MonoBehaviour {
    public float TimeBetweenAttacks = 0.5f;
    public int AttackDamage = 10;

    //Animator anim;
    GameObject player;
    Player_Health playerHealth;
    Enemy_Health enemyHealth;
    bool playerInRange;
    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Player_Health>();
        enemyHealth = GetComponent<Enemy_Health>();
        //anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == player)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == player)
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TimeBetweenAttacks && playerInRange && enemyHealth.CurrentHealth > 0) 
        {
            Attack();
        }
        if (playerHealth.CurrentHealth <= 0)
        {
            //anim.SetTrigger("Pla")
            //Debug.Log("플레이어 사망");
        }
    }
    void Attack()
    {
        timer = 0f;
        if (playerHealth.CurrentHealth > 0)
        {
            playerHealth.TakeDamage(AttackDamage);
        }
    }
}
