using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Linq;
using Assets.UltimateIsometricToolkit.Scripts.Core;
public class LevelConstructor : MonoBehaviour
{
    public GameObject tileConstructPrefab;
    //public GameObject starPrefab;
    //public GameObject coinPrefab;
    //public GameObject dotPrefab;
    public Transform tileField;
    public List<GameObject> prefabs;
    public Transform decors;
    public Transform player;
    GameObject prefab;
    



    //public Dictionary<int, List<int>> tiles = new Dictionary<int, List<int>> ();
    [HideInInspector]
    public static  List<Vector2> tiles = new List<Vector2>();

    public static int tileSize = 30;
    // Start is called before the first frame update

    /*public*/ static LevelData levelData;
    public bool construct;

    int toggleNumber;
    void Start()
    {
        levelData = new LevelData();

        load(LevelController.level);
        if (construct) {

            for (int i = -35; i < 35; i++) {
                for (int j = -35; j < 35; j++) {
                    tiles.Add(new Vector2(i, j));

                }
            }

            create(tiles, tileConstructPrefab, tileField);

            prefab = prefabs[0];


            //load level
            if (levelData != null) {
                create(levelData.tiles, prefabs[0], tileField);
                create(levelData.coins, prefabs[1], tileField);
                create(levelData.dots, prefabs[2], tileField);
                create(levelData.dangers, prefabs[3], tileField);
                create(levelData.birds, prefabs[4], tileField);
                create(levelData.birds2, prefabs[5], tileField);
                create(levelData.teleport1, prefabs[6], tileField);
                create(levelData.teleport2, prefabs[7], tileField);
                create(levelData.teleport3, prefabs[8], tileField);
                create(levelData.teleport4, prefabs[9], tileField);
                create(levelData.peaks, prefabs[10], tileField);
                create(levelData.fats, prefabs[11], tileField);
                create(levelData.start, prefabs[12], tileField);
                create(levelData.exit, prefabs[13], tileField);
                create(levelData.shooter0, prefabs[14], tileField);
                create(levelData.shooter1, prefabs[15], tileField);
                create(levelData.shooter2, prefabs[16], tileField);
                create(levelData.shooter3, prefabs[17], tileField);
                create(levelData.teleportAnother1, prefabs[18], tileField);
                create(levelData.teleportAnother2, prefabs[19], tileField);

            }
        }
    }

    void create (List<Vector2> list, GameObject prefab, Transform parent ) {

        foreach (Vector2 point in list) {
            GameObject t = Instantiate(prefab);
            t.transform.SetParent(parent);
            t.transform.localScale = new Vector3(1, 1, 1);
            t.transform.localPosition = new Vector3(point.x * tileSize, point.y * tileSize);
            
        }
    }

    public void createTile(Transform go) {
        Debug.Log(prefab.name);
        if (prefab.name == "Delete") {
            Vector2 v = new Vector2((int)(go.localPosition.x / LevelController.tileSize), (int)(go.localPosition.y / LevelController.tileSize));
            foreach (Transform child in tileField) {
                if (child.localPosition == go.localPosition && child.name != "TileConstructPrefab(Clone)") 
                    GameObject.Destroy(child.gameObject);
            }
            //Destroy(gameObject);
            
            levelData.tiles.Remove(v);
            levelData.coins.Remove(v);
            levelData.dots.Remove(v);
            levelData.dangers.Remove(v);
            levelData.birds.Remove(v);
            levelData.birds2.Remove(v);
            levelData.teleport1.Remove(v);
            levelData.teleport2.Remove(v);
            levelData.teleport3.Remove(v);
            levelData.teleport4.Remove(v);
            levelData.peaks.Remove(v);
            levelData.fats.Remove(v);
            levelData.start.Remove(v);
            levelData.exit.Remove(v);
            levelData.shooter0.Remove(v);
            levelData.shooter1.Remove(v);
            levelData.shooter2.Remove(v);
            levelData.shooter3.Remove(v);
            levelData.teleportAnother1.Remove(v);
            levelData.teleportAnother2.Remove(v);
        }
        else {

            GameObject t = Instantiate(prefab);
            t.transform.SetParent(tileField);
            t.transform.localScale = new Vector3(1, 1, 1);
            Vector2 v = new Vector2((int)(go.localPosition.x / LevelController.tileSize), (int)(go.localPosition.y / LevelController.tileSize));
            //t.transform.localPosition = new Vector3(0, 0);
            t.transform.localPosition = go.localPosition;

            if (toggleNumber == 0) levelData.tiles.Add(v);
            if (toggleNumber == 1) levelData.coins.Add(v);
            if (toggleNumber == 2) levelData.dots.Add(v);
            if (toggleNumber == 3) levelData.dangers.Add(v);
            if (toggleNumber == 4) levelData.birds.Add(v);
            if (toggleNumber == 5) levelData.birds2.Add(v);
            if (toggleNumber == 6) levelData.teleport1.Add(v);
            if (toggleNumber == 7) levelData.teleport2.Add(v);
            if (toggleNumber == 8) levelData.teleport3.Add(v);
            if (toggleNumber == 9) levelData.teleport4.Add(v);
            if (toggleNumber == 10) levelData.peaks.Add(v);
            if (toggleNumber == 11) levelData.fats.Add(v);
            if (toggleNumber == 12) levelData.start.Add(v);
            if (toggleNumber == 13) levelData.exit.Add(v);
            if (toggleNumber == 14) levelData.shooter0.Add(v);
            if (toggleNumber == 15) levelData.shooter1.Add(v);
            if (toggleNumber == 16) levelData.shooter2.Add(v);
            if (toggleNumber == 17) levelData.shooter3.Add(v);
            if (toggleNumber == 18) levelData.teleportAnother1.Add(v);
            if (toggleNumber == 19) levelData.teleportAnother2.Add(v);

        }
        save(LevelController.level);
    }


