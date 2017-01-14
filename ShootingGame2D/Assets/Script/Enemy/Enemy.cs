using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /* Public Object */
    public GameObject Explosion;        // 폭팔효과 오브젝트(플레이어에게 죽었을 때)
    public GameObject MissileObject;    // 미사일 오브젝트
    public Transform MissileLocation;   // 미사일이 발사될 지점
    public int MissileMaximumPool = 5;  // 미사일 오브젝트의 최대 개수
    public float FireRateTime = 1f;     // 미사일이 발사되는 속도

    /* Private Object */
    private GameObject EventSP;                     // Event_ScoreHP와 연결
    private GameObject DownShift;                   // 플레이어에 붙어있는 "DownShift"와 연결
    private MemoryPool MPool = new MemoryPool();    // 미사일 오브젝트의 메모리풀
    private GameObject[] Missile;                   // 미사일 오브젝트를 생성할 배열

    /* 어플리케이션이 종료될 때 자동으로 실행 */
    void OnApplicationQuit()
    {
        MPool.Dispose();
    }

    void Start ()
    {
        EventSP = GameObject.Find("Player Event");
        DownShift = GameObject.Find("DownShift");

        MPool.Create(MissileObject, MissileMaximumPool);    // 미사일 오브젝트를 메모리풀에 등록
        Missile = new GameObject[MissileMaximumPool];       // 미사일 오브젝트를 배열에 생성  

        GetComponent<Enemy_Info>().FireState = true;
        GetComponent<Enemy_Info>().FireEnabled = false;
        GetComponent<Enemy_Info>().ScoreCheck = false;
        GetComponent<AudioSource>().Stop();
    }
	
	void Update ()
    {
        IsDead();
        DistanceChecker();
        if (GetComponent<Enemy_Info>().FireEnabled)
        {
            MissileFire();
        }
	}

    /* 충돌 체크를 위한 함수 */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_PLAYER))
        {            
            GetComponent<Enemy_Info>().HP -= Player_Data.Damage;
            if (GetComponent<Enemy_Info>().HP < 0)
            {
                GetComponent<Enemy_Info>().HP = 0;
            }
        }
        else if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_PLAYER_MISSILE))
        {
            GetComponent<Enemy_Info>().HP -= Player_Data.Damage;
            if (GetComponent<Enemy_Info>().HP < 0)
            {
                GetComponent<Enemy_Info>().HP = 0;
            }
        }
    }

    /* 죽었는가 안죽었는가 자기 자신을 체크 */
    void IsDead()
    {
        if (GetComponent<Enemy_Info>().HP == 0)
        {
            if (!GetComponent<Enemy_Info>().ScoreCheck)
            {
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Enemy_Info>().FireEnabled = false;
                GetComponent<AudioSource>().Play();
                EventSP.GetComponent<Event_ScoreHP>().AddScore(GetComponent<Enemy_Info>().Score);
                GetComponent<Enemy_Info>().ScoreCheck = true;
            }
            Explosion.SetActive(true);
            StartCoroutine(Dead(1f));
        }
    }

    /* 플레이어와 적(자기자신)과의 거리를 측정 */
    void DistanceChecker()
    {
        if (transform.position.y - DownShift.transform.position.y < Constant.RECOGNIZED_PLAYER)
        {
            if(GetComponent<Enemy_Info>().HP != 0 && Player_Data.HP != 0)
            {
                GetComponent<Enemy_Info>().FireEnabled = true;
            }
            else
            {
                GetComponent<Enemy_Info>().FireEnabled = false;
            }
        }
        if (transform.position.y - DownShift.transform.position.y == Constant.DISTROY_DISTANCE_Y || transform.position.x - DownShift.transform.position.x >= Constant.DISTROY_DISTANCE_X || transform.position.x - DownShift.transform.position.x <= -Constant.DISTROY_DISTANCE_X)
        {
            StartCoroutine(Dead(0f));
        }
    }

    /* 미사일 발사 */
    void MissileFire()
    {
        if (GetComponent<Enemy_Info>().FireState)
        {
            StartCoroutine(FireCycleControl());
            for(int i = 0; i < MissileMaximumPool; i++)
            {
                if (Missile[i] == null)
                {
                    Missile[i] = MPool.NewItem();
                    Missile[i].GetComponent<Enemy_Missile>().Damage = GetComponent<Enemy_Info>().Damage;
                    Missile[i].transform.position = MissileLocation.transform.position;
                    
                    break;
                }
            }
        }

        /* 미사일을 다시 메모리풀로 돌려보냄 */
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

    /* 미사일 발사 제어를 위한 함수 */
    IEnumerator FireCycleControl()
    {
        GetComponent<Enemy_Info>().FireState = false;
        yield return new WaitForSeconds(FireRateTime);
        GetComponent<Enemy_Info>().FireState = true;
    }

    /* 적(자기자신)이 죽었을 때, 비활성화 */
    IEnumerator Dead(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < MissileMaximumPool; i++)
        {
            if (Missile[i])
            {
                if (Missile[i].GetComponent<Collider2D>().enabled == false)
                {
                    Missile[i].GetComponent<Collider2D>().enabled = true;
                    MPool.RemoveItem(Missile[i]);
                    Missile[i] = null;
                }
            }
        }
        gameObject.SetActive(false);
    }
}
