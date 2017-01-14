using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {
    /* Public Object */
    public float MoveSpeed = 3f;
    public GameObject Explosion;
    public GameObject PlayerEvent;
    public GameObject Down;
    [HideInInspector]
    public bool Death;

    /* Private Object */
    private const float EnabledPosX = 3.05f;

	void Start () {
        Death = false;
        Explosion.SetActive(false);
    }
	
	void Update () {
        if (!Death)
        {
            Move();
            Move_Limit();
        }
        if (Player_Data.Devmode == 1)
        {
            HiddenKey();
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

    void HiddenKey()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerEvent.GetComponent<Event_ScoreHP>().BossDeathCheck = true;
        }
    }

    // Limit Position
    void Move_Limit()
    {
        if (transform.position.x < -Constant.PLAYER_ENABLED_X)
        {
            transform.position = new Vector3(-Constant.PLAYER_ENABLED_X, transform.position.y, 0f);
        }
        if (transform.position.x > Constant.PLAYER_ENABLED_X)
        {
            transform.position = new Vector3(Constant.PLAYER_ENABLED_X, transform.position.y, 0f);
        }
        if (transform.position.y - Down.transform.position.y < Constant.PLAYER_ENABLED_Y_DOWN)
        {
            transform.position = new Vector3(transform.position.x, Down.transform.position.y + 1f, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_ENEMY) || col.GetComponent<Collider2D>().CompareTag(Constant.TAG_BOSS))
        {
            Player_Data.HP -= 5;
        }
        if (col.GetComponent<Collider2D>().CompareTag(Constant.TAG_ENEMY_MISSILE))
        {
            Player_Data.HP -= col.GetComponent<Enemy_Missile>().Damage;
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
