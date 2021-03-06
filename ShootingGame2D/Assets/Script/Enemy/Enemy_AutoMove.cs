﻿using UnityEngine;
using System.Collections;

public class Enemy_AutoMove : MonoBehaviour
{
    public byte MoveType = 1;

    private GameObject DownShift;

    void Start ()
    {
        DownShift = GameObject.Find("DownShift");
    }

	void Update ()
    {
        DistanceChecker();
	}

    void DistanceChecker()
    {
        if (transform.position.y - DownShift.transform.position.y < Constant.RECOGNIZED_PLAYER - 1f)
        {
            if (GetComponent<Enemy_Info>().HP != 0)
            {
                switch(MoveType)
                {
                    case 1:
                        transform.Translate(-(Player_Data.AutoSpeed + 1f) * Time.deltaTime, -Player_Data.AutoSpeed * Time.deltaTime, 0f);
                        break;
                    case 2:
                        transform.Translate((Player_Data.AutoSpeed + 1f) * Time.deltaTime, -Player_Data.AutoSpeed * Time.deltaTime, 0f);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
