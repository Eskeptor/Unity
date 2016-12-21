using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_Test : MonoBehaviour {
    private Vector2 MousePos;
    private GameObject Enemy;
    private bool Click;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Enemy = Clicked();
            if (Enemy != null)
            {
                Click = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Click = false;
        }

        if (true == Click)
        {
            MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Enemy.transform.position = new Vector3(MousePos.x, MousePos.y, Enemy.transform.position.z);
        }
    }

    private GameObject Clicked()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject target = null;
        Ray2D ray = new Ray2D(MousePos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
}
