using UnityEngine;
using System.Collections;

public class Auto_Move : MonoBehaviour {
    public float AutoSpeed = 0.2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(0f, AutoSpeed * Time.deltaTime, 0f);
	}
}
