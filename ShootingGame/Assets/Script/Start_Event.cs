﻿using UnityEngine;
using UnityEngine.UI;

public class Start_Event : MonoBehaviour {
    public GameObject RestartMenu = null;
    public GameObject Player = null;
    public Text ScoreText = null;
    //public Text HPText = null;
    public Slider HPSlider = null;
    public Image DamageImage = null;
    public int MaxScore = 0;

    int score = 0;
    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
	void Start () {
        RestartMenu.SetActive(false);
        Score_Manager(0);
        HP_Manager(100);
	}
    void Update()
    {
        // 플레이어의 컬라이더가 false 되었을때 재시작 메뉴 띄움
        if (Player.GetComponent<Collider>().enabled == false)
        {
            RestartMenu.SetActive(true);
        }
        PlayerPrefs.HasKey("Score");
        MaxScore = PlayerPrefs.GetInt("Score");
        PlayerPrefs.Save();
        if(HPSlider.value == 0)
        {
            DamageImage.color = Color.Lerp(DamageImage.color, Color.black, 0.1f);
        }
    }
    public void Score_Manager(int add)
    {
        ScoreText.text = "Score : " + add.ToString();
        PlayerPrefs.SetInt("Score", add);
        //score = Convert.ToInt16(ScoreText.text);
        //Debug.Log("Start_Event.cs : " + score);
    }
    public void HP_Manager(int min)
    {
        HPSlider.value = min;
        //HPText.text = "HP : " + min.ToString();
    }
}
