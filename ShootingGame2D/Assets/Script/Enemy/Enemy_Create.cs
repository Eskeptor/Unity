using UnityEngine;

public class Enemy_Create : MonoBehaviour
{
    /* Public Objects */
    public TextAsset LevelData;             // 레벨용 CSV파일
    public GameObject[] EnemyObject;        // 현 스테이지에서 사용하는 적
    public GameObject BossObject;           // 보스

    /* Private Objects */
    private TextAsset EnemyData;            // 적 정보용 CSV파일
    private string[] LevelDataCSV;          // LevelData를 사용하여 변환한 1차적 가로 데이터
    private string[][] LevelDataCSV_Spec;   // LevelDataCSV를 사용하여 변환한 2차적 세부 데이터
    private string[] EnemyDataCSV;          // EnemyData를 사용하여 변환한 1차적 가로 데이터
    private string[][] EnemyDataCSV_Spec;   // EnemyDataCSV를 사용하여 변환한 2차적 세부 데이터
    private GameObject[,] Enemys;           // 변환한 적 오브젝트

    void Start ()
    {
        EnemyData = Resources.Load("EnemyCSV", typeof(TextAsset)) as TextAsset; 
        LevelDataCSV = LevelData.text.Split('\n');                              
        LevelDataCSV_Spec = new string[LevelDataCSV.Length - 1][];              
        EnemyDataCSV = EnemyData.text.Split('\n');                              
        EnemyDataCSV_Spec = new string[EnemyDataCSV.Length - 1][];              
        Enemys = new GameObject[EnemyObject.Length, LevelDataCSV.Length - 1];   

        for (int i = 1; i < EnemyDataCSV.Length; i++)
        {
            EnemyDataCSV_Spec[i - 1] = EnemyDataCSV[i].Split(',');              
        }

        for (int i = 1; i < LevelDataCSV.Length; i++)
        {
            LevelDataCSV_Spec[i - 1] = LevelDataCSV[i].Split(',');              
            for (int j = 0; j < EnemyObject.Length; j++)
            {
                if(EnemyObject[j].GetComponent<Enemy_Info>().Type == byte.Parse(LevelDataCSV_Spec[i - 1][Constant.LEVEL_CSV_TYPE]))
                {
                    Enemys[j, i - 1] = Instantiate(EnemyObject[j]) as GameObject;
                    Enemys[j, i - 1].transform.position = new Vector3(float.Parse(LevelDataCSV_Spec[i - 1][Constant.LEVEL_CSV_XPOS]), float.Parse(LevelDataCSV_Spec[i - 1][Constant.LEVEL_CSV_YPOS]), 0f);
                    Enemys[j, i - 1].transform.rotation = Quaternion.Euler(0, 0, 180f);
                    for (int k = 0; k < EnemyObject.Length; k++)
                    {
                        if (EnemyObject[j].GetComponent<Enemy_Info>().Type == byte.Parse(EnemyDataCSV_Spec[k][Constant.ENEMY_CSV_TYPE]))
                        {
                            Enemys[j, i - 1].GetComponent<Enemy_Info>().Damage = int.Parse(EnemyDataCSV_Spec[k][Constant.ENEMY_CSV_DAMAGE]);
                            Enemys[j, i - 1].GetComponent<Enemy_Info>().FireRate = float.Parse(EnemyDataCSV_Spec[k][Constant.ENEMY_CSV_FIRERATE]);
                            Enemys[j, i - 1].GetComponent<Enemy_Info>().HP = int.Parse(EnemyDataCSV_Spec[k][Constant.ENEMY_CSV_HP]);
                            Enemys[j, i - 1].GetComponent<Enemy_Info>().Score = int.Parse(EnemyDataCSV_Spec[k][Constant.ENEMY_CSV_SCORE]);
                        }
                    }
                }
            }
        }

        BossObject = Instantiate(BossObject) as GameObject;
        BossObject.transform.position = new Vector3(float.Parse(LevelDataCSV_Spec[LevelDataCSV.Length - 2][Constant.LEVEL_CSV_XPOS]), float.Parse(LevelDataCSV_Spec[LevelDataCSV.Length - 2][Constant.LEVEL_CSV_YPOS]), 0f);
        BossObject.transform.rotation = Quaternion.Euler(0, 0, 180f);
    }
}
