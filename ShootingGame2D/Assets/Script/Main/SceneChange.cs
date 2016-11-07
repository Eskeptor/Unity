using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// AppHelper class by Unity Community
// http://answers.unity3d.com/questions/161858/startstop-playmode-from-editor-script.html

public class SceneChange : MonoBehaviour {
    //public Animation Menu;
    //public Animation Set;
    public Animator HowTo;

    void Start()
    {
        Screen.SetResolution(400, 600, false);
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
        AppHelper.Quit();
    }
}

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
