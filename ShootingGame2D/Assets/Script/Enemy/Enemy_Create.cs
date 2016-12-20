using UnityEngine;

public class Enemy_Create : MonoBehaviour {
    /* Public Object */
    public TextAsset LevelData;             // Level Data CSV file
    public GameObject[] EnemyObject;        // Enemy Objects(used to this stage)
    public GameObject BossObject;           // Boss Object(used to this stage)

    /* Private Object */
    private TextAsset EnemyData;            // Enemy Data CSV file
    private string[] LevelDataCSV;          // converted data(using LevelData)
    private string[][] LevelDataCSV_Spec;   // converted and split data(using LevelData)
    private string[] EnemyDataCSV;          // converted data(using EnemyData)
    private string[][] EnemyDataCSV_Spec;   // converted and split data(using EnemyData)
    private GameObject[,] Enemys;           // generated object

    // Use this for initialization
    void Start () {
        EnemyData = Resources.Load("EnemyCSV", typeof(TextAsset)) as TextAsset; // load "EnemyCSV" file
        LevelDataCSV = LevelData.text.Split('\n');                              // converted data(using LevelCSV(split '\n'))
        LevelDataCSV_Spec = new string[LevelDataCSV.Length - 1][];              // initialization array(allocation size by LevelDataCSV)
        EnemyDataCSV = EnemyData.text.Split('\n');                              // converted data(using EnemyCSV(split '\n'))
        EnemyDataCSV_Spec = new string[EnemyDataCSV.Length - 1][];              // initialization array(allocation size by EnemyDataCSV)
        Enemys = new GameObject[EnemyObject.Length, LevelDataCSV.Length - 1];   // Enemys array initialization

        for (int i = 1; i < EnemyDataCSV.Length; i++)
        {
            EnemyDataCSV_Spec[i - 1] = EnemyDataCSV[i].Split(',');              // converted data(using EnemyDataCSV(split ','))
        }

        for (int i = 1; i < LevelDataCSV.Length; i++)
        {
            LevelDataCSV_Spec[i - 1] = LevelDataCSV[i].Split(',');              // converted data(using LevelDataCSV(split ','))
            for (int j = 0; j < EnemyObject.Length; j++)
            {
                if(EnemyObject[j].GetComponent<Enemy_Info>().Type == byte.Parse(LevelDataCSV_Spec[i - 1][Constant.LEVEL_CSV_TYPE]))
                {
                    // initialization Enemys Object
                    Enemys[j, i - 1] = Instantiate(EnemyObject[j]) as GameObject;
                    Enemys[j, i - 1].transform.position = new Vector3(float.Parse(LevelDataCSV_Spec[i - 1][Constant.LEVEL_CSV_XPOS]), float.Parse(LevelDataCSV_Spec[i - 1][Constant.LEVEL_CSV_YPOS]), 0f);
                    Enemys[j, i - 1].transform.rotation = Quaternion.Euler(0, 0, 180f);
                    for (int k = 0; k < EnemyObject.Length; k++)
                    {
                        // initialization Enemys Object's Enemy_Info
                        if (EnemyObject[j].GetComponent<Enemy_Info>().Type == byte.Parse(EnemyDataCSV_Spec[k][Constant.ENEMY_CSV_TYPE]))
                        {
                            Enemys[j, i - 1].GetComponent<Enemy_Info>().HP = int.Parse(EnemyDataCSV_Spec[k][Constant.ENEMY_CSV_HP]);
                            Enemys[j, i - 1].GetComponent<Enemy_Info>().Score = int.Parse(EnemyDataCSV_Spec[k][Constant.ENEMY_CSV_SCORE]);
                        }
                    }
                }
            }
        }

        // initialization Boss Object
        BossObject = Instantiate(BossObject) as GameObject;
        BossObject.transform.position = new Vector3(float.Parse(LevelDataCSV_Spec[LevelDataCSV.Length - 2][Constant.LEVEL_CSV_XPOS]), float.Parse(LevelDataCSV_Spec[LevelDataCSV.Length - 2][Constant.LEVEL_CSV_YPOS]), 0f);
        BossObject.transform.rotation = Quaternion.Euler(0, 0, 180f);
    }
}
