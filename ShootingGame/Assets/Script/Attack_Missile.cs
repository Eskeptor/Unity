using UnityEngine;

public class Attack_Missile : MonoBehaviour {
    public float FireSpeed = 0.2f;
    public float DestroyMissileZpos = 10f;
    public int MissilePoolCount = 20;
    public GameObject Missile = null;
    public Transform FireMissileRightLocation = null;
    public Transform FireMissileLeftLocation = null;
    public AudioSource MissileFire = null;

    private bool Gun_Fire_State = true;
    private bool Deadcheck;

    MemoryPool pool = new MemoryPool();

    GameObject[] missile_left;
    GameObject[] missile_right;

    /* 프로그램 종료시 메모리 비움 */
    void OnApplicationQuit()
    {
        pool.Dispose();
    }

    /* 프로그램 시작하자 마자 실행 */
    void Start()
    {
        // 미사일 생성
        pool.Create(Missile, MissilePoolCount);
        missile_left = new GameObject[MissilePoolCount];
        missile_right = new GameObject[MissilePoolCount];
        for (int i = 0; i < missile_left.Length; i++)
        {
            missile_left[i] = null;
            missile_right[i] = null;
        }
        Deadcheck = GameObject.Find("Player").GetComponent<Player_Move>().Deadcheck;
    }
    void Update () {
        Deadcheck = GameObject.Find("Player").GetComponent<Player_Move>().Deadcheck;

        if (!Deadcheck)
        {
            // 공격했을 때
            if (!Attack())
            {
                return;
            }
        }
	}

    /* 미사일 발사 속도 조절 */
    private void FireSpeedControl()
    {
        Gun_Fire_State = true;
    }

    /* 공격 했을 때 */
    bool Attack()
    {
        if (Gun_Fire_State)
        {
            if (Input.GetKey(KeyCode.A))
            {
                // FireSpeed에 맞춰 실행
                Invoke("FireSpeedControl", FireSpeed);
                // 미사일 활성화 및 발사(미리 생성해놓은 메모리 에서 가져옴)
                for (int i = 0; i < missile_left.Length; i++)
                {
                    if (missile_left[i] == null && missile_right[i] == null)
                    {
                        missile_right[i] = pool.NewItem();
                        missile_left[i] = pool.NewItem();
                        missile_right[i].transform.position = FireMissileRightLocation.transform.position;
                        missile_left[i].transform.position = FireMissileLeftLocation.transform.position;
                        break;
                    }
                }
                Debug.Log("Attack_Missile.cs : 총 발사됨");
                Gun_Fire_State = false;
                MissileFire.Play();
            }
        }

        /* 미사일 메모리로 다시 돌려보냄 */
        for (int i = 0; i < missile_left.Length; i++)
        {
            // 왼쪽 미사일이 활성화 되어 있을 시
            if (missile_left[i])
            {
                // 미사일이 DestoryMissileZpos 이상으로 움직였을 시 돌려보냄
                if(missile_left[i].transform.position.z > DestroyMissileZpos)
                {
                    pool.RemoveItem(missile_left[i]);
                    missile_left[i] = null;
                    Debug.Log("Attack_Missile.cs : 거리에서 벗어남");
                }
                // 미사일의 콜라이더가 비활성화 됬을시 돌려보냄(미사일이 적에게 부딛혔을 때)
                else if (missile_left[i].GetComponent<Collider>().enabled == false)
                {
                    Debug.Log("Attack_Missile.cs : 적과 충돌함"); 
                    missile_left[i].GetComponent<Collider>().enabled = true;
                    pool.RemoveItem(missile_left[i]);
                    missile_left[i] = null;
                }
            }
            // 오른쪽 미사일이 활성화 되어 있을 시
            if (missile_right[i])
            {
                // 미사일이 DestoryMissileZpos 이상으로 움직였을 시 돌려보냄
                if (missile_right[i].transform.position.z > DestroyMissileZpos)
                {
                    pool.RemoveItem(missile_right[i]);
                    missile_right[i] = null;
                    Debug.Log("Attack_Missile.cs : 거리에서 벗어남");
                }
                // 미사일의 콜라이더가 비활성화 됬을시 돌려보냄(미사일이 적에게 부딛혔을 때)
                else if (missile_right[i].GetComponent<Collider>().enabled == false)
                {
                    missile_right[i].GetComponent<Collider>().enabled = true;
                    pool.RemoveItem(missile_right[i]);
                    missile_right[i] = null;
                    Debug.Log("Attack_Missile.cs : 적과 충돌함");
                }
            }
        }
        return true;
    }
}
