using System.Collections;
using UnityEngine;

public class Boss_Level4 : MonoBehaviour
{
    /* Public Object */
    public Boss_Level4_LW LeftWing;
    public Boss_Level4_RW RightWing;
    public Boss_Level4_BD Body;
    [HideInInspector]
    public bool LeftDead, RightDead, BodyDead;
    private GameObject EventSP;                     // for Event_ScoreHP
    private GameObject DownShift;                   // for Player's DownShift

    // Use this for initialization
    void Start()
    {
        EventSP = GameObject.Find("Player Event");
        DownShift = GameObject.Find("DownShift");
        LeftDead = RightDead = BodyDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsDead();
    }

    // When enemy is down, deactivate object(not destroy) and returns missiles in memory pool
    void Dead()
    {
        EventSP.GetComponent<Event_ScoreHP>().BossDeathCheck = true;
        gameObject.SetActive(false);
    }

    // Dead checker
    void IsDead()
    {
        if (LeftDead && RightDead && BodyDead)
        {
            EventSP.GetComponent<Event_ScoreHP>().BossDeathCheck = true;
            Invoke("Dead", 3f);
        }
    }
}