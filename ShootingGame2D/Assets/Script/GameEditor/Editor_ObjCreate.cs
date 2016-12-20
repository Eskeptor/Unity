using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Editor_ObjCreate : MonoBehaviour {
    public GameObject ObjLocate;
    private GameObject[] Obj;
    private string filename, Data;

	// Use this for initialization
	void Start () {
        Screen.SetResolution(400, 600, false);
    }
	
	// Update is called once per frame

    public void ObjectCreate()
    {
        Data = "//x,y,type" + System.Environment.NewLine;
        filename = GameObject.FindGameObjectWithTag("InputBox").GetComponent<InputField>().text;
        Obj = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < Obj.Length; i++)
        {
            Data += Obj[i].GetComponent<Transform>().position.x;
            Data += ",";
            Data += Obj[i].GetComponent<Transform>().position.y;
            Data += ",";
            Data += Obj[i].GetComponent<Enemy_Info>().Type;
            Data += System.Environment.NewLine;
        }
        System.IO.File.WriteAllText(filename, Data);
        Debug.Log("작업 완료");
        GameObject.FindGameObjectWithTag("InputBox").GetComponent<InputField>().text = "";
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("Main");
    }
}
