using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public float MoveSpeed = 3f;
    public GameObject MenuUI;

    private Vector2 MaxPos;
    private Vector2 MinPos;
    private bool Check;
	// Use this for initialization
	void Start ()
    {
        MaxPos = new Vector2(0, Constant.EDITOR_MAX_YPOS);
        MinPos = new Vector2(0, Constant.EDITOR_MIN_YPOS);
        MenuUI.SetActive(false);
        Check = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Menu();
        Move();
    }

    private void Move ()
    {
        if (Input.GetButton("Vertical"))
        {
            transform.Translate(0f, Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime, 0f); 
        }
        if (transform.position.y >= Constant.EDITOR_MAX_YPOS)
        {
            transform.position = MaxPos;
        }
        if (transform.position.y <= Constant.EDITOR_MIN_YPOS)
        {
            transform.position = MinPos;
        }
    }

    private void Menu()
    {
        if (Input.GetKey(KeyCode.I))
        {
            MenuUI.SetActive(!Check);
            Invoke("Delay", 1f);
        }
    }

    private void Delay()
    {
        Check = !Check;
    }
}
