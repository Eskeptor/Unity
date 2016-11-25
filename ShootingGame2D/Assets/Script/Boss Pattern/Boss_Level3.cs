using UnityEngine;

public class Boss_Level3 : MonoBehaviour
{
    /* Public Object */
    public GameObject Explosion1;               // Explosion object(When the boss1 died)
    public GameObject Explosion2;
    public GameObject Explosion3;
    public GameObject MissileObject1;           // Boss2's missile object
    public GameObject MissileObject2;
    public GameObject MissileObject3;
    public GameObject MissileObject4;
    //public GameObject HiddenMissileObject;    // Not implements!!
    public Transform MissileLocation1;          // Boss2's missile fire location
    public Transform MissileLocation2;
    public Transform MissileLocation3;
    public Transform MissileLocation4;
    public Transform MissileLocation5;
    public Transform MissileLocation6;
    //public Transform HiddenMissileLocation;   // Not implements!!
    public int MissileMaximumPool = 6;          // Boss2's missile memory maximum pool
    public int ExtraPool = 6;                   // Boss2's missile extra memory maximum pool
    public float FireRateTime = 1f;            // Boss2's missile fire rate time

    /* Private Object */
    private bool FireEnabled;                       // By measuring the distance to fire a missile
    private bool FireState;                         // for Fire cycle control
    private bool Score;                             // Check whether boss gave the score
    private GameObject EventSP;                     // for Event_ScoreHP
    private GameObject DownShift;                   // for Player's DownShift
    private MemoryPool MPool1 = new MemoryPool();   // for Boss2's missile memory pool
    private MemoryPool MPool2 = new MemoryPool();
    private MemoryPool MPool3 = new MemoryPool();
    private MemoryPool MPool4 = new MemoryPool();
    private GameObject[] Missile1;                  // for Boss2's missile
    private GameObject[] Missile2;
    private GameObject[] Missile3;
    private GameObject[] Missile4;
    //private GameObject HMissile;                  // Not implements!!
    private int Pattern;                            // Boss2's missile pattern
    private int SemiPattern;                        // Boss2's missile semi pattern

    // When application quit, Memory clear
    void OnApplicationQuit()
    {
        MPool1.Dispose();
        MPool2.Dispose();
        MPool3.Dispose();
        MPool4.Dispose();
    }

