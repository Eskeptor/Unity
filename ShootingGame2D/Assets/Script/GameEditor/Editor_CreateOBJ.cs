using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor_CreateOBJ : MonoBehaviour {
    public GameObject[] Enemys;

    private GameObject Enemy;
    private Vector3 MousePos;


    public void DraggingOBJ(int num)
    {
        MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        for (int i = 0; i < Enemys.Length; i++)
        {
            if(Enemys[i].GetComponent<Enemy_Info>().Type == num)
            {
                Instantiate(Enemys[i], new Vector3(MousePos.x, MousePos.y, 0f), Enemys[i].GetComponent<Transform>().rotation);
            }
        }
        transform.Find(Constant.NAME_EDITOR_TOOLS).gameObject.SetActive(false);
        GetComponent<Editor_SceneChange>().SetCheck(false);
    }
}
