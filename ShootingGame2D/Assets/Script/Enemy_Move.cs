using UnityEngine;
using System.Collections;

public class Enemy_Move : MonoBehaviour {
    public float MoveSpeed = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    // Enemy move
    void Move()
    {
        transform.Translate(0f, MoveSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().tag == "Player")
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Enemy_Move : 플레이어와 부딛힘");
        }
        else if (col.GetComponent<Collider2D>().tag == "Missile")
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Enemy_Move : 미사일과 부딛힘");
        }
    }
}
