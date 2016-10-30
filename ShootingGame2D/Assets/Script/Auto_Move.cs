using UnityEngine;
using System.Collections;

public class Auto_Move : MonoBehaviour {
    public float AutoSpeed = 0.2f;
    [HideInInspector]
    public bool AutoCheck;

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
