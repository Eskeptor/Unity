using UnityEngine;
using System.Collections;

public class Player_Run : MonoBehaviour {
    //public float CharactorSpeed = 3.0f;
    public float Acceleration = 8.0f;
    public float CharactorSpeedMin = 4.0f;
    public float CharactorSpeedMax = 8.0f;
    public float JumpMax = 300f;
    public float JumpReduceSpeed = 0.5f;
    public float Timer = 0.0f;

    public enum STATE
    {
        NONE = -1,
        RUN = 0,
        JUMP = 1,
        FAIL = 2,
        STATUS = 3,
    };

    public STATE Current = STATE.NONE;
    public STATE Next = STATE.NONE;

    private Rigidbody2D rd2d;
    private bool is_Landed = false;
    private bool is_Colided = false;
    private bool is_KeyReleased = false;
    private bool DoubleJump = false;
    private float Jump_Timer = -1.0f;
    private float Click_Attemp_Time = 0.5f;

	// Use this for initialization
	void Start()
    {
        this.Next = STATE.RUN;
        rd2d = GetComponent<Rigidbody2D>();
        //this.Current = STATE.RUN;
    }
	// Update is called once per frame
	void Update () {
        //this.transform.Translate(new Vector3(CharactorSpeed * Time.deltaTime, 0.0f, 0.0f));
        //this.Next = STATE.RUN;

        if (!Player_State())
        {
            return;
        }
        
	}
    private void Land_Check()
    {
        this.is_Landed = false;
        do
        {
            // 현재 위치
            Vector2 cur_Pos = this.transform.position;
            Vector2 cur_Pos_Down = cur_Pos + Vector2.down * 1.0f;
            //Vector3 cur_Pos = this.transform.position;
            //Vector3 cur_Pos_Down = cur_Pos + Vector3.down * 1.0f;
            //RaycastHit2D hit;
            if (!Physics2D.Linecast(cur_Pos,cur_Pos_Down, 1 << LayerMask.NameToLayer("Block")))
            {
                break;
            }
            if(this.Current == STATE.JUMP)
            {
                if(this.Timer < Time.deltaTime * 3.0f)
                {
                    break;
                }
            }
            this.is_Landed = true;
        } while (false);
    }
    bool Player_State()
    {
        // 속도 설정
        Vector2 velocity = rd2d.velocity;
        //Vector3 velocity = this.GetComponent<Rigidbody2D>().velocity;
        // 착지한것인지 확인
        this.Land_Check();
        this.Timer += Time.deltaTime;
        if(this.Current == STATE.RUN)
        {
            if(this.transform.position.y < -10)
            {
                this.Next = STATE.FAIL;
            }
        }
        else if(this.Current == STATE.JUMP)
        {
            if(this.transform.position.y < -10)
            {
                this.Next = STATE.FAIL;
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            this.Jump_Timer = 0.0f;
        }
        else
        {
            if (this.Jump_Timer >= 0.0f)
            {
                this.Jump_Timer += Time.deltaTime;
            }
        }

        if (this.Next == STATE.NONE)
        {
            if (this.Current == STATE.RUN)
            {
                if(0.0f <= this.Jump_Timer && this.Jump_Timer <= Click_Attemp_Time)
                {
                    if (this.is_Landed)
                    {
                        this.Jump_Timer = -1.0f;
                        this.Next = STATE.JUMP;
                    }
                }
            }
            else if (this.Current == STATE.JUMP)
            {
                if (this.is_Landed)
                {
                    // 점프해서 착지했다면 "RUN"으로 상태 변경
                    this.Next = STATE.RUN;
                }
            }
        }
        
        // 상태를 변화 시킬때
        while (this.Next != STATE.NONE)
        {
            this.Current = this.Next;
            this.Next = STATE.NONE;
            
            if ((this.Current == STATE.JUMP))
            {
                // 점프 속도 계산
                //rd2d.AddForce(new Vector2(0,JumpMax));
                DoubleJump = true;
                velocity.y += Mathf.Sqrt(2.0f * 9.8f * JumpMax);
                if(DoubleJump)
                {
                    //rd2d.AddForce(new Vector2(0, JumpMax));
                    DoubleJump = false;
                }
                // 점프 버튼에서 손이 떨어졌을때
                this.is_KeyReleased = false;
                
            }
            
            this.Timer = 0.0f;
        }
        if (this.Current == STATE.RUN)
        {
            velocity.x += Acceleration * Time.deltaTime;
            // 속도제한걸기
            if (Mathf.Abs(velocity.x) > CharactorSpeedMax)
            {
                velocity.x *= CharactorSpeedMax / Mathf.Abs(this.GetComponent<Rigidbody2D>().velocity.x);
                //velocity.y = 0.0001f;
            }
        }
        else if (this.Current == STATE.JUMP)
        {
            do
            {
                if (!Input.GetKey(KeyCode.Space))
                {
                    break;
                }
                if (this.is_KeyReleased)
                {
                    break;
                }
                if (velocity.y <= 0.0f)
                {
                    break;
                }
                velocity.y *= JumpReduceSpeed;
                this.is_KeyReleased = true;
            } while (false);
        }
        
        this.GetComponent<Rigidbody2D>().velocity = velocity;
        return true;
    }
    public bool is_End()
    {
        bool mes = false;
        if(this.Current == STATE.FAIL)
        {
            mes = true;
            Debug.Log("죽음");
        }
        return mes;
    }
}
