using UnityEngine;
using System.Collections;

public class Enemy_Create : MonoBehaviour {
    public GameObject Enemy = null;
    public int EnemyMaximumPool = 5;
    public float DestroyEnemyYpos = -2f;

    private MemoryPool MPool = new MemoryPool();
    private GameObject[] EnemyObject;
    private int EnemyDeathCount;
    private bool EnemyAllDeathCheck;

	// Use this for initialization
	void Start () {
        // Init Enemy death count and death check
        EnemyDeathCount = 0;
        EnemyAllDeathCheck = false;
        // Create Enemy in memory pool
        MPool.Create(Enemy, EnemyMaximumPool);
        EnemyObject = new GameObject[EnemyMaximumPool];
	}
	
	// Update is called once per frame
	void Update () {
        if (!Create())
        {
            return;
        }
	}

    // Enemy create
    bool Create()
    {
        if (EnemyDeathCount == 0)
        {
            for (int i = 0; i < EnemyMaximumPool; i++)
            {
                if (EnemyObject[i] == null)
                {
                    EnemyObject[i] = MPool.NewItem();
                    EnemyObject[i].transform.position = new Vector3(Random.Range(-3.05f, 3.05f), 8f);
                }
            }
        }
        for(int i = 0; i < EnemyMaximumPool; i++)
        {
            if (EnemyObject[i])
            {
                if (EnemyObject[i].transform.position.y < DestroyEnemyYpos)
                {
                    MPool.RemoveItem(EnemyObject[i]);
                    EnemyObject[i] = null;
                    EnemyDeathCount++;
                    Debug.Log("Enemy_Create : 적이 범위를 벗어남");
                }
                else if (EnemyObject[i].GetComponent<Collider2D>().enabled == false)
                {
                    EnemyObject[i].GetComponent<Collider2D>().enabled = true;
                    MPool.RemoveItem(EnemyObject[i]);
                    EnemyObject[i] = null;
                    EnemyDeathCount++;
                    Debug.Log("Enemy_Create : 적이 뭔가와 부딛힘");
                }
            }
            if (EnemyDeathCount == EnemyMaximumPool)
            {
                EnemyDeathCount = 0;
            }
        }
        return true;
    }
}
