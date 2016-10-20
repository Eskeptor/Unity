using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour {
    public void OnStartButton()
    {
        Debug.Log("Main_Menu.cs : 시작버튼 눌림");
        SceneManager.LoadScene("Stage01");
    }
    public void OnExitButton()
    {
        Debug.Log("Main_Menu.cs : 종료버튼 눌림");
        Application.Quit();
    }
}
