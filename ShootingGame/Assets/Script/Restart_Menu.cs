using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Restart_Menu : MonoBehaviour {
	public void OnRestartButton()
    {
        Debug.Log("Restart_Menu.cs : 재시작버튼 눌림");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnExitButton()
    {
        Debug.Log("Restart_Menu.cs : 종료버튼 눌림");
        Application.Quit();
    }
}
