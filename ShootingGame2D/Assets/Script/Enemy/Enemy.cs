using UnityEngine;

public class Enemy : MonoBehaviour {
    /* Public Object */
    public GameObject Explosion;        // Explosion object(When the enemy died)
    public GameObject MissileObject;    // Enemy's missile object
    public Transform MissileLocation;   // Enemy's missile fire location
    public int MissileMaximumPool = 5;  // Enemy's missile memory maximum pool
    public float FireRateTime = 1f;    // Enemy's missile fire rate time
    [HideInInspector]
    public int HP;
    [HideInInspector]
    public int Score;

    /* Private Object */
    private bool FireEnabled;                       // By measuring the distance to fire a missile
    private bool FireState;                         // for Fire cycle control
    private bool ScoreCheck;                        // Check whether boss gave the score
    private GameObject EventSP;                     // for Event_ScoreHP
    private GameObject DownShift;                   // for Player's DownShift
    private MemoryPool MPool = new MemoryPool();    // for Enemy's missile memory pool
    private GameObject[] Missile;                   // for Enemy's missile

    // When application quit, Memory clear
    void OnApplicationQuit()
    {
        MPool.Dispose();
    }

    // Use this for initialization
    void Start () {
        EventSP = GameObject.Find("Player Event");
        DownShift = GameObject.Find("DownShift");

        // Create Missile
        MPool.Create(MissileObject, MissileMaximumPool);
        Missile = new GameObject[MissileMaximumPool];

        FireState = true;
        FireEnabled = false;
        ScoreCheck = false;
        GetComponent<AudioSource>().Stop();
    }
	
	// Update is called once per frame
	void Update () {
        IsDead();
        DistanceChecker();
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
            if (GetComponent<Enemy_Info>().HP < 0)
            {
                GetComponent<Enemy_Info>().HP = 0;
            }
            //Debug.Log("Enemy_Move : 플레이어와 부딛힘");
        }
        else if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_PLAYER_MISSILE))
        {
            GetComponent<Enemy_Info>().HP -= Player_Data.Damage;
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
            if (!ScoreCheck)
            {
                FireEnabled = false;
                GetComponent<AudioSource>().Play();
                EventSP.GetComponent<Event_ScoreHP>().AddScore(GetComponent<Enemy_Info>().Score);
                ScoreCheck = true;
            }
            Explosion.SetActive(true);
            Invoke("Dead", 1f);
        }
    }

    // Distance check between player and enemy
    void DistanceChecker()
    {
        if (transform.position.y - DownShift.transform.position.y < Constant.RECOGNIZED_PLAYER)
        {
            if(GetComponent<Enemy_Info>().HP != 0 && Player_Data.HP != 0)
            {
                FireEnabled = true;
            }
            else
            {
                FireEnabled = false;
            }
        }
        if (transform.position.y - DownShift.transform.position.y == Constant.DISTROY_DISTANCE_Y || transform.position.x - DownShift.transform.position.x >= Constant.DISTROY_DISTANCE_X || transform.position.x - DownShift.transform.position.x <= -Constant.DISTROY_DISTANCE_X)
        {
            Dead();
        }
        //if (Player_Data.HP == 0)
        //{
        //    FireEnabled = false;
        //}
    }

    // Missile fire
    void MissileFire()
    {
        if (FireState)
        {
            Invoke("FireCycleControl", FireRateTime);
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

        // Returns missiles in memory pool
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
    }

    // Fire Cycle Control
    private void FireCycleControl()
    {
        FireState = true;
    }
}
