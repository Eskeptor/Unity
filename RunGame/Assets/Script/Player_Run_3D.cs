using UnityEngine;
using System.Collections;

public class Player_Run_3D : MonoBehaviour {
    public float CharactorSpeed = 3.0f;
    public float Acceleration = 10.0f;
    public float CharactorSpeedMin = 4.0f;
    public float CharactorSpeedMax = 8.0f;
    public float JumpMax = 3.0f;
    public float JumpReduceSpeed = 0.5f;
    public float Timer = 0.0f;

    public enum STATE
    {
        NONE = -1,
        RUN = 0,
        JUMP,
        FAIL,
        STATUS,
    };

    public STATE Current = STATE.NONE;
    public STATE Next = STATE.RUN;

    private bool is_Landed = false;
    private bool is_Colided = false;
    private bool is_KeyReleased = false;

	// Use this for initialization
	void Start()
    {
        this.Next = STATE.RUN;
        //this.Current = STATE.RUN;
    }
	// Update is called once per frame
	void Update () {
        //this.transform.Translate(new Vector3(CharactorSpeed * Time.deltaTime, 0.0f, 0.0f));
        this.Next = STATE.RUN;

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
            Vector3 cur_Pos = this.transform.position;
            Vector3 cur_Pos_Down = cur_Pos + Vector3.down * 1.0f;
            //RaycastHit2D hit;
            if (!Physics.Linecast(cur_Pos,cur_Pos_Down, 1 << LayerMask.NameToLayer("Block")))
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
        Vector3 velocity = this.GetComponent<Rigidbody>().velocity;
        // 착지한것인지 확인
        this.Land_Check();
        this.Timer += Time.deltaTime;

        if (this.Next == STATE.NONE)
        {
            if (this.Current == STATE.RUN)
            {
                if (!this.is_Landed)
                {
                    // 지금 달리고 있고 착지하지 않은 경우 아무것도 하지 않음
                }
                else
                {
                    if (Input.GetKey(KeyCode.Space))
                    {
                        // 달리는 도중에 "JUMP"키를 누르면 상태를 점프로
                        Debug.Log("Space!!");
                        // 점프 속도 계산
                        velocity.y = Mathf.Sqrt(2.0f * 9.8f * JumpMax);

                        // 점프 버튼에서 손이 떨어졌을때
                        this.is_KeyReleased = false;
                        this.Timer = 0.0f;
                        this.Next = STATE.JUMP;
                    }
                }
            }
            if (this.Current == STATE.JUMP)
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
            if (this.Current == STATE.JUMP)
            {
                // 점프 속도 계산
                velocity.y = Mathf.Sqrt(2.0f * 9.8f * JumpMax);

                // 점프 버튼에서 손이 떨어졌을때
                this.is_KeyReleased = false;
                this.Timer = 0.0f;
            }
        }
        if (this.Current == STATE.RUN)
        {
            velocity.x += Acceleration * Time.deltaTime;
            // 속도제한걸기
            if (Mathf.Abs(velocity.x) > CharactorSpeedMax)
            {
                velocity.x *= CharactorSpeedMin / Mathf.Abs(this.GetComponent<Rigidbody>().velocity.x);
            }
        }
        if (this.Current == STATE.JUMP)
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
            } while (false);
        }
        this.GetComponent<Rigidbody>().velocity = velocity;
        return true;
    }
}
