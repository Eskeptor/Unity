using UnityEngine;
using System.Collections;

/* Missile Type List
 * 1 : 일반 미사일(Player Transform-based)
 * 2 : 일직선 미사일
 * 3 : 유도 미사일
 * 4 : 근접 미사일(오직 보스용)(오른쪽 타입)
 * 5 : 근접 미사일(오직 보스용)(왼쪽 타입)
 * 6 : 자기중심 회전형 일반 미사일
 */

public class Enemy_Missile : MonoBehaviour
{
    public float MoveSpeed = 8f;
    public byte MissileType = 1;
    public int Damage = 3;

    private Vector3 GoalPos;
    private float Angle;
    private bool Locked;
    private Transform PlayerPos;
    private Vector3 AdjacencyPos;

    // Use this for initialization
    void Start ()
    {
        Locked = true;
        PlayerPos = GameObject.Find("Aircraft Body").GetComponent<Transform>();
        if (MissileType == 4)
        {
            AdjacencyPos = new Vector3(PlayerPos.transform.position.x + 2f, PlayerPos.transform.position.y);
        }
        else if(MissileType == 5)
        {
            AdjacencyPos = new Vector3(PlayerPos.transform.position.x - 2f, PlayerPos.transform.position.y);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(MissileType == 1)
        {
            Move();
            LockedPos();
        }
        else if(MissileType == 2)
        {
            StartCoroutine(Delay());
        }
        else if(MissileType == 3)
        {
            Move();
            LockedPos();
        }
        else if(MissileType == 4)
        {
            Move();
            LockedPos();
        }
        else if(MissileType == 5)
        {
            Move();
            LockedPos();
        }
        else if(MissileType == 6)
        {
            Move();
            LockedPos();
        }
	}

    void Move()
    {
        transform.Translate(GoalPos * MoveSpeed * Time.deltaTime);
        if (MissileType != 6) 
            transform.Find("Enemy Missile Image").rotation = Quaternion.Euler(0, 0, Angle - 90);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_PLAYER))
        {
            //Debug.Log("Enemy_Missile : Player와 부딛힘");
            if(MissileType != 2)
            {
                GetComponent<Collider2D>().enabled = false;
                Locked = true;
            }
        }
        if (col.GetComponent<Collider2D>().CompareTag("DownShift"))
        {
            //Debug.Log("Enemy_Missile : 바닥과 부딛힘");
            if (MissileType != 2)
            {
                GetComponent<Collider2D>().enabled = false;
                Locked = true;
            }
        }
    }

    void LockedPos()
    {
        switch (MissileType)
        {
            case 1:
                {
                    if (GetComponent<Collider2D>().enabled)
                    {
                        if (Locked)
                        {
                            GoalPos = -(PlayerPos.transform.position - gameObject.transform.position).normalized;
                            Angle = Mathf.Atan2(GoalPos.y, GoalPos.x) * Mathf.Rad2Deg;
                            Locked = false;
                        }
                    }
                    break;
                }
            case 3:
                {
                    if (GetComponent<Collider2D>().enabled)
                    {
                        if (Locked)
                        {
                            GoalPos = -(PlayerPos.transform.position - gameObject.transform.position).normalized;
                            Angle = Mathf.Atan2(GoalPos.y, GoalPos.x) * Mathf.Rad2Deg;
                            if (Mathf.Abs(PlayerPos.transform.position.y - transform.position.y) <= 3.5f)
                                Locked = false;
                        }
                    }
                    break;
                }
            case 4:
            case 5:
                {
                    if (GetComponent<Collider2D>().enabled)
                    {
                        if (Locked)
                        {
                            GoalPos = -(AdjacencyPos - gameObject.transform.position).normalized;
                            Angle = Mathf.Atan2(GoalPos.y, GoalPos.x) * Mathf.Rad2Deg;
                            Locked = false;
                        }
                    }
                    break;
                }
            case 6:
                {
                    if (GetComponent<Collider2D>().enabled)
                    {
                        if (Locked)
                        {
                            GoalPos = -(AdjacencyPos - gameObject.transform.position).normalized;
                            Locked = false;
                        }
                    }
                    break;
                }
        }
        
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = false;
    }
    void DelayDestroy()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
