using UnityEngine;
using System.Collections;

public class Map_Create : MonoBehaviour
{
    public static float BLOCK_WIDTH = 1.0f;
    public static float BLOCK_HEIGHT = 0.2f;
    public static int BLOCK_IN_SCREEN = 20;
    public TextAsset Level_Edit;

    private GameLogic Game_Logic;
    private FloorBlock Last_Block;
    private Map_Level Map_Lvl;
    public Player_Run Player;
    public Map_Block Block_Creating;

    private struct FloorBlock
    {
        public bool is_Create;
        public Vector3 Block_Now_Position;
    };

    // Use this for initialization
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        this.Player = go.GetComponent<Player_Run>();

        this.Last_Block.is_Create = false;
        this.Block_Creating = this.gameObject.GetComponent<Map_Block>();
        this.Map_Lvl = new Map_Level();
        this.Map_Lvl.init();
        this.Map_Lvl.Lvl_Data_Load(this.Level_Edit);
        this.Game_Logic = this.gameObject.GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        float Get_Block_X = this.Player.transform.position.x;
        Get_Block_X += BLOCK_WIDTH * ((float)BLOCK_IN_SCREEN + 1) / 2.0f;
        while (this.Last_Block.Block_Now_Position.x < Get_Block_X)
        {
            this.Create();
        }
    }
    private void Create()
    {
        Vector3 Block_Create_Position;
        //Transform Block_Create_Rotation;
        if (!this.Last_Block.is_Create) //마지막 블록이 생성되지 않은 경우
        {
            Block_Create_Position = this.Player.transform.position;
            Block_Create_Position.x -= BLOCK_WIDTH * ((float)BLOCK_IN_SCREEN / 2.0f);
            Block_Create_Position.y = 0.0f;
        }
        else //마지막 블록이 생성된경우
        {
            Block_Create_Position = this.Last_Block.Block_Now_Position;
        }
        Block_Create_Position.x += BLOCK_WIDTH;
        //Map_Block.cs 로 넘겨줌(생성지시)
        //this.Block_Creating.Block_Create(Block_Create_Position);

        // 레벨 갱신
        this.Map_Lvl.Block_Update(this.Game_Logic.getPlayTime());
        //this.Map_Lvl.Block_Update();
        // 새로만드는 블록의 높이 지정
        Block_Create_Position.y = Map_Lvl.Cur_Block.Height * BLOCK_HEIGHT;
        // 새로만들 블록의 정보를 저장
        Map_Level.BlockInfo cur = this.Map_Lvl.Cur_Block;
        // 블록이 바닥이면 생성
        if(cur.B_Type == Map_Level.Block_Type.FLOOR)
        {
            this.Block_Creating.Block_Create(Block_Create_Position);
        }
        this.Last_Block.Block_Now_Position = Block_Create_Position;
        this.Last_Block.is_Create = true;

    }
    
    public bool isDelete(GameObject Block)
    {
        bool re = false;

        float Player_Limit_Left = this.Player.transform.position.x - BLOCK_WIDTH * ((float)BLOCK_IN_SCREEN / 2.0f);
        if(Block.transform.position.x < Player_Limit_Left)
        {
            re = true;
        }
        return re;
    }
    
}
