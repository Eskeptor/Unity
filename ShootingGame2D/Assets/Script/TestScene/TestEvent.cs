﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestEvent : MonoBehaviour {
    public Text Score;
    public Slider HP;
    public Text Warnning;
    public GameObject Player;
    public GameObject GameOver;
    public float WarnningYpos = 11f;
    public bool BossRound;

    [HideInInspector]
    public bool BossDeathCheck;

    private Transform Boss;
    private bool Invinc;

    void Start()
    {
        Screen.SetResolution(400, 600, false);
        BossDeathCheck = false;
        Invinc = false;
        if (BossRound)
        {
            Boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = "Score : " + Player_Data.Score;
        HP.value = Player_Data.HP;
        if (BossRound)
        {
            Boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Transform>();
            BossAreaCheck();
        }
        GameOverCheck();
        Invincibility();
    }

    public void AddScore(int add)
    {
        Player_Data.Score += add;
    }

    public void MinHP(byte min)
    {
        Player_Data.HP -= min;
    }

    private void BossAreaCheck()
    {
        if (Boss.transform.position.y - Player.transform.position.y <= WarnningYpos)
        {
            Warnning.enabled = true;
            Warnning.GetComponent<Animator>().SetBool("Warn", true);
        }
        if (Boss.transform.position.y - Player.transform.position.y < Constant.RECOGNIZED_PLAYER - 3f)
        {
            if(Boss.GetComponent<Enemy_Info>().HP >0)
            {
                Boss.GetComponent<Enemy_Info>().FireEnabled = true;
            }
            GameObject.Find(Constant.NAME_PLAYER).GetComponent<Auto_Move>().AutoCheck = false;
        }
        if(Player_Data.HP<=0)
        {
            Boss.GetComponent<Enemy_Info>().FireEnabled = false;
            Boss.GetComponent<Enemy_Info>().FireState = false;
        }
    }

    private void GameOverCheck()
    {
        if (Player_Data.HP <= 0)
        {
            Player_Data.HP = 0;
            Player_Data.AutoSpeed = 0f;
            Player.transform.Find("Main Camera").Find("GameOver Canvas").GetComponent<GameOver_Menu>().GameOverType = 0;
            Player.transform.Find("Aircraft Body").GetComponent<Player_Move>().Death = true;
            GameOver.transform.Find("Title").GetComponent<Text>().text = "게임 오버";
            GameOver.transform.Find("LastScore").GetComponent<Text>().text = "최종점수 : " + Player_Data.Score;
            GameOver.transform.Find("Restart").GetComponentInChildren<Text>().text = "재시작";
            GameOver.GetComponent<Animator>().SetBool("GameOver", true);
        }
        else if (BossDeathCheck)
        {
            Player_Data.AutoSpeed = 0f;
            Player.GetComponent<Transform>().Translate(0f, 0f, 0f);
            Player.transform.Find("Main Camera").Find("GameOver Canvas").GetComponent<GameOver_Menu>().GameOverType = 1;
            Player.transform.Find("Aircraft Body").GetComponent<Player_Move>().Death = true;
            GameOver.transform.Find("Title").GetComponent<Text>().text = "클리어";
            GameOver.transform.Find("LastScore").GetComponent<Text>().text = "최종점수 : " + Player_Data.Score;
            GameOver.transform.Find("Restart").GetComponentInChildren<Text>().text = "다음단계";
            GameOver.GetComponent<Animator>().SetBool("GameOver", true);
        }
    }

    private void Invincibility()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            Invinc = !Invinc;
        }
        if (Invinc)
        {
            Player_Data.HP = 90;
        }
    }
}
