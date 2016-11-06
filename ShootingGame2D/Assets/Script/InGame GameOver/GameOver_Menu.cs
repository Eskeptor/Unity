using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver_Menu : MonoBehaviour {
    [HideInInspector]
    public int GameOverType;

    void Start()
    {
        GameOverType = 0;
    }

	public void GameRestart()
    {
        if (GameOverType == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Player_Data.HP = Player_Data.Init_HP;
            Player_Data.Score = 0;
        }
        else if (GameOverType == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main");
    }
}
