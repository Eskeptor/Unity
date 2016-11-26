using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// AppHelper class by Unity Community
// http://answers.unity3d.com/questions/161858/startstop-playmode-from-editor-script.html

// Support Event List
// https://docs.unity3d.com/Manual/SupportedEvents.html

public class SceneChange : MonoBehaviour {
    /* Public Object */
    public Animator HowToAni;              // HowToPlay animator
    public Animator CharactorSelectAni;    // CharactorSelect animator
    public GameObject CharactorSelect;            // Text group
    public Image[] PlayerTypes;

    /* Private Object */
    private TextAsset PlayerCSV;            // for CSV -> Text
    private string[] PlayerDataCSV;         // converted data(using PlayerCSV)
    private string[][] PlayerDataCSV_Spec;  // converted and split data(using PlayerDataCSV)

    void Start()
    {
        PlayerCSV = Resources.Load("PlayerCSV", typeof(TextAsset)) as TextAsset;    // for CSV -> Text
        PlayerDataCSV = PlayerCSV.text.Split('\n');                     // converted data(using PlayerCSV(split '\n'))
        PlayerDataCSV_Spec = new string[PlayerDataCSV.Length - 1][];    // initialization array(allocation size by PlayerDataCSV)
        for (int i = 1; i < PlayerDataCSV.Length; i++)
        {
            PlayerDataCSV_Spec[i - 1] = PlayerDataCSV[i].Split(',');    // converted data(using PlayerDataCSV(split ','))
        }

        Screen.SetResolution(400, 600, false);      // Screen set resolution
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

    private bool PlayerInit(int type)
    {
        if (type <= PlayerDataCSV.Length - 1) 
        {
            Player_Data.Type = byte.Parse(PlayerDataCSV_Spec[type - 1][Constant.PLAYER_CSV_TYPE]);
            Player_Data.Damage = int.Parse(PlayerDataCSV_Spec[type - 1][Constant.PLAYER_CSV_DAMAGE]);
            Player_Data.FireRate = float.Parse(PlayerDataCSV_Spec[type - 1][Constant.PLAYER_CSV_FIRERATE]);
            Player_Data.HP = Player_Data.Init_HP = byte.Parse(PlayerDataCSV_Spec[type - 1][Constant.PLAYER_CSV_HP]);
            return true;
        }
        return false;
    }
}

// This class is to terminate the Unity Program
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
