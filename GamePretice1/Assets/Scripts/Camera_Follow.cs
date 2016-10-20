using UnityEngine;
using System.Collections;

public class Camera_Follow : MonoBehaviour {
    public Transform target;
    public float smoothing = 5f;

    Vector3 offset; //플레이어와 카메라 사이의 거리

    void Start()
    {
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
