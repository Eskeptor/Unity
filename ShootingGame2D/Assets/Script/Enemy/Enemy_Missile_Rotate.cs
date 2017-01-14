using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Missile_Rotate : MonoBehaviour {
    private float rot;
	// Use this for initialization
	void Start () {
        rot = 10f;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, 0f, rot);
        if (transform.rotation.z >= 360)
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }
}
