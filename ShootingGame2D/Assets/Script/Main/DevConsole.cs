﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DevConsole : MonoBehaviour
{
    /* public objects */
    public GameObject Console;
    public GameObject Title;
    private GameObject[] DevButton;

    /* private objects */
    private bool Activated;
    private string[] Command;

	void Start ()
    {
        Console.SetActive(false);
        Activated = false;
        Command = new string[2];

        DevButton = GameObject.FindGameObjectsWithTag("Dev");
        for(int i = 0; i < DevButton.Length; i++)
        {
            DevButton[i].SetActive(false);
        }
    }
	
	void Update ()
    {
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
                Title.GetComponent<Text>().text = "Space Shooter";
                for (int i = 0; i < DevButton.Length; i++)
                {
                    DevButton[i].SetActive(false);
                }
            }
            else if (Command[1] == "1")
            {
                Player_Data.Devmode = 1;
                Title.GetComponent<Text>().text = "개발자 모드";
                for (int i = 0; i < DevButton.Length; i++)
                {
                    DevButton[i].SetActive(true);
                }
            }
        }
        Console.GetComponent<InputField>().text = "";
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
