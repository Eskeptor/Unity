using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver_Menu : MonoBehaviour {

	public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
