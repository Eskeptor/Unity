using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Level_Data
{
    public struct Range
    {
        public int Min;
        public int Max;
    };
    public float EndTime;
    public float PlayerSpeed;
    public Range Floor_Cnt;
    public Range Hole_Cnt;
    public Range Height_UD;

    public Level_Data()
    {
        this.EndTime = 15f;
        this.PlayerSpeed = 5f;
        this.Floor_Cnt.Max = 10;
        this.Floor_Cnt.Min = 100;
        this.Hole_Cnt.Max = 2;
        this.Hole_Cnt.Min = 6;
        this.Height_UD.Max = 0;
        this.Height_UD.Min = 0;
    }
}

public class Map_Level : MonoBehaviour {
    public enum Block_Type
    {
        NONE = -1,
        FLOOR = 0,
        HOLE = 1,
        NUM = 2,
        OBSTACLE = 3,
        JUMPTOKEN = 4,
    }
    public struct BlockInfo
    {
        public Block_Type B_Type;
        public int Max_Cnt;
        public int Height;
        public int Current_Cnt;
    };
    public int Block_cnt = 0;
    public int Lvl = 0;
    public BlockInfo Prev_Block;
    public BlockInfo Cur_Block;
    public BlockInfo Nex_Block;
    public int Height_Max = 10;
    public int Height_Min = -3;

    private List<Level_Data> Lvl_Data_List = new List<Level_Data>();
    
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    public void Block_Update (float time) {
        this.Cur_Block.Current_Cnt++;
        // 블록 개수가 최대이상이면(Max_Cnt)
        if(this.Cur_Block.Current_Cnt >= this.Cur_Block.Max_Cnt)
        {
            // 블록 떠넘기기
            this.Prev_Block = this.Cur_Block;
            this.Cur_Block = this.Nex_Block;
            // 초기화
            this.Clr_Nex_Block(ref this.Nex_Block);
            // 다음 블록 설정
            this.Lvl_Update(ref this.Nex_Block, this.Cur_Block, time);
            //this.Lvl_Update(ref this.Nex_Block, this.Cur_Block);
        }
        // 블록 개수 증가
        this.Block_cnt++;
    }
    private void Clr_Nex_Block(ref BlockInfo b)
    {
        b.B_Type = Block_Type.FLOOR;
        b.Max_Cnt = 15;
        b.Height = 0;
        b.Current_Cnt = 0;
    }
    public void init()
    {
        this.Block_cnt = 0;
        this.Clr_Nex_Block(ref this.Prev_Block);
        this.Clr_Nex_Block(ref this.Cur_Block);
        this.Clr_Nex_Block(ref this.Nex_Block);
    }
    private void Lvl_Update(ref BlockInfo cur, BlockInfo prev, float time)
    {
        //int lvl = 0;
        float time_Local = Mathf.Repeat(time, this.Lvl_Data_List[this.Lvl_Data_List.Count - 1].EndTime);

        int i;
        for(i = 0; i < this.Lvl_Data_List.Count - 1; i++)
        {
            if (time_Local <= this.Lvl_Data_List[i].EndTime)
            {
                break;
            }
        }
        this.Lvl = i;
        //Debug.Log("Lvl = i => " + i);
        cur.B_Type = Block_Type.FLOOR;
        cur.Max_Cnt = 1;
        if(this.Block_cnt >= 10)
        {
            // 현재 레벨 데이터 가져오기
            Level_Data lvl_data;
            lvl_data = this.Lvl_Data_List[this.Lvl];
            
            if (prev.B_Type == Block_Type.FLOOR)
            {
                // 다음 블록은 구멍
                cur.B_Type = Block_Type.HOLE;
                cur.Max_Cnt = Random.Range(lvl_data.Hole_Cnt.Min, lvl_data.Hole_Cnt.Max);
                cur.Height = prev.Height;
            }
            // 현재 블록이 구멍이면
            else if (prev.B_Type == Block_Type.HOLE)
            {
                // 다음 블록은 바닥
                cur.B_Type = Block_Type.FLOOR;
                cur.Max_Cnt = Random.Range(lvl_data.Floor_Cnt.Min, lvl_data.Floor_Cnt.Max);
                int Height_min = prev.Height + lvl_data.Height_UD.Min;
                int Height_max = prev.Height + lvl_data.Height_UD.Max;
                Height_min = Mathf.Clamp(Height_min, Height_Min, Height_Max);
                Height_max = Mathf.Clamp(Height_max, Height_Min, Height_Max);

                cur.Height = Random.Range(Height_min, Height_max);
            }

        }
        /*
        // 현재 블록이 바닥이면
        if (prev.B_Type == Block_Type.FLOOR)
        {
            // 다음 블록은 구멍
            cur.B_Type = Block_Type.HOLE;
            cur.Max_Cnt = 4;
            cur.Height = prev.Height;
        }
        // 현재 블록이 구멍이면
        else if(prev.B_Type == Block_Type.HOLE)
        {
            // 다음 블록은 바닥
            cur.B_Type = Block_Type.FLOOR;
            cur.Max_Cnt = 11;
        }*/
    }
    public void Lvl_Data_Load(TextAsset Level_Edit)
    {
        string lv = Level_Edit.text;
        string[] line = lv.Split('\n');

        for(int i = 0; i < line.Length; i++)
        {
            if (line[i] == "")
            {
                continue;
            }
            Debug.Log(line[i]);
            string[] words = line[i].Split();
            int num = 0;
            Level_Data Lvl_Data = new Level_Data();
            for(int t = 0; t < words.Length; t++)
            {
                if (words[t].StartsWith("#"))
                {
                    break;
                }
                if(words[t] == "")
                {
                    continue;
                }
                if (num == 0)
                {
                    Lvl_Data.EndTime = float.Parse(words[t]);
                } else if (num == 1)
                {
                    Lvl_Data.PlayerSpeed = float.Parse(words[t]);
                } else if (num == 2)
                {
                    Lvl_Data.Floor_Cnt.Max = int.Parse(words[t]);
                } else if (num == 3)
                {
                    Lvl_Data.Floor_Cnt.Min = int.Parse(words[t]);
                } else if (num == 4)
                {
                    Lvl_Data.Hole_Cnt.Max = int.Parse(words[t]);
                } else if (num == 5)
                {
                    Lvl_Data.Hole_Cnt.Min = int.Parse(words[t]);
                } else if (num == 6)
                {
                    Lvl_Data.Height_UD.Max = int.Parse(words[t]);
                } else if (num == 7)
                {
                    Lvl_Data.Height_UD.Min = int.Parse(words[t]);
                }
                num++;
                Debug.Log(words[t]);
            }
            if (num >= 8)
            {
                this.Lvl_Data_List.Add(Lvl_Data);
            }
            else
            {
                if (num == 0)
                {
                    Debug.Log("정상 작동");
                }
                else
                {
                    Debug.LogError("Lvl_Data 파라미터 벗어남");
                }
            }
        }
        if(this.Lvl_Data_List.Count == 0)
        {
            Debug.LogError("Lvl_Data_List에 값이 하나도 없음");
            this.Lvl_Data_List.Add(new Level_Data());
        }
    }
}
