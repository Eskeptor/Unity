using UnityEngine;

public class Enemy : MonoBehaviour {
    public int Enemy1Count = 10;
    public int Missile1Count = 50;
    public float DestroyEnemyZpos = -1.8f;
    public int Score = 0;

    public GameObject EnemyObject1 = null;
    public Player_Move Player = null;
    public Start_Event ScoreManager = null;
    public TextAsset LevelDataText = null;

    MemoryPool enemy1_pool = new MemoryPool();
    GameObject[] enemy1 = null;

    private bool enemy_State;
    private int Enemy_Death_Counter;
    private int level = 0;
    private float EnemySpawnMaxXpos = 0f;

    /* 프로그램 종료시 메모리 비움 */
    void OnApplicationQuit()
    {
        enemy1_pool.Dispose();
    }

    
    void Start () {
        // 게임 실행과 동시에 적 생성
        enemy1_pool.Create(EnemyObject1, Enemy1Count);
        enemy1 = new GameObject[Enemy1Count];
        enemy_State = true;
        for (int i = 0; i < enemy1.Length; i++)
        {
            enemy1[i] = null;
        }

        //데스카운터 초기화
        Enemy_Death_Counter = 0;

        EnemySpawnMaxXpos = Player.MoveMaxXpos;
    }
	
	void Update () {
        
        if (!Enemy_Create())
        {
            return;
        }
        if (!Missile_Create())
        {
            return;
        }
	}
    // 적생성
    bool Enemy_Create()
    {
        if (enemy_State)
        {
            
            for (int i = 0; i <enemy1.Length; i++)
            {
                if(enemy1[i] == null)
                {
                    enemy1[i] = enemy1_pool.NewItem();
                    enemy1[i].transform.position = new Vector3(Random.Range(-EnemySpawnMaxXpos, EnemySpawnMaxXpos), this.Player.transform.position.y + 0.3f, 8f);
                }
            }
            enemy_State = false;
            
            Debug.Log("Enemy.cs : 적 생성 완료");
        }
        for(int i = 0; i < enemy1.Length; i++)
        {
            if (enemy1[i])
            {
                if (enemy1[i].transform.position.z < DestroyEnemyZpos)
                {
                    enemy1_pool.RemoveItem(enemy1[i]);
                    enemy1[i] = null;
                    Enemy_Death_Counter++;
                }
                else if(enemy1[i].GetComponent<Collider>().enabled == false)
                {
                    Score += 20;
                    ScoreManager.Score_Manager(Score);
                    enemy1[i].GetComponent<Collider>().enabled = true;
                    enemy1_pool.RemoveItem(enemy1[i]);
                    enemy1[i] = null;
                    Enemy_Death_Counter++;
                    Debug.Log("Enemy.cs : 적 제거 완료");
                }
            }
            // 만약에 적을 다 죽였다면 다시 적을 생성하도록 enemy_State를 true로 만든다.
            if(Enemy_Death_Counter == Enemy1Count)
            {
                level++;
                Enemy_Death_Counter = 0;
                enemy_State = true;
            }
        }
        return true;
    }
    bool Missile_Create()
    {

        return true;
    }
}

