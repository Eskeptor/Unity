using UnityEngine;
using System.Collections;

public class Camera_Move : MonoBehaviour {
    private GameObject Player = null;
    private Vector3 Position_Offset = Vector3.zero;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        this.Position_Offset = this.transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void LastUpdate () {
        Vector3 Moving_Position = this.transform.position;
        Moving_Position.x = Player.transform.position.x + this.Position_Offset.x;
        this.transform.position = Moving_Position;
	}
}
