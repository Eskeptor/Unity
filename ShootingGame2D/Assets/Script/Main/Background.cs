using UnityEngine;

// This class is background effect class
public class Background : MonoBehaviour {
    /* Public Object */
    public float RotateSpeed = 0.1f;    // Screen rotate speed
	
	// Update is called once per frame
	void Update () {
        Vector3 MousePos = Input.mousePosition; 
        Vector3 BackroundPos = transform.position; 
        Vector3 WorldPos = Camera.main.ScreenToWorldPoint(MousePos);

        float dx = WorldPos.x - BackroundPos.x;
        float rotateDegree = dx * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0f, rotateDegree * RotateSpeed, 0f);
    }
}
