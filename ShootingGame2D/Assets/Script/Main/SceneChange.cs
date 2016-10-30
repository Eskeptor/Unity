using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {
    //public Animation Menu;
    //public Animation Set;
    public Animator HowTo;

    void Start()
    {
        HowTo.SetBool("CloseDown", false);
        HowTo.SetBool("ButtonDown", false);
    }

	public void GameStart()
    {
        SceneManager.LoadScene("Level1");
    }

    public void HowToPlay()
    {
        HowTo.SetBool("CloseDown", false);
        HowTo.SetBool("ButtonDown", true);
    }

    public void Close()
    {
        HowTo.SetBool("CloseDown", true);
        HowTo.SetBool("ButtonDown", false);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
