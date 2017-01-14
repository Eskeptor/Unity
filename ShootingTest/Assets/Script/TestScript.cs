using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {
    int a, b;
    int num1, num2;

    void Start ()
    {
        a = 5;
        b = 7;

        a += b;
        Debug.Log("a+=b : " + a);
        a -= b;
        Debug.Log("a-=b : " + a);
        b *= a;
        Debug.Log("b*=a : " + b);
        b /= a;
        Debug.Log("b/=a : " + b);

        num1 = 4;
        num2 = 8;

        Debug.Log("num1 : " + num1);
        Debug.Log("num1++ : " + num1++);
        Debug.Log("num1 : " + num1);
        Debug.Log("++num1 : " + ++num1);
        Debug.Log("num1 : " + num1);
        Debug.Log("num2 : " + num2);
        Debug.Log("num2-- : " + num2--);
        Debug.Log("num2 : " + num2);
        Debug.Log("--num2 : " + --num2);
        Debug.Log("num2 : " + num2);
    }
}
