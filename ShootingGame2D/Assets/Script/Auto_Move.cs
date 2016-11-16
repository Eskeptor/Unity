using UnityEngine;
using System.Collections;

public class Auto_Move : MonoBehaviour {
    /* Public Object */
    public float AutoSpeed = 0.2f;      // player's auto move speed
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
            transform.Translate(0f, AutoSpeed * Time.deltaTime, 0f);
        }
    }
}