    public void changePrefab(Toggle t) {
        if (t.isOn) {
            toggleNumber = t.transform.GetSiblingIndex();
            prefab = prefabs[toggleNumber];
        }

    }
    
    public void saveDecoration () {
        levelData.decor0 = new List<Vector2>();
        levelData.decor0Size = new List<Vector2>();

        foreach (Transform decor in decors) {

            if ( decor.name.IndexOf("Decor0") != -1) {

                Debug.Log(decor.GetComponent<IsoTransform>().Position);
                levelData.decor0.Add(decor.GetComponent<IsoTransform>().Position);
                levelData.decor0Size.Add(decor.GetChild(0).localScale);
                //Debug.Log(decor.GetChild(0).localScale);
            }
        }
        save(LevelController.level);
    }
    
    
    //методы загрузки и сохранения статические, поэтому их можно вызвать откуда угодно
    public static void save(int level) {
        Debug.Log("save");
        Debug.Log(Application.persistentDataPath);
        int group = PlayerPrefs.GetInt("USER_GROUP_LEVELS", 1);
        string adding = "";
        //if (level <= 7) adding = "_" + group;

        //BinaryFormatter bf = new BinaryFormatter();
        XmlSerializer bf = new XmlSerializer(typeof(LevelData));
        //Application.persistentDataPath это строка; выведите ее в логах и вы увидите расположение файла сохранений
        FileStream file = File.Create(Application.persistentDataPath + "/Level" + level + ".txt");
        bf.Serialize(file, levelData);
        file.Close();
    }

    public static void load(int level) {
        int group = PlayerPrefs.GetInt("USER_GROUP_LEVELS", 2);
        Debug.Log("USER_GROUP_LEVELS: " + group);
        string adding = "";
        //if (level <= 7) adding = "_" + group;
        //from resources -----------------
#if !UNITY_EDITOR || UNITY_IOS
        string levelsTxt = Tools.LoadAsText("Levels/Level" + level + adding, "txt");
        //Debug.Log(levelsTxt);
        XmlSerializer bf = new XmlSerializer(typeof(LevelData));
        using (TextReader reader = new StringReader(levelsTxt)) {
            levelData = (LevelData)bf.Deserialize(reader);
        }

        LevelController.levelData = levelData;
        //file.Close();

        LevelController.levelLoaded = LevelController.level;
#else
        //------------------------



        //FileStream file = File.Open(Application.persistentDataPath + "/Level" + level + ".gd", FileMode.Open);
        //levelData = (LevelData)bf.Deserialize(levelsTxt);
        //bf.Deserialize("sdfsdf");
        //using (TextReader reader = new StringReader(levelsTxt)) {
        //    levelData = (LevelData)bf.Deserialize(reader);
        //}

        //LevelController.levelData = levelData;
        //file.Close();

        //LevelController.levelLoaded = LevelController.level;

        Debug.Log(Application.persistentDataPath);
        Debug.Log(File.Exists(Application.persistentDataPath + "/Level" + level + adding + ".txt"));
        if (File.Exists(Application.persistentDataPath + "/Level" + level + adding + ".txt")) {
            //BinaryFormatter bf = new BinaryFormatter();
            XmlSerializer bf = new XmlSerializer(typeof(LevelData));
            FileStream file = File.Open(Application.persistentDataPath + "/Level" + level + adding + ".txt", FileMode.Open);
            levelData = (LevelData)bf.Deserialize(file);
            LevelController. levelData = levelData;
            file.Close();

            LevelController.levelLoaded = LevelController.level;
        }
#endif

    }



}
