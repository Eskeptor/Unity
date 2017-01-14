using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// "AppHelper 클래스" by Unity Community
// http://answers.unity3d.com/questions/161858/startstop-playmode-from-editor-script.html

// Support Event List
// https://docs.unity3d.com/Manual/SupportedEvents.html

public class SceneChange : MonoBehaviour
{
    /* Public Object */
    public Animator HowToAni;              // HowToPlay 애니메이터와 연결
    public Animator CharactorSelectAni;    // CharactorSelect 애니메이터와 연결
    public GameObject CharactorSelect;     // Text 그룹과 연결
    public Image[] PlayerTypes;            // 플레이어 타입 이미지 배열

    /* Private Object */
    private TextAsset PlayerCSV;            // CSV -> Text 변환을 위한 텍스트에셋
    private string[] PlayerDataCSV;         // PlayerCSV를 사용하여 변환한 1차적 가로 데이터
    private string[][] PlayerDataCSV_Spec;  // PlayerDataCSV를 사용하여 변환한 2차적 세부 데이터

    void Start()
    {
        PlayerCSV = Resources.Load("PlayerCSV", typeof(TextAsset)) as TextAsset;    // CSV -> Text
        PlayerDataCSV = PlayerCSV.text.Split('\n');                     
        PlayerDataCSV_Spec = new string[PlayerDataCSV.Length - 1][];   
        for (int i = 1; i < PlayerDataCSV.Length; i++)
        {
            PlayerDataCSV_Spec[i - 1] = PlayerDataCSV[i].Split(',');    
        }

        Screen.SetResolution(400, 600, false);      // 화면 크기고정
        HowToAni.SetBool("CloseDown", false);
        HowToAni.SetBool("ButtonDown", false);
    }

	public void GameStart()
    {
        CharactorSelectAni.SetBool("gameStart", true);
    }

    public void GameStartType1()
    {
        if (PlayerInit(1))
        {
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.Log("PlayerInit Error");
        }
    }

    public void GameStartType2()
    {
        if (PlayerInit(2))
        {
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.Log("PlayerInit Error");
        }
    }

    public void GameStartClose()
    {
        CharactorSelectAni.SetBool("gameStart", false);
    }

    public void GameStartMouseOver(int num)
    {
        CharactorSelect.transform.Find("Texts").Find("MissileType").GetComponent<Text>().text = "미사일 타입 : " + PlayerDataCSV_Spec[num - 1][Constant.PLAYER_CSV_TYPE];
        CharactorSelect.transform.Find("Texts").Find("MissileDamage").GetComponent<Text>().text = "미사일 데미지 : " + PlayerDataCSV_Spec[num - 1][Constant.PLAYER_CSV_DAMAGE];
        CharactorSelect.transform.Find("Texts").Find("FireRate").GetComponent<Text>().text = "연사 속도 : " + PlayerDataCSV_Spec[num - 1][Constant.PLAYER_CSV_FIRERATE];
        CharactorSelect.transform.Find("Texts").Find("PlayerHP").GetComponent<Text>().text = "비행기 체력 : " + PlayerDataCSV_Spec[num - 1][Constant.PLAYER_CSV_HP];
        CharactorSelect.transform.Find("Airplane").GetComponent<Image>().sprite = PlayerTypes[num - 1].sprite;
        CharactorSelect.transform.Find("Airplane").GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }

    public void GameStartMouseRealease()
    {
        CharactorSelect.transform.Find("Texts").Find("MissileType").GetComponent<Text>().text = "미사일 타입 : ";
        CharactorSelect.transform.Find("Texts").Find("MissileDamage").GetComponent<Text>().text = "미사일 데미지 : ";
        CharactorSelect.transform.Find("Texts").Find("FireRate").GetComponent<Text>().text = "연사 속도 : ";
        CharactorSelect.transform.Find("Texts").Find("PlayerHP").GetComponent<Text>().text = "비행기 체력 : ";
        CharactorSelect.transform.Find("Airplane").GetComponent<Image>().sprite = null;
        CharactorSelect.transform.Find("Airplane").GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }

    public void HowToPlay()
    {
        HowToAni.SetBool("CloseDown", false);
        HowToAni.SetBool("ButtonDown", true);
    }

    public void HowToPlayClose()
    {
        HowToAni.SetBool("CloseDown", true);
        HowToAni.SetBool("ButtonDown", false);
    }

    public void GameExit()
    {
        AppHelper.Quit();
    }

    public void TestFunc()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void MapEditor()
    {
        SceneManager.LoadScene("GameEditor");
    }

    private bool PlayerInit(int type)
    {
        if (type <= PlayerDataCSV.Length - 1) 
        {
            Player_Data.Type = byte.Parse(PlayerDataCSV_Spec[type - 1][Constant.PLAYER_CSV_TYPE]);
            Player_Data.Damage = byte.Parse(PlayerDataCSV_Spec[type - 1][Constant.PLAYER_CSV_DAMAGE]);
            Player_Data.FireRate = float.Parse(PlayerDataCSV_Spec[type - 1][Constant.PLAYER_CSV_FIRERATE]);
            Player_Data.HP = Player_Data.Init_HP = byte.Parse(PlayerDataCSV_Spec[type - 1][Constant.PLAYER_CSV_HP]);
            return true;
        }
        return false;
    }
}

/* 유니티 종료 클래스(타입에 따라서 다름) */
public static class AppHelper
{
#if UNITY_WEBPLAYER
     public static string webplayerQuitURL = "http://google.com";
#endif
    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }
}
