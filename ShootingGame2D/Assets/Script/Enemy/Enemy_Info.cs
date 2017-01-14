using UnityEngine;

public class Enemy_Info : MonoBehaviour
{
    public int Score = 10;      // 해당 적을 죽일시 얻는 점수
    public int HP = 10;         // 해당 적의 체력
    public byte Type = 1;       // 해당 적의 타입
    public float FireRate = 1f; // 해당 적의 미사일발사 속도
    public int Damage = 3;      // 해당 적의 공격력
    public bool FireEnabled = false;
    public bool FireState = true;
    public bool ScoreCheck = false;
}
