using UnityEngine;
using System.Collections;

public class Enemy_Collision : MonoBehaviour {
    public float EnemySpeed = 0.2f;
    public float MissileFireSpeed = 0.5f;
    public int MissileCount = 8;
    public float DestroyMissileZpos = -1.8f;
    public GameObject EnemyMissile = null;
    public GameObject EnemyFireLocation = null;
    

    MemoryPool missile_pool = new MemoryPool();
    GameObject[] enemy_missile;
    bool Missile_Fire_State = true;

    void OnApplicationQuit()
    {
        missile_pool.Dispose();
    }
    void Start()
    {
        missile_pool.Create(EnemyMissile, MissileCount);
        enemy_missile = new GameObject[MissileCount];
        for(int i = 0; i < enemy_missile.Length; i++)
        {
            enemy_missile[i] = null;
        }
    }

    void Update()
    {
        // 미사일이 생성되자마자 MissileSpeed 속도로 날라감
        this.transform.Translate(new Vector3(0, 0, -1) * EnemySpeed * Time.deltaTime);
        if (Missile_Fire_State)
        {
            Invoke("Missile_Fire_Cur", MissileFireSpeed);
            for(int i = 0; i < enemy_missile.Length; i++)
            {
                if(enemy_missile[i] == null)
                {
                    enemy_missile[i] = missile_pool.NewItem();
                    enemy_missile[i].transform.position = EnemyFireLocation.transform.position;
                    break;
                }
            }
            Missile_Fire_State = false;
        }
        for(int i = 0; i < enemy_missile.Length; i++)
        {
            if (enemy_missile[i])
            {
                if(enemy_missile[i].transform.position.z < DestroyMissileZpos)
                {
                    missile_pool.RemoveItem(enemy_missile[i]);
                    enemy_missile[i] = null;
                }
                else if(enemy_missile[i].GetComponent<Collider>().enabled == false)
                {
                    enemy_missile[i].GetComponent<Collider>().enabled = true;
                    missile_pool.RemoveItem(enemy_missile[i]);
                    enemy_missile[i] = null;
                }
            }

        }
    }
    void Missile_Fire_Cur()
    {
        Missile_Fire_State = true;
    }
    void OnCollisionEnter(Collision col)
    {
        // 부딛힌 상대의 태그가 "Enemy"일때
        if (col.collider.tag == "Missile")
        {
            // 미사일의 콜라이더를 false처리 한다.
            // (false처리가 됨으로 "Attack_Missile.cs"에서 메모리로 다시 불러들임
            this.GetComponent<Collider>().enabled = false;
            Debug.Log("Enemy_Collision.cs : 미사일과 충돌함");
        }
        if(col.collider.tag == "Player")
        {
            this.GetComponent<Collider>().enabled = false;
            Debug.Log("Enemy_Collision.cs : 플레이어와 충돌함");
        }
    }
}
