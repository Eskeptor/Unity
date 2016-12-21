using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Editor_ConvertCSV : MonoBehaviour {

    private GameObject[] Obj;
    private string SavePath;
    private StringBuilder filename, Data;
    private System.IO.DirectoryInfo dirCheck;

    // Use this for initialization
    void Start () {
        Screen.SetResolution(400, 600, false);

        SavePath = Application.persistentDataPath + "/CustomMaps/";
        dirCheck = new System.IO.DirectoryInfo(SavePath);
        if(dirCheck.Exists == false)
        {
            dirCheck.Create();
        }

        filename = new StringBuilder();
        filename.Append(SavePath);
        Debug.Log(filename);
    }
	
	// Update is called once per frame

    public void ObjectCreate()
    {
        Data = new StringBuilder();
        Data.AppendLine("//x,y,type");
        filename.Append(GameObject.FindGameObjectWithTag("InputBox").GetComponent<InputField>().text);
        Obj = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < Obj.Length; i++)
        {
            Data.Append(Obj[i].GetComponent<Transform>().position.x);
            Data.Append(",");
            Data.Append(Obj[i].GetComponent<Transform>().position.y);
            Data.Append(",");
            Data.Append(Obj[i].GetComponent<Enemy_Info>().Type);
            Data.AppendLine();
        }
        filename.Append(".csv");
        System.IO.File.WriteAllText(filename.ToString(), Data.ToString());
        Debug.Log("작업 완료");
        filename.Remove(filename.Length - GameObject.FindGameObjectWithTag("InputBox").GetComponent<InputField>().text.Length - 4, GameObject.FindGameObjectWithTag("InputBox").GetComponent<InputField>().text.Length + 4);
        GameObject.FindGameObjectWithTag("InputBox").GetComponent<InputField>().text = "";
        Debug.Log(filename);
    }
}
