using UnityEngine;
using System.Collections;

public class Enemy_Missile : MonoBehaviour {
    public float MissileSpeed = 10f;
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector3(0, 0, 1) * -1 * MissileSpeed * Time.deltaTime);
	}
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player")
        {
            this.GetComponent<Collider>().enabled = false;
            Debug.Log("Enemy_Missile.cs : 플레이어와 부딛침");
        }
    }
}