    // Use this for initialization
    void Start()
    {
        EventSP = GameObject.Find("Player Event");
        DownShift = GameObject.Find("DownShift");

        // Create Missile
        MPool1.Create(MissileObject1, MissileMaximumPool * ExtraPool);
        MPool2.Create(MissileObject2, MissileMaximumPool * ExtraPool);
        MPool3.Create(MissileObject3, MissileMaximumPool);
        MPool4.Create(MissileObject4, 4);
        Missile1 = new GameObject[MissileMaximumPool * ExtraPool];
        Missile2 = new GameObject[MissileMaximumPool * ExtraPool];
        Missile3 = new GameObject[MissileMaximumPool];
        Missile4 = new GameObject[4];
        //HMissile = HiddenMissileObject;

        // All missile array initialize
        for (int i = 0; i < Missile1.Length; i++)
        {
            Missile1[i] = null;
            Missile2[i] = null;
        }
        for (int i = 0; i < Missile3.Length; i++)
        {
            Missile3[i] = null;
        }
        for (int i = 0; i < Missile4.Length; i++)
        {
            Missile4[i] = null;
        }

        Score = false;
        FireState = true;
        FireEnabled = false;
        Pattern = 1;
        SemiPattern = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DistanceChecker();
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
        for (int i = 0; i < Missile4.Length; i++)
        {
            if (Missile4[i])
            {
                Missile4[i].GetComponent<Collider2D>().enabled = true;
                MPool4.RemoveItem(Missile4[i]);
                Missile4[i] = null;
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
            Explosion3.SetActive(true);
            EventSP.GetComponent<Event_ScoreHP>().BossDeathCheck = true;
            Invoke("Dead", 3f);
        }
    }

    // Distance check between player and enemy
    void DistanceChecker()
    {
        if (transform.position.y - DownShift.transform.position.y < Constant.RECOGNIZED_PLAYER - 2f)
        {
            if (GetComponent<Enemy_Info>().HP > 0)
            {
                FireEnabled = true;
            }
            GameObject.Find(Constant.NAME_PLAYER).GetComponent<Auto_Move>().AutoCheck = false;
        }
        if (Player_Data.HP <= 0)
        {
            FireState = false;
            FireEnabled = false;
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
                // Boss2's missile pattern
                switch (Pattern)
                {
                    case 1:
                        {
                            Invoke("FireCycleControl", FireRateTime);
                            if (Missile1[SemiPattern * MissileMaximumPool] == null && Missile1[SemiPattern * MissileMaximumPool + 1] == null
                                && Missile1[SemiPattern * MissileMaximumPool + 2] == null && Missile1[SemiPattern * MissileMaximumPool + 3] == null
                                && Missile1[SemiPattern * MissileMaximumPool + 4] == null && Missile1[SemiPattern * MissileMaximumPool + 5] == null)
                            {
                                for (int i = SemiPattern * MissileMaximumPool; i < SemiPattern * MissileMaximumPool + ExtraPool; i++)
                                {
                                    Missile1[i] = MPool1.NewItem();
                                }
                                Missile1[SemiPattern * MissileMaximumPool].transform.position = MissileLocation1.transform.position;
                                Missile1[SemiPattern * MissileMaximumPool + 1].transform.position = MissileLocation2.transform.position;
                                Missile1[SemiPattern * MissileMaximumPool + 2].transform.position = MissileLocation3.transform.position;
                                Missile1[SemiPattern * MissileMaximumPool + 3].transform.position = MissileLocation4.transform.position;
                                Missile1[SemiPattern * MissileMaximumPool + 4].transform.position = MissileLocation5.transform.position;
                                Missile1[SemiPattern * MissileMaximumPool + 5].transform.position = MissileLocation6.transform.position;
                            }
                            FireState = false;
                            Pattern = 2;
                            break;
                        }
                    case 2:
                        {
                            Invoke("FireCycleControl", FireRateTime);
                            if (Missile2[SemiPattern * MissileMaximumPool] == null && Missile2[SemiPattern * MissileMaximumPool + 1] == null
                                && Missile2[SemiPattern * MissileMaximumPool + 2] == null && Missile2[SemiPattern * MissileMaximumPool + 3] == null
                                && Missile2[SemiPattern * MissileMaximumPool + 4] == null && Missile2[SemiPattern * MissileMaximumPool + 5] == null)
                            {
                                for (int i = SemiPattern * MissileMaximumPool; i < SemiPattern * MissileMaximumPool + ExtraPool; i++)
                                {
                                    Missile2[i] = MPool2.NewItem();
                                }
                                Missile2[SemiPattern * MissileMaximumPool].transform.position = MissileLocation1.transform.position;
                                Missile2[SemiPattern * MissileMaximumPool + 1].transform.position = MissileLocation2.transform.position;
                                Missile2[SemiPattern * MissileMaximumPool + 2].transform.position = MissileLocation3.transform.position;
                                Missile2[SemiPattern * MissileMaximumPool + 3].transform.position = MissileLocation4.transform.position;
                                Missile2[SemiPattern * MissileMaximumPool + 4].transform.position = MissileLocation5.transform.position;
                                Missile2[SemiPattern * MissileMaximumPool + 5].transform.position = MissileLocation6.transform.position;
                            }
                            FireState = false;
                            if (SemiPattern < 5)
                            {
                                SemiPattern++;
                                Pattern = 1;
                            }
                            else
                            {
                                Pattern++;
                                SemiPattern = 0;
                            }
                            break;
                        }
                    case 3:
                        {
                            Invoke("FireCycleControl", FireRateTime);
                            switch (SemiPattern)
                            {
                                case 0:
                                    {
                                        if (Missile3[0] == null && Missile3[1] == null)
                                        {
                                            for (int i = 0; i < 2; i++)
                                            {
                                                Missile3[i] = MPool3.NewItem();
                                            }
                                            Missile3[0].transform.position = new Vector3(MissileLocation2.transform.position.x, MissileLocation2.transform.position.y - 2.3f, 0f);
                                            Missile3[1].transform.position = new Vector3(MissileLocation5.transform.position.x, MissileLocation5.transform.position.y - 2.3f, 0f);
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        if (Missile3[2] == null && Missile3[3] == null)
                                        {
                                            for (int i = 2; i < 4; i++)
                                            {
                                                Missile3[i] = MPool3.NewItem();
                                            }
                                            Missile3[2].transform.position = new Vector3(MissileLocation3.transform.position.x, MissileLocation3.transform.position.y - 2.3f, 0f);
                                            Missile3[3].transform.position = new Vector3(MissileLocation6.transform.position.x, MissileLocation6.transform.position.y - 2.3f, 0f);
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        if (Missile3[4] == null && Missile3[5] == null)
                                        {
                                            for (int i = 4; i < 6; i++)
                                            {
                                                Missile3[i] = MPool3.NewItem();
                                            }
                                            Missile3[4].transform.position = new Vector3(MissileLocation1.transform.position.x, MissileLocation1.transform.position.y - 2.3f, 0f);
                                            Missile3[5].transform.position = new Vector3(MissileLocation4.transform.position.x, MissileLocation4.transform.position.y - 2.3f, 0f);
                                        }
                                        break;
                                    }
                            }

                            FireState = false;
                            if (SemiPattern < 2)
                            {
                                SemiPattern++;
                            }
                            else
                            {
                                Pattern++;
                                SemiPattern = 0;
                            }
                            break;
                        }
                    case 4:
                        {
                            Invoke("FireCycleControl", FireRateTime);
                            switch (SemiPattern)
                            {
                                case 0:
                                    {
                                        if (Missile4[0] == null && Missile4[1] == null)
                                        {
                                            for (int i = 0; i < 2; i++)
                                            {
                                                Missile4[i] = MPool4.NewItem();
                                            }
                                            Missile4[0].transform.position = MissileLocation1.transform.position;
                                            Missile4[1].transform.position = MissileLocation6.transform.position;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        if (Missile4[2] == null && Missile4[3] == null)
                                        {
                                            for (int i = 2; i < 4; i++)
                                            {
                                                Missile4[i] = MPool4.NewItem();
                                            }
                                            Missile4[2].transform.position = MissileLocation3.transform.position;
                                            Missile4[3].transform.position = MissileLocation4.transform.position;
                                        }
                                        break;
                                    }
                            }
                            FireState = false;
                            if (SemiPattern < 1)
                            {
                                SemiPattern++;
                            }
                            else
                            {
                                SemiPattern = 0;
                                Pattern++;
                            }
                            break;
                        }
                }
            }
        }

        // Returns missiles in memory pool
        for (int i = 0; i < Missile1.Length; i++)
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
            if (Missile2[i])
            {
                if (Missile2[i].GetComponent<Collider2D>().enabled == false)
                {
                    Missile2[i].GetComponent<Collider2D>().enabled = true;
                    MPool2.RemoveItem(Missile2[i]);
                    Missile2[i] = null;
                }
            }
            if (i < 6)
            {
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
            if (i < 4)
            {
                if (Missile4[i])
                {
                    if (Missile4[i].GetComponent<Collider2D>().enabled == false)
                    {
                        Missile4[i].GetComponent<Collider2D>().enabled = true;
                        MPool4.RemoveItem(Missile4[i]);
                        Missile4[i] = null;
                    }
                }
            }
        }
    }



    // Fire Cycle Control
    private void FireCycleControl()
    {
        FireState = true;
        if (Pattern >= 5)
        {
            Pattern = 1;
        }
    }
}