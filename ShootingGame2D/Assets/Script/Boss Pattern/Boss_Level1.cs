using UnityEngine;

public class Boss_Level1 : MonoBehaviour {
    public GameObject Explosion1;
    public GameObject Explosion2;
    public GameObject Explosion3;
    public GameObject MissileObject1;
    public GameObject MissileObject2;
    public GameObject MissileObject3;
    public Transform MissileLocation1;
    public Transform MissileLocation2;
    public Transform MissileLocation3;
    public Transform MissileLocation4;
    public int MissileMaximumPool = 4;
    public float FireCycleTime = 1f;

    private bool FireEnabled;   // By measuring the distance to fire a missile
    private bool FireState;     // for Fire cycle control
    private GameObject Player;
    private GameObject EventSP;
    private GameObject DownShift;
    private MemoryPool MPool1 = new MemoryPool();
    private MemoryPool MPool2 = new MemoryPool();
    private MemoryPool MPool3 = new MemoryPool();
    private GameObject[] Missile1;
    private GameObject[] Missile2;
    private GameObject[] Missile3;
    private int Pattern;

    // When application quit, Memory clear
    void OnApplicationQuit()
    {
        MPool1.Dispose();
        MPool2.Dispose();
        MPool3.Dispose();
    }

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Aircraft Body");
        EventSP = GameObject.Find("ScoreHP Event");
        DownShift = GameObject.Find("DownShift");

        // Create Missile
        MPool1.Create(MissileObject1, MissileMaximumPool * 2);
        MPool2.Create(MissileObject2, MissileMaximumPool / 2);
        MPool3.Create(MissileObject3, MissileMaximumPool / 2);
        Missile1 = new GameObject[MissileMaximumPool * 2];
        Missile2 = new GameObject[MissileMaximumPool / 2];
        Missile3 = new GameObject[MissileMaximumPool / 2];
        
        for (int i = 0; i < MissileMaximumPool * 2; i++)
        {
            Missile1[i] = null;
        }
        for(int i = 0; i < MissileMaximumPool / 2; i++)
        {
            Missile2[i] = null;
            Missile3[i] = null;
        }

        FireState = true;
        FireEnabled = false;
        Pattern = 1;
    }
	
	// Update is called once per frame
	void Update () {
        DistanceChecker();
        if (FireEnabled)
        {
            IsDead();
            MissileFire();
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().tag == "Player")
        {
            GetComponent<Enemy_Info>().HP -= Player.GetComponent<Player_Fire>().Damage;
            EventSP.GetComponent<Event_ScoreHP>().MinHP(50);
            Debug.Log("Enemy_Move : 플레이어와 부딛힘");
        }
        else if (col.GetComponent<Collider2D>().tag == "Missile")
        {
            GetComponent<Enemy_Info>().HP -= Player.GetComponent<Player_Fire>().Damage;
            Debug.Log("Enemy_Move : 미사일과 부딛힘");
        }
    }

    // When enemy is down, deactivate object(not destroy)
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
        EventSP.GetComponent<Event_ScoreHP>().check = true;
        gameObject.SetActive(false);
    }

    // Dead checker
    void IsDead()
    {
        if (GetComponent<Enemy_Info>().HP <= 0)
        {
            EventSP.GetComponent<Event_ScoreHP>().AddScore(GetComponent<Enemy_Info>().Score);
            FireEnabled = false;
            Explosion1.SetActive(true);
            Explosion2.SetActive(true);
            Explosion3.SetActive(true);
            Invoke("Dead", 3f);
        }
    }

    // Distance check between player and enemy
    void DistanceChecker()
    {
        if(transform.position.y-DownShift.transform.position.y < Constant.MAX_YPOS_UP - 2f)
        {
            FireEnabled = true;
            GameObject.Find("Player").GetComponent<Auto_Move>().AutoCheck = false;
        }
        if(EventSP.GetComponent<Event_ScoreHP>().hp == 0)
        {
            FireState = false;

        }
    }

    // Missile fire
    void MissileFire()
    {
        if (FireState)
        {
            switch (Pattern)
            {
                case 1:
                    {
                        Invoke("FireCycleControl", FireCycleTime);
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
                        FireState = false;
                        break;
                    }
                case 2:
                    {
                        Invoke("FireCycleControl", FireCycleTime);
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
                        FireState = false;
                        break;
                    }
                case 3:
                    {
                        Invoke("FireCycleControl", FireCycleTime);
                        if (Missile2[0] == null && Missile2[1] == null)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                Missile2[i] = MPool2.NewItem();
                            }
                            Missile2[0].transform.position = MissileLocation1.transform.position;
                            Missile2[1].transform.position = MissileLocation4.transform.position;
                        }
                        FireState = false;
                        break;
                    }
                case 4:
                    {
                        Invoke("FireCycleControl", FireCycleTime);
                        if (Missile3[0] == null && Missile3[1] == null)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                Missile3[i] = MPool3.NewItem();
                            }
                            Missile3[0].transform.position = MissileLocation2.transform.position;
                            Missile3[1].transform.position = MissileLocation3.transform.position;
                        }
                        FireState = false;
                        break;
                    }
            }
        }
        
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
        FireState = true;
        ++Pattern;
        if(Pattern >= 5)
        {
            Pattern = 1;
        }
    }
}
