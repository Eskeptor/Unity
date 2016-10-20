using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Level_Convert
{
    private List<Level_Data> level_data = new List<Level_Data>();

    public void Level_Data_Load(TextAsset levelDesign)
    {
        // 텍스트파일 가져오기
        string level_design_txt = levelDesign.text;

        // 개행 할때마다 문자열 배열에 넣기
        string[] line = level_design_txt.Split('\n');

        for (int i = 0; i < line.Length; i++)
        {
            // 행을 워드단위로 나눈다
            string[] word = line[i].Split();
            // 레벨데이터에 저장할 인덱스 생성
            int sel = 0;
            // 현재 레벨데이터 변수 추가
            Level_Data level_data_cur = new Level_Data();

            // 만약에 행이 비었다면 다시 처음으로
            if (line[i] == "")
            {
                continue;
            }
            Debug.Log(line[i]);

            for (int j = 0; j < word.Length; j++)
            {
                // 메모장안에 @로 시작하는 구절은 주석으로 처리
                if (word[j].StartsWith("@"))
                {
                    break;
                }
                // 워드가 비어있으면 처음으로
                if (word[i] == " ")
                {
                    continue;
                }
                switch (sel)
                {
                    case 0:
                        level_data_cur.enemy_count = int.Parse(word[i]);
                        break;
                }
                sel++;
            }
            // sel이 정상적으로 처리가 되면(sel이 3이상이 되면)
            // 레벨데이터에 현재 데이터 추가
            if (sel >= 1)
            {
                this.level_data.Add(level_data_cur);
            }
            else
            {
                if (sel == 0)
                {
                    // 0일 경우는 첫행이 주석일 경우이므로 정상
                }
                else
                {
                    Debug.LogError("Level_Convert.cs : 데이터 개수가 맞지 않음");
                }
            }
        }
        // 레벨데이터가 비어있을 시
        if (this.level_data.Count == 0)
        {
            Debug.LogError("Level_Convert.cs : 데이터가 비어있음");
            this.level_data.Add(new Level_Data());
        }
    }
}