using UnityEngine;
using System.Collections;

public class Enemy_Missile : MonoBehaviour {
    public float MoveSpeed = 8f;

    private Vector3 GoalPos;
    private bool Locked;

    // Use this for initialization
    void Start () {
        Locked = true;
        LockedPos();
    }
	
	// Update is called once per frame
	void Update () {
        LockedPos();
        Move();
	}

    void Move()
    {
        transform.Translate(GoalPos * MoveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().tag == "Player")
        {
            GetComponent<Collider2D>().enabled = false;
            //Debug.Log("Enemy_Missile : Player와 부딛힘");
            Locked = true;
        }
        if (col.GetComponent<Collider2D>().tag == "DownShift")
        {
            GetComponent<Collider2D>().enabled = false;
            //Debug.Log("Enemy_Missile : 바닥과 부딛힘");
            Locked = true;
        }
    }

    void LockedPos()
    {
        if (GetComponent<Collider2D>().enabled)
        {
            if (Locked)
            {
                GoalPos = -(GameObject.Find("Aircraft Body").GetComponent<Transform>().transform.position - gameObject.transform.position).normalized;
                Locked = false;
            }
        }
    }
}
