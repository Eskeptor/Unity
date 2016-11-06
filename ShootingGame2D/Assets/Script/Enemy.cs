using UnityEngine;

public class Enemy : MonoBehaviour {
    public GameObject Explosion;
    public GameObject MissileObject;
    public Transform MissileLocation;
    public int MissileMaximumPool = 5;
    public float FireCycleTime = 1f;

    private bool FireEnabled;   // By measuring the distance to fire a missile
    private bool FireState;     // for Fire cycle control
    private bool Score;
    private GameObject Player;
    private GameObject EventSP;
    private GameObject DownShift;
    MemoryPool MPool = new MemoryPool();
    private GameObject[] Missile;

    // When application quit, Memory clear
    void OnApplicationQuit()
    {
        MPool.Dispose();
    }

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Aircraft Body");
        EventSP = GameObject.Find("ScoreHP Event");
        DownShift = GameObject.Find("DownShift");

        // Create Missile
        MPool.Create(MissileObject, MissileMaximumPool);
        Missile = new GameObject[MissileMaximumPool];
        for (int i = 0; i < MissileMaximumPool; i++)
        {
            Missile[i] = null;
        }

        FireState = true;
        FireEnabled = false;
        Score = true;
        GetComponent<AudioSource>().Stop();
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
            if (GetComponent<Enemy_Info>().HP < 0)
            {
                GetComponent<Enemy_Info>().HP = 0;
            }
            //Debug.Log("Enemy_Move : 플레이어와 부딛힘");
        }
        else if (col.GetComponent<Collider2D>().tag == "Missile")
        {
            GetComponent<Enemy_Info>().HP -= Player.GetComponent<Player_Fire>().Damage;
            if (GetComponent<Enemy_Info>().HP < 0)
            {
                GetComponent<Enemy_Info>().HP = 0;
            }
            //Debug.Log("Enemy_Move : 미사일과 부딛힘");
        }
    }

    // When enemy is down, deactivate object(not destroy)
    void Dead()
    {
        for (int i = 0; i < MissileMaximumPool; i++)
        {
            if (Missile[i])
            {
                if(Missile[i].GetComponent<Collider2D>().enabled == false)
                {
                    Missile[i].GetComponent<Collider2D>().enabled = true;
                    MPool.RemoveItem(Missile[i]);
                    Missile[i] = null;
                }
            }
        }
        gameObject.SetActive(false);
    }

    // Dead checker
    void IsDead()
    {
        if (GetComponent<Enemy_Info>().HP == 0)
        {
            if (Score)
            {
                FireEnabled = false;
                GetComponent<AudioSource>().Play();
                EventSP.GetComponent<Event_ScoreHP>().AddScore(GetComponent<Enemy_Info>().Score);
                Score = false;
            }
            Explosion.SetActive(true);
            Invoke("Dead", 1f);
        }
    }

    // Distance check between player and enemy
    void DistanceChecker()
    {
        if(transform.position.y-DownShift.transform.position.y < Constant.MAX_YPOS_UP)
        {
            if(GetComponent<Enemy_Info>().HP != 0)
            {
                FireEnabled = true;
            }
        }
        if (Player_Data.HP == 0)
        {
            FireEnabled = false;
        }
    }

    // Missile fire
    void MissileFire()
    {
        if (FireState)
        {
            Invoke("FireCycleControl", FireCycleTime);
            for(int i = 0; i < MissileMaximumPool; i++)
            {
                if (Missile[i] == null)
                {
                    Missile[i] = MPool.NewItem();
                    Missile[i].transform.position = MissileLocation.transform.position;
                    break;
                }
            }
            FireState = false;
        }
        for(int i = 0; i < MissileMaximumPool; i++)
        {
            if (Missile[i])
            {
                if(Missile[i].GetComponent<Collider2D>().enabled == false)
                {
                    Missile[i].GetComponent<Collider2D>().enabled = true;
                    MPool.RemoveItem(Missile[i]);
                    Missile[i] = null;
                }
            }
        }
    }

    // Fire Cycle Control
    private void FireCycleControl()
    {
        FireState = true;
    }
}
