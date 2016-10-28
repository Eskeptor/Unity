using UnityEngine;
using System.Collections;

public class Background_Move : MonoBehaviour {
    public float MoveSpeed = 0.02f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        transform.Translate(0f, MoveSpeed * Time.deltaTime * -1, 0f);
    }
}
