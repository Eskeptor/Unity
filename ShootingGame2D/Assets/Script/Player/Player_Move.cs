using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {
    /* Public Object */
    public float MoveSpeed = 3f;
    public GameObject Explosion;
    public GameObject EventSP;
    [HideInInspector]
    public bool Death;

    /* Private Object */
    private const float EnabledPosX = 3.05f;

	// Use this for initialization
	void Start () {
        Death = false;
        Explosion.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!Death)
        {
            Move();
            Move_Limit();
        }
        DeadCheck();
    }

    // Key Array
    void Move()
    {
        if (Input.GetButton("Horizontal"))
        {
            transform.Translate(Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime, 0f, 0f);
        }
        if (Input.GetButton("Vertical"))
        {
            transform.Translate(0f, Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime, 0f);
        }
    }

    // Limit Position
    void Move_Limit()
    {
        if (transform.position.x < -EnabledPosX)
        {
            transform.position = new Vector3(-EnabledPosX, transform.position.y, 0f);
        }
        if (transform.position.x > EnabledPosX)
        {
            transform.position = new Vector3(EnabledPosX, transform.position.y, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_ENEMY) || col.GetComponent<Collider2D>().CompareTag(Constant.TAG_BOSS))
        {
            Player_Data.HP -= 50;
            //Debug.Log("Player_Move : 적과 부딛힘");
        }
        if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_ENEMY_MISSILE))
        {
            Player_Data.HP -= (byte)col.GetComponent<Enemy_Missile>().Damage;
            //Debug.Log("Player_Move : 적 미사일과 부딛힘");
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }

    void DeadCheck()
    {
        if (Player_Data.HP <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            Death = true;
            Explosion.SetActive(true);
            Invoke("Dead", 2f);
        }
    }
}
