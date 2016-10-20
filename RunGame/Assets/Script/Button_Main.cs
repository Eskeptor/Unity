using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Button_Main : MonoBehaviour {
    //public bool is_Clicked = false;
	// Use this for initialization
	public void OnGUI()
    {
        Debug.Log("버튼눌림");
        SceneManager.LoadScene("Run");

    }
    public void ExitGUI()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
