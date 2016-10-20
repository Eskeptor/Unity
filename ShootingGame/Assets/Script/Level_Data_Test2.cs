using UnityEngine;
using System.Collections;

public class Level_Data_Test2 : MonoBehaviour {

	void Start () {
        PlayerPrefs.SetInt("HP", 100);
        PlayerPrefs.SetInt("Score", 0);
	}
	
	// Update is called once per frame
	void Update () {
        PlayerPrefs.HasKey("HP");
        PlayerPrefs.Save();
        Debug.Log("HP : " + PlayerPrefs.GetInt("HP"));
	}
}
