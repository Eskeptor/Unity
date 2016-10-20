using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player_Move : MonoBehaviour {
    public float CharacterMoveSpeed = 0.5f;
    public float MoveMaxXpos = 8.2f;
    public float MoveMaxZpos = 8.5f;
    public float MoveMinZpos = -0.6f;
    public int HP = 100;
    public int Damage = 15;
    public ParticleSystem DeathParticle = null;
    public Start_Event HpManager = null;


    void Update () {
        
        if (!MoveCharacter())
        {
            return;
        }
        if (!MoveCondition())
        {
            return;
        }
        if(HP == 0)
        {
            Instantiate(DeathParticle, this.transform.position, this.transform.rotation);
            this.GetComponent<Collider>().enabled = false;
            this.gameObject.SetActive(false);
            Debug.Log("게임오버");
        }
	}
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Enemy" || col.collider.tag == "Enemy Missile")
        {
            HP -= Damage;
            if (HP <= 0)
            {
                HP = 0;
            }
            HpManager.HP_Manager(HP);
            Debug.Log("Player_Move.cs : " + Damage + "타격");
        }
    }
    bool MoveCharacter()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(new Vector3(0, 0, 1) * CharacterMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(new Vector3(0, 0, 1) * -1*  CharacterMoveSpeed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(new Vector3(1, 0, 0) * -1 * CharacterMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(new Vector3(1, 0, 0) * CharacterMoveSpeed * Time.deltaTime);
        }
        if(this.transform.position.x < -5f || this.transform.position.x > 5f)
        {
            this.transform.Translate(new Vector3(0, 0, 0));
        }
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        return true;
    }
    bool MoveCondition()
    {
        if(this.transform.position.x > MoveMaxXpos)
        {
            this.transform.position = new Vector3(MoveMaxXpos, this.transform.position.y, this.transform.position.z);
            //Debug.Log("Player_Move : X좌표 " + MoveMaxXpos);
        }
        if (this.transform.position.x < -MoveMaxXpos)
        {
            this.transform.position = new Vector3(-MoveMaxXpos, this.transform.position.y, this.transform.position.z);
            //Debug.Log("Player_Move : X좌표 " + -MoveMaxXpos);
        }
        if(this.transform.position.z > MoveMaxZpos)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, MoveMaxZpos);
            //Debug.Log("Player_Move : Z좌표 " + MoveMaxZpos);
        }
        if(this.transform.position.z < MoveMinZpos)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, MoveMinZpos);
            //Debug.Log("Player_Move : Z좌표 " + MoveMinZpos);
        }
        return true;
    }
}
/* 모바일 버전
Rigidbody Player;
void Start()
{
    Player = this.GetComponent<Rigidbody>();
}

void Update()
{
    Vector3 moveVec = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical")) * CharacterMoveSpeed;
    bool isFire = CrossPlatformInputManager.GetButton("FireButton");
    Debug.Log(isFire ? "발사됨" : "발사안됨");
    //Player.AddForce(moveVec);
    this.transform.Translate(moveVec);
}
*/
