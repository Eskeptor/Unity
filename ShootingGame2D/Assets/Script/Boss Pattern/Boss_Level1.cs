﻿using UnityEngine;

public class Boss_Level1 : MonoBehaviour {
    /* Public Object */
    public GameObject Explosion1;       // Explosion object(When the boss1 died)
    public GameObject Explosion2;
    public GameObject Explosion3; 
    public GameObject MissileObject1;   // Boss1's missile object
    public GameObject MissileObject2;
    public GameObject MissileObject3;
    public Transform MissileLocation1;  // Boss1's missile fire location
    public Transform MissileLocation2;
    public Transform MissileLocation3;
    public Transform MissileLocation4;
    public int MissileMaximumPool = 4;  // Boss1's missile memory maximum pool
    public float FireRateTime = 1f;     // Boss1's missile fire rate time

    /* Private Object */
    private GameObject EventSP;                     // for Event_ScoreHP
    private GameObject DownShift;                   // for Player's DownShift
    private MemoryPool MPool1 = new MemoryPool();   // for Boss1's missile memory pool
    private MemoryPool MPool2 = new MemoryPool();
    private MemoryPool MPool3 = new MemoryPool();
    private GameObject[] Missile1;                  // for Boss1's missile
    private GameObject[] Missile2;
    private GameObject[] Missile3;
    private byte Pattern;                            // Boss1's missile pattern

    // When application quit, Memory clear
    void OnApplicationQuit()
    {
        MPool1.Dispose();
        MPool2.Dispose();
        MPool3.Dispose();
    }

    // Use this for initialization
    void Start () {
        EventSP = GameObject.Find("Player Event");
        DownShift = GameObject.Find("DownShift");

        // Create missile into memory pool
        MPool1.Create(MissileObject1, MissileMaximumPool * 2);
        MPool2.Create(MissileObject2, MissileMaximumPool / 2);
        MPool3.Create(MissileObject3, MissileMaximumPool / 2);
        Missile1 = new GameObject[MissileMaximumPool * 2];
        Missile2 = new GameObject[MissileMaximumPool / 2];
        Missile3 = new GameObject[MissileMaximumPool / 2];

        GetComponent<Enemy_Info>().ScoreCheck = false;
        GetComponent<Enemy_Info>().FireState = true;
        GetComponent<Enemy_Info>().FireEnabled = false;
        Pattern = 1;
    }
	
	// Update is called once per frame
	void Update () {
        IsDead();
        MissileFire();
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
        for (int i = 0; i < MissileMaximumPool * 2; i++) 
        {
            if (Missile1[i])
            {
                Missile1[i].GetComponent<Collider2D>().enabled = true;
                MPool1.RemoveItem(Missile1[i]);
                Missile1[i] = null;
            }
        }
        for(int i = 0; i < MissileMaximumPool / 2; i++)
        {
            if (Missile2[i])
            {
                Missile2[i].GetComponent<Collider2D>().enabled = true;
                MPool2.RemoveItem(Missile2[i]);
                Missile2[i] = null;
            }
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
            if (!GetComponent<Enemy_Info>().ScoreCheck)
            {
                GetComponent<Enemy_Info>().FireEnabled = false;
                GetComponent<Enemy_Info>().HP = 0;
                EventSP.GetComponent<Event_ScoreHP>().AddScore(GetComponent<Enemy_Info>().Score);
                GetComponent<AudioSource>().Play();
                GetComponent<Enemy_Info>().ScoreCheck = true;
            }
            Explosion1.SetActive(true);
            Explosion2.SetActive(true);
            Explosion3.SetActive(true);
            EventSP.GetComponent<Event_ScoreHP>().BossDeathCheck = true;
            Invoke("Dead", 3f);
        }
    }

    // Missile fire
    void MissileFire()
    {
        if (GetComponent<Enemy_Info>().FireEnabled)
        {
            if (GetComponent<Enemy_Info>().FireState)
            {
                // Boss1's missile pattern
                switch (Pattern)
                {
                    case 1:
                        {
                            Invoke("FireCycleControl", FireRateTime);
                            if (Missile1[0] == null && Missile1[1] == null && Missile1[2] == null && Missile1[3] == null)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    Missile1[i] = MPool1.NewItem();
                                }
                                Missile1[0].transform.position = MissileLocation1.transform.position;
                                Missile1[1].transform.position = MissileLocation2.transform.position;
                                Missile1[2].transform.position = MissileLocation3.transform.position;
                                Missile1[3].transform.position = MissileLocation4.transform.position;
                            }
                            GetComponent<Enemy_Info>().FireState = false;
                            break;
                        }
                    case 2:
                        {
                            Invoke("FireCycleControl", FireRateTime);
                            if (Missile1[4] == null && Missile1[5] == null && Missile1[6] == null && Missile1[7] == null)
                            {
                                for (int i = 4; i < 8; i++)
                                {
                                    Missile1[i] = MPool1.NewItem();
                                }
                                Missile1[4].transform.position = MissileLocation1.transform.position;
                                Missile1[5].transform.position = MissileLocation2.transform.position;
                                Missile1[6].transform.position = MissileLocation3.transform.position;
                                Missile1[7].transform.position = MissileLocation4.transform.position;
                            }
                            GetComponent<Enemy_Info>().FireState = false;
                            break;
                        }
                    case 3:
                        {
                            Invoke("FireCycleControl", FireRateTime);
                            if (Missile2[0] == null && Missile2[1] == null)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    Missile2[i] = MPool2.NewItem();
                                }
                                Missile2[0].transform.position = MissileLocation1.transform.position;
                                Missile2[1].transform.position = MissileLocation4.transform.position;
                            }
                            GetComponent<Enemy_Info>().FireState = false;
                            break;
                        }
                    case 4:
                        {
                            Invoke("FireCycleControl", FireRateTime);
                            if (Missile3[0] == null && Missile3[1] == null)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    Missile3[i] = MPool3.NewItem();
                                }
                                Missile3[0].transform.position = MissileLocation2.transform.position;
                                Missile3[1].transform.position = MissileLocation3.transform.position;
                            }
                            GetComponent<Enemy_Info>().FireState = false;
                            break;
                        }
                }
            }
        }

        // Returns missiles in memory pool
        for (int i = 0; i < MissileMaximumPool * 2; i++)
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
            if (i < 2)
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
    }

    // Fire Cycle Control
    private void FireCycleControl()
    {
        GetComponent<Enemy_Info>().FireState = true;
        ++Pattern;  // next pattern
        if (Pattern >= 5)
        {
            // If the pattern is finished, start the pattern again.
            Pattern = 1;
        }
    }
}
