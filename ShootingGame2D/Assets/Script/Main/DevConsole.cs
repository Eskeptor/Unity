using UnityEngine;
using System.Collections;

public class DevConsole : MonoBehaviour {
    public GameObject Console;
    private bool Activated;
    private string[] Command;

	// Use this for initialization
	void Start () {
        Console.SetActive(false);
        Activated = false;
        Command = new string[2];
    }
	
	// Update is called once per frame
	void Update () {
        KeydownConsole();
    }

    public void ConsoleInput(string argv)
    {
        Command = argv.Split('=');
        if(Command[0] == Constant.COMMAND_DEVMODE)
        {
            if (Command[1] == "0")
            {
                Player_Data.Devmode = 0;
            }
            else if (Command[1] == "1")
            {
                Player_Data.Devmode = 1;
            }
        }
        //Debug.Log(Player_Data.Devmode);
    }

    void KeydownConsole()
    {
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (Activated)
            {
                Console.SetActive(false);
                Activated = false;
            }
            else
            {
                Console.SetActive(true);
                Activated = true;
            }
        }
    }
}
