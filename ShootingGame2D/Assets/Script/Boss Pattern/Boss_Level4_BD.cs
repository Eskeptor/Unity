using System.Collections;
using UnityEngine;

public class Boss_Level4_BD : MonoBehaviour
{
    /* Public Object */
    public GameObject Explosion1;               // Explosion object(When the boss1 died)
    public GameObject Explosion2;
    public GameObject MissileObject1;           // Boss4's missile object
    public GameObject MissileObject2;
    public Transform MissileLocation1;          // Boss4's missile fire location
    public Transform MissileLocation2;

    public float FireRateTime = 1f;            // Boss4's missile fire rate time
    [HideInInspector]
    public int HP;

    /* Private Object */
    private bool FireEnabled;                       // By measuring the distance to fire a missile
    private bool FireState;                         // for Fire cycle control
    private bool Score;                             // Check whether boss gave the score
    private GameObject EventSP;                     // for Event_ScoreHP
    private GameObject DownShift;                   // for Player's DownShift
    private GameObject Boss4;
    private MemoryPool MPool1 = new MemoryPool();   // for Boss4's missile memory pool
    private MemoryPool MPool2 = new MemoryPool();
    private MemoryPool MPool3 = new MemoryPool();
    private GameObject[] Missile1;                  // for Boss4's missile
    private GameObject[] Missile2;
    private GameObject[] Missile3;
    private byte Pattern;                            // Boss4's missile pattern
    private byte SemiPattern;                        // Boss4's missile semi pattern

    private const byte Missile1Pool = 2;
    private const byte Missile2Pool = 1;
    private const byte Missile3Pool = 5;
    private const byte SemiPool = 4;

    // When application quit, Memory clear
    void OnApplicationQuit()
    {
        MPool1.Dispose();
        MPool2.Dispose();
        MPool3.Dispose();
    }

    // Use this for initialization
    void Start()
    {
        EventSP = GameObject.Find("Player Event");
        DownShift = GameObject.Find("DownShift");
        Boss4 = GameObject.Find("Level4 Boss");

        // Create Missile
        MPool1.Create(MissileObject1, Missile1Pool * SemiPool);
        MPool2.Create(MissileObject2, Missile2Pool * SemiPool);
        Missile1 = new GameObject[Missile1Pool * SemiPool];
        Missile2 = new GameObject[Missile2Pool * SemiPool];
        Missile3 = new GameObject[Missile3Pool * SemiPool];

        Score = false;
        FireState = true;
        FireEnabled = false;
        Pattern = 1;
        SemiPattern = 0;
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
        for (int i = 0; i < Missile2.Length; i++)
        {
            if (Missile2[i])
            {
                Missile2[i].GetComponent<Collider2D>().enabled = true;
                MPool2.RemoveItem(Missile2[i]);
                Missile2[i] = null;
            }
        }
        for (int i = 0; i < Missile3.Length; i++)
        {
            if (Missile3[i])
            {
                Missile3[i].GetComponent<Collider2D>().enabled = true;
                MPool3.RemoveItem(Missile3[i]);
                Missile3[i] = null;
            }
        }
        EventSP.GetComponent<Event_ScoreHP>().BossDeathCheck = true;
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
            Explosion2.SetActive(true);
            EventSP.GetComponent<Event_ScoreHP>().BossDeathCheck = true;
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
        for (int i = 0; i < Missile3.Length; i++)
        {
            if (i < Missile1.Length)
            {
                if (Missile1[i])
                {
                    if (Missile1[i].GetComponent<Collider2D>().enabled == false)
                    {
                        Missile1[i].GetComponent<Collider2D>().enabled = true;
                        MPool1.RemoveItem(Missile1[i]);
                        Missile1[i] = null;
                    }
                }
            }
            if (i < Missile2.Length)
            {
                if (Missile2[i])
                {
                    if (Missile2[i].GetComponent<Collider2D>().enabled == false)
                    {
                        Missile2[i].GetComponent<Collider2D>().enabled = true;
                        MPool2.RemoveItem(Missile2[i]);
                        Missile2[i] = null;
                    }
                }
            }
            if (Missile3[i])
            {
                if (Missile3[i].GetComponent<Collider2D>().enabled == false)
                {
                    Missile3[i].GetComponent<Collider2D>().enabled = true;
                    MPool3.RemoveItem(Missile3[i]);
                    Missile3[i] = null;
                }
            }
            
        }
    }


    // Fire Cycle Control
    private void FireCycleControl()
    {
        FireState = true;
        if (Pattern >= 4)
        {
            Pattern = 1;
        }
    }
}