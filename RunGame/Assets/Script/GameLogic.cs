using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour {
    public float PlayTime = 0f;
    private Player_Run player = null;
	// Use this for initialization
	
	void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Run>();
    }
	// Update is called once per frame
	void Update () {
        this.PlayTime += Time.deltaTime;
        if (this.player.is_End())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
	}
    public float getPlayTime()
    {
        float time;
        time = this.PlayTime;
        return time;
    }
}
