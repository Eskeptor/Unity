using UnityEngine;
using System.Collections;

public class Map_Create_3D : MonoBehaviour
{
    public static float BLOCK_WIDTH = 1.0f;
    public static float BLOCK_HEIGHT = 0.2f;
    public static int BLOCK_IN_SCREEN = 20;

    private FloorBlock Last_Block;
    public Player_Run_3D Player;
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
        this.Player = go.GetComponent<Player_Run_3D>();
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
        this.Block_Creating.Block_Create(Block_Create_Position);
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
