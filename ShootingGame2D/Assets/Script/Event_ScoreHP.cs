using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Event_ScoreHP : MonoBehaviour {
    public Text Score;
    public Slider HP;
    public Text Warnning;
    public GameObject Player;
    public GameObject GameOver;
    public float WarnningYpos = 11f;

    [HideInInspector]
    public int hp;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public bool check;

	// Use this for initialization
	void Start () {
        score = 0;
        hp = 90;
        check = false;
    }
	
	// Update is called once per frame
	void Update () {
        Score.text = "Score : " + score;
        HP.value = hp;
        BossAreaCheck();
        GameOverCheck();
    }

    public void AddScore(int add)
    {
        score += add;
    }

    public void MinHP(int min)
    {
        hp -= min;
    }

    private void BossAreaCheck()
    {
        if(Player.transform.position.y >= 11f)
        {
            Warnning.enabled = true;
            Warnning.GetComponent<Animator>().SetBool("Warn", true);
        }
    }

    private void GameOverCheck()
    {
        if (hp <= 0 || check)
        {
            Player.GetComponent<Auto_Move>().AutoSpeed = 0f;
            GameOver.GetComponent<Animator>().SetBool("GameOver", true);
            GameOver.GetComponentInChildren<Text>().text = "최종점수 : " + score;
        }
    }
}
