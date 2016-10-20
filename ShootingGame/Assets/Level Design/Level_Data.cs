using UnityEngine;
using System.Collections;

public class Level_Data{
    public int stage;
    public float missile_speed;
    public int enemy_count;
    public int enemy_max_count;

    public Level_Data()
    {
        this.stage = 1;
        this.missile_speed = 3;
        this.enemy_count = 5;
        this.enemy_max_count = 7;
    }
}
