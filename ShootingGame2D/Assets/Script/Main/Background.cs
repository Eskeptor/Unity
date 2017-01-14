using UnityEngine;

/* 마우스에 따라서 배경화면이 따라 움직이는 기능 */
public class Background : MonoBehaviour
{
    /* Public Object */
    public float RotateSpeed = 0.1f;    // 회전 속도
	
	void Update ()
    {
        Vector3 MousePos = Input.mousePosition; 
        Vector3 BackroundPos = transform.position; 
        Vector3 WorldPos = Camera.main.ScreenToWorldPoint(MousePos);

        float dx = WorldPos.x - BackroundPos.x;
        float rotateDegree = dx * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0f, rotateDegree * RotateSpeed, 0f);
    }
}
