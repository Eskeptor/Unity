using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {
    public float MoveSpeed = 3f;
    public GameObject Explosion = null;

    [HideInInspector]
    public bool DeadCheck;

    private const float EnabledPosX = 3.05f;
    private const float EnabledPosY_down = -0.76f;
    private const float EnabledPosY_up = 7.84f;

	// Use this for initialization
	void Start () {
        DeadCheck = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!DeadCheck)
        {
            if (!Move())
            {
                return;
            }
            if (!Move_Limit())
            {
                return;
            }
        }
	}

    // Key Array
    bool Move()
    {
        if (Input.GetButton("Horizontal"))
        {
            transform.Translate(Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime, 0f, 0f);
        }
        if (Input.GetButton("Vertical"))
        {
            transform.Translate(0f, Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime, 0f);
        }
        return true;
    }

    // Limit Position
    bool Move_Limit()
    {
        if (transform.position.x < -EnabledPosX)
        {
            transform.position = new Vector3(-EnabledPosX, transform.position.y, 0f);
        }
        if (transform.position.x > EnabledPosX)
        {
            transform.position = new Vector3(EnabledPosX, transform.position.y, 0f);
        }
        if (transform.position.y < EnabledPosY_down)
        {
            transform.position = new Vector3(transform.position.x, EnabledPosY_down, 0f);
        }
        if (transform.position.y > EnabledPosY_up)
        {
            transform.position = new Vector3(transform.position.x, EnabledPosY_up, 0f);
        }
        return true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().tag == "Enemy")
        {
            Debug.Log("Player_Move : 적과 부딛힘");
            DeadCheck = true;
            Explosion.SetActive(true);
            Invoke("Dead", 2f);
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
        
    }
}
