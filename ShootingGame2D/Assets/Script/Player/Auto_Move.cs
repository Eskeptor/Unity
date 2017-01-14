using UnityEngine;
using System.Collections;

public class Auto_Move : MonoBehaviour
{
    /* Public Objects */
    [HideInInspector]
    public bool AutoCheck;            

	void Start ()
    {
        AutoCheck = true;
    }
	
	void Update ()
    {
        Move();
    }

    void Move()
    {
        if (AutoCheck)
        {
            transform.Translate(0f, Player_Data.AutoSpeed * Time.deltaTime, 0f);
        }
    }
}
