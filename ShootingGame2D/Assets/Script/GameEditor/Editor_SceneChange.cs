using UnityEngine.SceneManagement;
using UnityEngine;

public class Editor_SceneChange : MonoBehaviour
{
    public GameObject MenuUI;
    private bool Check;
    void Start()
    {
        MenuUI.SetActive(false);
        Check = false;
    }

    public void Menu()
    {
        MenuUI.SetActive(!Check);
        Check = !Check;
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void SetCheck(bool Check)
    {
        this.Check = Check;
    }
}
