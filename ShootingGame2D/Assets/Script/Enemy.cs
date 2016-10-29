using UnityEngine;

// Constant Class
static class Constant
{
    public const float MAX_XPOS = 3.33f;
    public const float MAX_YPOS_UP = 8.28f;
    public const float MAX_YPOS_DOWN = -2f;
}

public class Enemy : MonoBehaviour {
    public GameObject Explosion;
    public GameObject MissileObject;
    public Transform MissileLocation;
    public int MissileMaximumPool = 5;
    public float FireCycleTime = 1f;

    private bool FireEnabled;   // By measuring the distance to fire a missile
    private bool FireState;     // for Fire cycle control
    private GameObject Player;
    private GameObject EventSP;
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

        // Create Missile
        MPool.Create(MissileObject, MissileMaximumPool);
        Missile = new GameObject[MissileMaximumPool];
        for (int i = 0; i < MissileMaximumPool; i++)
        {
            Missile[i] = null;
        }

        FireState = true;
        FireEnabled = false;
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
            EventSP.GetComponent<Event_ScoreHP>().AddScore(GetComponent<Enemy_Info>().Score);
            GetComponent<Enemy_Info>().HP -= Player.GetComponent<Player_Fire>().Damage;
            EventSP.GetComponent<Event_ScoreHP>().MinHP(50);
            Debug.Log("Enemy_Move : 플레이어와 부딛힘");
        }
        else if (col.GetComponent<Collider2D>().tag == "Missile")
        {
            EventSP.GetComponent<Event_ScoreHP>().AddScore(GetComponent<Enemy_Info>().Score);
            GetComponent<Enemy_Info>().HP -= Player.GetComponent<Player_Fire>().Damage;
            Debug.Log("Enemy_Move : 미사일과 부딛힘");
        }
    }

    // When enemy is down, deactivate object(not destroy)
    void Dead()
    {
        gameObject.SetActive(false);
    }

    // Dead checker
    void IsDead()
    {
        if (GetComponent<Enemy_Info>().HP <= 0)
        {
            
            Explosion.SetActive(true);
            Invoke("Dead", 1f);
        }
    }

    // Distance check between player and enemy
    void DistanceChecker()
    {
        if(transform.position.y-Player.transform.position.y < Constant.MAX_YPOS_UP)
        {
            FireEnabled = true;
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
