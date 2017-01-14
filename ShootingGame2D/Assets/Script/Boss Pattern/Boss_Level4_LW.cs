using System.Collections;
using UnityEngine;

public class Boss_Level4_LW : MonoBehaviour
{
    /* Public Object */
    public GameObject Explosion1;               // Explosion object(When the boss1 died)
    public GameObject MissileObject1;           // Boss3's missile object
    public Transform MissileLocation1;          // Boss3's missile fire location
    public float FireRateTime = 0.2f;           // Boss3's missile fire rate time
    public int Missile1Pool = 30;
    [HideInInspector]
    public int HP;

    /* Private Object */
    private bool FireEnabled;                       // By measuring the distance to fire a missile
    private bool FireState;                         // for Fire cycle control
    private bool Score;                             // Check whether boss gave the score
    private GameObject EventSP;                     // for Event_ScoreHP
    private GameObject DownShift;                   // for Player's DownShift
    private GameObject Boss4;
    private MemoryPool MPool1 = new MemoryPool();   // for Boss2's missile memory pool
    private GameObject[] Missile1;                  // for Boss2's missile

    // When application quit, Memory clear
    void OnApplicationQuit()
    {
        MPool1.Dispose();
    }

    // Use this for initialization
    void Start()
    {
        EventSP = GameObject.Find("Player Event");
        DownShift = GameObject.Find("DownShift");
        Boss4 = GameObject.Find("Level4 Boss");

        // Create Missile
        MPool1.Create(MissileObject1, Missile1Pool);
        Missile1 = new GameObject[Missile1Pool];


        Score = false;
        FireState = true;
        FireEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsDead();
        if (FireEnabled)
        {
            MissileFire();
        }
    }

    // for Collision check
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_PLAYER))
        {
            GetComponent<Enemy_Info>().HP -= Player_Data.Damage;
            //Debug.Log("Enemy_Move : 플레이어와 부딛힘");
        }
        else if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_PLAYER_MISSILE))
        {
            GetComponent<Enemy_Info>().HP -= Player_Data.Damage;
            //Debug.Log("Enemy_Move : 미사일과 부딛힘");
        }
    }

    // When enemy is down, deactivate object(not destroy) and returns missiles in memory pool
    void Dead()
    {

        for (int i = 0; i < Missile1.Length; i++)
        {
            if (Missile1[i])
            {
                Missile1[i].GetComponent<Collider2D>().enabled = true;
                MPool1.RemoveItem(Missile1[i]);
                Missile1[i] = null;
            }
        }

        Boss4.GetComponent<Boss_Level4>().LeftDead = true;
        gameObject.SetActive(false);
    }

    // Dead checker
    void IsDead()
    {
        if (GetComponent<Enemy_Info>().HP <= 0)
        {
            if (!Score)
            {
                FireEnabled = false;
                GetComponent<Enemy_Info>().HP = 0;
                EventSP.GetComponent<Event_ScoreHP>().AddScore(GetComponent<Enemy_Info>().Score);
                GetComponent<AudioSource>().Play();
                Score = true;
            }
            Explosion1.SetActive(true);
            Invoke("Dead", 3f);
        }
    }

    // Missile fire
    void MissileFire()
    {
        if (FireEnabled)
        {
            if (FireState)
            {
                //Debug.Log("SemiPattern : " + SemiPattern);
                //Debug.Log("Pattern : " + Pattern);
                // Boss3's missile pattern

            }
        }

        // Returns missiles in memory pool

    }


    // Fire Cycle Control
    private void FireCycleControl()
    {
        FireState = true;
        
    }
}