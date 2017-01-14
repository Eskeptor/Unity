using UnityEngine;
using System.Collections;

public class Event_PlayerCreate : MonoBehaviour {
    public GameObject PlayerType;
    public GameObject Type1;
    public GameObject Type2;

    private GameObject Player;

    void Start () {
        if (Player_Data.Type == 1)
        {
            PlayerType.transform.Find("Aircraft Body").Find("Aircraft").GetComponent<SpriteRenderer>().sprite = Type1.GetComponent<SpriteRenderer>().sprite;
            PlayerType.transform.position = new Vector3(0f, 0f, 0f);
        }
        else if(Player_Data.Type == 2)
        {
            PlayerType.transform.Find("Aircraft Body").Find("Aircraft").GetComponent<SpriteRenderer>().sprite = Type2.GetComponent<SpriteRenderer>().sprite;
            PlayerType.transform.position = new Vector3(0f, 0f, 0f);
        }
        else
        {
            Debug.Log("플레이어 오브젝트 생성 오류");
        }
	}
}
