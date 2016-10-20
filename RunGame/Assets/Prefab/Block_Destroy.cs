using UnityEngine;
using System.Collections;

public class Block_Destroy : MonoBehaviour {
    public Map_Create MapCreate = null;
    // Use this for initialization
    private bool Destroy_Object_State = true;
	void Start () {
        MapCreate = GameObject.Find("GameLogic").GetComponent<Map_Create>();
	}
	
	// Update is called once per frame
	void Update () {
        /*if (!DOS())
        {
            return;
        }*/
        
        if (this.MapCreate.isDelete(this.gameObject))
        {
            GameObject.Destroy(this.gameObject);
        }
        
        
	}/*
    private void Destroy_Object()
    {
        Destroy_Object_State = true;
    }
    bool DOS()
    {
        if (Destroy_Object_State)
        {
            Invoke("Destroy_Object", 111f);
            GameObject.Destroy(this.gameObject);
            Destroy_Object_State = false;
        }
        return true;
    }*/
}
