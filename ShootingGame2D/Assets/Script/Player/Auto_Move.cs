using UnityEngine;
using System.Collections;

public class Auto_Move : MonoBehaviour {
    /* Public Object */
    [HideInInspector]
    public bool AutoCheck;              // player auto move enabler

	// Use this for initialization
	void Start () {
        AutoCheck = true;
    }
	
	// Update is called once per frame
	void Update () {
        Move();
    }

    void Move()
    {
        if (AutoCheck)
        {
            transform.Translate(0f, Player_Data.AutoSpeed * Time.deltaTime, 0f);
        }
    }
}
