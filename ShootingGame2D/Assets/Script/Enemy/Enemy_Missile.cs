using UnityEngine;
using System.Collections;

/* Missile Type List
 * 1 : Default Missile(Player Transform-based)
 * 2 : Straight Missile
 * 3 : Guided Missile
 */

public class Enemy_Missile : MonoBehaviour {
    public float MoveSpeed = 8f;
    public int MissileType = 1;
    public int Damage = 30;

    private Vector3 GoalPos;
    private float Angle;
    private bool Locked;

    // Use this for initialization
    void Start () {
        Locked = true;
    }
	
	// Update is called once per frame
	void Update () {
        if(MissileType == 1)
        {
            Move();
            LockedPos();
        }
        else if(MissileType == 2)
        {
            Invoke("DelayDestroy", 1f);
        }
        else if(MissileType == 3)
        {

        }
	}

    void Move()
    {
        transform.Translate(GoalPos * MoveSpeed * Time.deltaTime);
        transform.Find("Enemy Missile Image").rotation = Quaternion.Euler(0, 0, Angle + 90);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_PLAYER))
        {
            //Debug.Log("Enemy_Missile : Player와 부딛힘");
            if(MissileType == 1)
            {
                GetComponent<Collider2D>().enabled = false;
                Locked = true;
            }
        }
        if (col.GetComponent<Collider2D>().CompareTag("DownShift"))
        {
            //Debug.Log("Enemy_Missile : 바닥과 부딛힘");
            if (MissileType == 1)
            {
                GetComponent<Collider2D>().enabled = false;
                Locked = true;
            }
        }
    }

    void LockedPos()
    {
        if (GetComponent<Collider2D>().enabled)
        {
            if (Locked)
            {
                GoalPos = -(GameObject.Find("Aircraft Body").GetComponent<Transform>().transform.position - gameObject.transform.position).normalized;
                Angle = Mathf.Atan2(GoalPos.y, GoalPos.x) * Mathf.Rad2Deg;
                Locked = false;
            }
        }
    }

    void DelayDestroy()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}
