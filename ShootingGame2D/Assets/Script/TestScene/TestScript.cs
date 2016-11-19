using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
    
    void Start()
    {
        Player_Data.HP = Player_Data.Init_HP = 90;
        Player_Data.FireRate = 0.3f;
        Player_Data.Type = 1;
        Player_Data.Score = 0;
    }
}
