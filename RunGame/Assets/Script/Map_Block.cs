using UnityEngine;
using System.Collections;

public class Map_Block : MonoBehaviour {
    public GameObject[] BlockPrefab;
    //public int MaxBlock = 10;
    //GameObject[] Block1 = new GameObject[10];
    //GameObject[] Block2 = new GameObject[10];
    private int Block_Create_Counter = 0;
    /*
    void Start()
    {
       
    }
    */
	public void Block_Create(Vector3 Block_Create_Position)
    {
        int Next_Block_Type = this.Block_Create_Counter % this.BlockPrefab.Length;
        GameObject go = GameObject.Instantiate(this.BlockPrefab[Next_Block_Type]) as GameObject;
        go.transform.position = Block_Create_Position;
        this.Block_Create_Counter++;
        /*
        for (int i = 0; i < MaxBlock; i++)
        {
            Block1[i] = ObjectPoolManager.Instance.Create("Block1", Block_Create_Position, Block_Create_Rotation);
            Block2[i] = ObjectPoolManager.Instance.Create("Block2", Bl)

        }*/
    }
}
