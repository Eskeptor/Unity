using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Event_ScoreHP : MonoBehaviour {
    public Text Score;
    public Slider HP;

    [HideInInspector]
    public int hp;
    [HideInInspector]
    public int score;

	// Use this for initialization
	void Start () {
        score = 0;
        hp = 90;
	}
	
	// Update is called once per frame
	void Update () {
        Score.text = "Score : " + score;
        HP.value = hp;
	}

    public void AddScore(int add)
    {
        score += add;
    }

    public void MinHP(int min)
    {
        hp -= min;
    }
}
