using UnityEngine;
using System.Collections;

public class Missile_Move : MonoBehaviour {
    public float MoveSpeed = 20f;
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    // Missile Move
    void Move()
    {
        transform.Translate(0f, MoveSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // When missile hit Enemy tag(The missile hits are Collider disabled)
        if (col.GetComponent<Collider2D>().tag == "Enemy")
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log("Missile_Move : 미사일이 적과 부딛힘");
        }
    }
}
