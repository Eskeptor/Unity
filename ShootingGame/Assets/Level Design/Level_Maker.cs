using UnityEngine;
using System.Collections;

public class Level_Maker : MonoBehaviour {
    public TextAsset LevelDataText = null;
    private Level_Convert LevelConvert = null;

	// Use this for initialization
	void Start () {
        this.LevelConvert = new Level_Convert();
        this.LevelConvert.Level_Data_Load(LevelDataText);
	}

}
