using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using SgLib;
using System;
using System.Collections.Generic;

public class gLevel : MonoBehaviour
{

    public static gLevel instance;
    //public static Level[] levelsData;
    public static List <Level> levels;
    public static int level = 1;
	//public static int levelDataIndex = 0;
 
    public Text txtLevel;

    private void Awake()
    {
        if (instance)
        {

        }
        else
        {
            instance = this;

            level = PlayerPrefs.GetInt("LEVEL", 1);

            loadLevels();

			//if (levelsData.Length > level) levelDataIndex = level - 1;
			//else levelDataIndex = levelsData.Length - 1;


        }
	}



    void OnEnable()
    {

        txtLevel.text = level.ToString();

    }

     public void increaseLevel ()
    {
        level++;
        PlayerPrefs.SetInt("LEVEL", level);

    }




    private void loadLevels() {

        levels = new List<Level>();

        var levelsTxt = Tools.LoadAsText("levels", "txt");
        //Level levelPrev = null;
        int levelNumber = 0;
        Level l = new Level();
        foreach (var levelStr in levelsTxt.Split('\n')) {
            Debug.Log(levelStr);
            if (levelStr.Length < 5) continue;
            if (levelStr.Substring(0, 2) == "//") {
                //if ()
                l = new Level();
                l.level = levelNumber;
                l.preset =  new List<LevelPreset>();
                levelNumber++;
                //List<LevelPreset> lPreset = new List<LevelPreset>();
                continue;
            }
            var levelArray = levelStr.Split('\t');

            LevelPreset levelPreset = new LevelPreset();
            levelPreset.dot = int.Parse(levelArray[0]);
            levelPreset.color = int.Parse(levelArray[1]);
            levelPreset.position = new Vector3( float.Parse(levelArray[2]), float.Parse(levelArray[3]), 0);

            l.preset.Add(levelPreset);


        }


    }

}

//[Serializable]
//public class LevelsData
//{
//    public List<Level> data;
//}

[Serializable]
public class Level
{
    public int level;
    public List<LevelPreset> preset;

}
[Serializable]
public class LevelPreset

{

    public int dot;
    public int color;
    public Vector3 position;

}
