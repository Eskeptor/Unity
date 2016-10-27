using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class Level_Data_Test : MonoBehaviour {
    public int woodplus;
    public int mineralplus;
    public int stoneplus;
    public int goldplus;

    protected FileInfo sourceFile = null;
    protected StreamReader reader = null;
    protected string text = " ";

    // Use this for initialization
    void Start()
    {
        sourceFile = new FileInfo("D:/Study/UnityProject/Test2/ShootingGame/Assets/StreamingAssets/Level_Design.txt"); //test.txt파일 불러오기
        reader = sourceFile.OpenText(); // 스티림리더로 텍스트 파일 불러오기
        GetArrays();
        Debug.Log("mineralplus : " + mineralplus);
        Debug.Log("woodplus : " + woodplus);
        Debug.Log("goldplus : " + goldplus);
    }
    void GetArrays()//불러온 텍스트 파일을 읽어 입력
    {
        for (int i = 0; (text = reader.ReadLine()) != null; i++)
        {
            if (Regex.IsMatch(text, "mineral", RegexOptions.IgnoreCase))
            { //문자열에서 특정 문자 찾기
                mineralplus = int.Parse(Regex.Replace(text, @"\D+", "")); //문자열에서 숫자만 찾아서 입력
            }
            else if (Regex.IsMatch(text, "wood", RegexOptions.IgnoreCase))
            {
                woodplus = int.Parse(Regex.Replace(text, @"\D+", ""));
            }
            else if (Regex.IsMatch(text, "gold", RegexOptions.IgnoreCase))
            {
                goldplus = int.Parse(Regex.Replace(text, @"\D+", ""));
            }
        }

        reader.Close();
    }
}
