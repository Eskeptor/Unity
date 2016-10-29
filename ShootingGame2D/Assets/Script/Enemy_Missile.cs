using UnityEngine;
using System.Collections;

public class Enemy_Missile : MonoBehaviour {
    public float MoveSpeed = 8f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        transform.Translate(0f, MoveSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().tag == "Player")
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Enemy_Missile : Player와 부딛힘");
        }
        if (col.GetComponent<Collider2D>().tag == "DownShift")
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Enemy_Missile : 바닥과 부딛힘");
        }
    }
}
