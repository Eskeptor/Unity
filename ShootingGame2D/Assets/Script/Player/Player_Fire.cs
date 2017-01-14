using UnityEngine;
using System.Collections;

public class Player_Fire : MonoBehaviour {
    /* Public Objects */
    public int MissileMaximumPool = 20;             // 플레이어의 미사일 최대개수
    public Transform RightMissileLocation;          // 플레이어의 미사일 발사 지점(오른쪽)
    public Transform LeftMissileLocation;           // 플레이어의 미사일 발사 지점(왼쪽)
    public AudioSource MissileSound;                // 플레이어 미사일 발사 효과음
    public GameObject MissileObject;                // 플레이어 미사일 오브젝트

    /* Private Objects */
    private bool FireState;                         // 미사일 발사 속도 제어
    private bool DeadCheck;                         // 플레이어의 데스체크
    private MemoryPool MPool = new MemoryPool();    // 플레이어 미사일 메모리풀
    private GameObject[] MissileLeft;               // 플레이어의 왼쪽 미사일
    private GameObject[] MissileRight;              // 플레이어의 오른쪽 미사일

    void OnApplicationQuit ()
    {
        MPool.Dispose();
    }

	void Start ()
    {
        FireState = true;
        DeadCheck = IsDead();

        /* 미사일 생성 */
        MPool.Create(MissileObject, MissileMaximumPool);
        MissileLeft = new GameObject[MissileMaximumPool / 2];
        MissileRight = new GameObject[MissileMaximumPool / 2];

        MissileSound.Stop();
    }
	
	void Update () {
        DeadCheck = IsDead();
        if (!DeadCheck)
        {
            if (!MissileFire())
            {
                return;
            }
        }
    }

    bool MissileFire ()
    {
        if (FireState)
        {
            if (Input.GetButton("Fire1"))
            {
                // 미사일 발사 속도 제어
                StartCoroutine(FireCycleControl());

                // 미사일 메모리풀에서 냠냠
                for(int i = 0; i < MissileMaximumPool / 2; i++)
                {
                    if (MissileLeft[i] == null && MissileRight[i] == null)
                    {
                        MissileLeft[i] = MPool.NewItem();
                        MissileRight[i] = MPool.NewItem();
                        MissileLeft[i].transform.position = LeftMissileLocation.transform.position;
                        MissileRight[i].transform.position = RightMissileLocation.transform.position;
                        break;
                    }
                }
                MissileSound.Play();
            }
        }

        // 미사일 메모리풀로 되돌려보내기
        for(int i = 0; i < MissileMaximumPool / 2; i++)
        {
            if (MissileLeft[i])
            {
                if (MissileLeft[i].GetComponent<Collider2D>().enabled == false)
                {
                    MissileLeft[i].GetComponent<Collider2D>().enabled = true;
                    MPool.RemoveItem(MissileLeft[i]);
                    MissileLeft[i] = null;
                }
            }
            if (MissileRight[i])
            {
                if (MissileRight[i].GetComponent<Collider2D>().enabled == false)
                {
                    MissileRight[i].GetComponent<Collider2D>().enabled = true;
                    MPool.RemoveItem(MissileRight[i]);
                    MissileRight[i] = null;
                }
            }
        }
        return true;
    }

    // Dead Check from Player Gameobject
    bool IsDead ()
    {
        return GameObject.Find("Aircraft Body").GetComponent<Player_Move>().Death;
    }

    // Fire Cycle Control
    IEnumerator FireCycleControl()
    {
        FireState = false;
        yield return new WaitForSeconds(Player_Data.FireRate);
        FireState = true;
    }
}
