using System.Collections;
using UnityEngine;

public class Boss_Level4_RW : MonoBehaviour
{
    /* Public Object */
    public GameObject Explosion1;               // 폭팔이펙트용 오브젝트
    public GameObject MissileObject1;           // Boss4의 미사일
    public Transform MissileLocation1;          // Boss4의 미사일 발사위치
    public float FireRateTime = 0.2f;           // Boss4의 미사일 발사 속도
    public int Missile1Pool = 30;               // 미사일 풀 크기

    /* Private Object */
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

        GetComponent<Enemy_Info>().ScoreCheck = false;
        GetComponent<Enemy_Info>().FireState = true;
        GetComponent<Enemy_Info>().FireEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsDead();
        if (GetComponent<Enemy_Info>().ScoreCheck)
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

        Boss4.GetComponent<Boss_Level4>().RightDead = true;
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
                //Debug.Log("SemiPattern : " + SemiPattern);
                //Debug.Log("Pattern : " + Pattern);
                // 미사일 발사 속도 제어
                StartCoroutine(FireCycleControl());

                // 미사일 메모리풀에서 냠냠
                for (int i = 0; i < Missile1Pool; i++)
                {
                    if (Missile1[i] == null)
                    {
                        Missile1[i] = MPool1.NewItem();
                        Missile1[i].transform.position = MissileLocation1.transform.position;
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < Missile1Pool; i++) 
        {
            if(Missile1[i])
            {
                if (Missile1[i].GetComponent<Collider2D>().enabled == false) 
                {
                    Missile1[i].GetComponent<Collider2D>().enabled = true;
                }
            }
        }

        // Returns missiles in memory pool
        
    }

    IEnumerator FireCycleControl()
    {
        GetComponent<Enemy_Info>().FireState = false;
        yield return new WaitForSeconds(FireRateTime);
        GetComponent<Enemy_Info>().FireState = true;
    }
}