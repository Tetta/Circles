using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;

using System;
public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    //---------------------------------
    public static int level = 22; //18
    public static int skin = 2;
    //---------------------------------

    public static int tileSize = 30;
    public static LevelData levelData;

    public static int levelLoaded;

    public Transform tileField;



    public Transform prefabs;

    public Transform bg;
    public Transform psBg;


    private void Awake() {
        //fix uncomment
        //level = PlayerPrefs.GetInt("LEVEL", 1);
    }

    //public List<GameObject> prefabs;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        loadLevel(level);
        prefabs.gameObject.SetActive(false);

        //background skins
        foreach (Transform c in bg.transform) {
            c.gameObject.SetActive(false);
        }
        bg.transform.GetChild(skin).gameObject.SetActive(true);
        //ps background skins
        foreach (Transform c in psBg) {
            c.gameObject.SetActive(false);
        }
        psBg.transform.GetChild(skin).gameObject.SetActive(true);

        
    }
    
    public void create (List<Vector2> list, GameObject prefab, Transform parent,List<Vector2> listSize = null) {
        GameController.logTime("create " + prefab.name);
        //for skins
        if (prefab.name == "TilePrefab" || prefab.name == "Decor0" || prefab.name == "DangerWallPrefab") {

            int localCounter = 0;
            //foreach (Transform c in prefab.transform) {
            for (int i =  0; i < prefab.transform.childCount; i ++) {
                Transform c = prefab.transform.GetChild(i);
                //Debug.Log(c.name);

                //c.gameObject.SetActive(false);
                if (localCounter != skin) {
                    //Debug.Log(c.name);
                    DestroyImmediate (c.gameObject);
                    i--;
                }
                localCounter++;
            }
            //Debug.Log(prefab.name);
            prefab.transform.GetChild(0).gameObject.SetActive(true);
            //prefab.transform.GetChild(skin).gameObject.SetActive(true);

        }

        int counter = 0;

        foreach (Vector2 point in list) {
            if ((skin == 0 || skin == 2) && prefab.name == "Decor0") {
                int r = UnityEngine.Random.Range(0, 2);
                //Debug.Log(r);
                prefab.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                prefab.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                prefab.transform.GetChild(0).GetChild(r).gameObject.SetActive(true);

            }

            GameObject t = Instantiate(prefab);
            t.transform.SetParent(parent);
            if (listSize != null) t.transform.localScale = listSize[counter];
            else t.transform.localScale = new Vector3(1, 1, 1);

            //t.transform.localPosition = new Vector3(point.x * tileSize, point.y * tileSize);
            t.GetComponent<IsoTransform>().Position = new Vector3(point.x, point.y, 0);

            if (prefab.name == "CoinPrefab" || prefab.name == "DotPrefab") t.GetComponent<IsoTransform>().Position = new Vector3(point.x, point.y, 1);
            if (prefab.name == "BirdPrefab") t.GetComponent<BirdManager>().create(point, levelData.birds2[counter]);
            if (prefab.name == "ShooterPrefab0") t.GetComponent<Shooter>().create(0, point);
            if (prefab.name == "ShooterPrefab1") t.GetComponent<Shooter>().create(1, point);
            if (prefab.name == "ShooterPrefab2") t.GetComponent<Shooter>().create(2, point);
            if (prefab.name == "ShooterPrefab3") t.GetComponent<Shooter>().create(3, point);
            counter++;
        }
    }

    private void loadLevel(int level) {
        Debug.Log("loadLevel: " + level);
        GameController.logTime("loadLevel");
        
        if (levelLoaded != level) {
            levelData = new LevelData();
            LevelConstructor.load(level);
        }
        //load level
        if (levelData != null) {
            foreach(Transform prefab in prefabs) {

            }
            
            create(levelData.tiles, prefabs.GetChild(0).gameObject, tileField);
            create(levelData.coins, prefabs.GetChild(1).gameObject, tileField);
            create(levelData.dots, prefabs.GetChild(2).gameObject, tileField);
            create(levelData.dangers, prefabs.GetChild(3).gameObject, tileField);
            create(levelData.birds, prefabs.GetChild(4).gameObject, tileField);
            //create(levelData.decor0, prefabs.GetChild(5).gameObject, tileField, levelData.decor0Size);
            create(levelData.teleport1, prefabs.GetChild(6).gameObject, tileField);
            create(levelData.teleport2, prefabs.GetChild(7).gameObject, tileField);
            create(levelData.teleport3, prefabs.GetChild(8).gameObject, tileField);
            create(levelData.teleport4, prefabs.GetChild(9).gameObject, tileField);
            create(levelData.peaks, prefabs.GetChild(10).gameObject, tileField);
            create(levelData.fats, prefabs.GetChild(11).gameObject, tileField);
            create(levelData.start, prefabs.GetChild(12).gameObject, tileField);
            create(levelData.exit, prefabs.GetChild(13).gameObject, tileField);
            create(levelData.shooter0, prefabs.GetChild(14).gameObject, tileField);
            create(levelData.shooter1, prefabs.GetChild(15).gameObject, tileField);
            create(levelData.shooter2, prefabs.GetChild(16).gameObject, tileField);
            create(levelData.shooter3, prefabs.GetChild(17).gameObject, tileField);
            create(levelData.teleportAnother1, prefabs.GetChild(18).gameObject, tileField);
            create(levelData.teleportAnother2, prefabs.GetChild(19).gameObject, tileField);
            /*
            create(levelData.tiles, tilePrefab, tileField);
            create(levelData.coins, coinPrefab, tileField);
            create(levelData.dots, dotPrefab, tileField);
            create(levelData.dangers, dangerPrefab, tileField);
            create(levelData.birds, birdPrefab, tileField);
            */

            //decors
            //create(levelData.decor0, decorPrefabs[0], tileField, levelData.decor0Size);
            //fix    
            Decor.create();
            GameController.logTime("create prefabs finish");

        }
    }

    public static void addLevel () {
        level ++;
        PlayerPrefs.GetInt("LEVEL", level);
    }

    public void centerLevelPS(Vector3 pos) {
        psBg.GetComponent<IsoTransform>().Position = pos;
    }
}
[Serializable]
public class LevelData {
    [SerializeField] public List<Vector2> tiles = new List<Vector2>();
    [SerializeField] public List<Vector2> coins = new List<Vector2>();
    [SerializeField] public List<Vector2> dots = new List<Vector2>();
    [SerializeField] public List<Vector2> dangers = new List<Vector2>();
    [SerializeField] public List<Vector2> birds = new List<Vector2>();
    [SerializeField] public List<Vector2> birds2 = new List<Vector2>();
    [SerializeField] public List<Vector2> decor0 = new List<Vector2>();
    [SerializeField] public List<Vector2> decor0Size = new List<Vector2>();
    [SerializeField] public List<Vector2> teleport1 = new List<Vector2>();
    [SerializeField] public List<Vector2> teleport2 = new List<Vector2>();
    [SerializeField] public List<Vector2> teleport3 = new List<Vector2>();
    [SerializeField] public List<Vector2> teleport4 = new List<Vector2>();
    [SerializeField] public List<Vector2> peaks = new List<Vector2>();
    [SerializeField] public List<Vector2> fats = new List<Vector2>();
    [SerializeField] public List<Vector2> start = new List<Vector2>();
    [SerializeField] public List<Vector2> exit = new List<Vector2>();
    [SerializeField] public List<Vector2> shooter0 = new List<Vector2>();
    [SerializeField] public List<Vector2> shooter1 = new List<Vector2>();
    [SerializeField] public List<Vector2> shooter2 = new List<Vector2>();
    [SerializeField] public List<Vector2> shooter3 = new List<Vector2>();
    [SerializeField] public List<Vector2> teleportAnother1 = new List<Vector2>();
    [SerializeField] public List<Vector2> teleportAnother2 = new List<Vector2>();
}

/// <summary>
/// Since unity doesn't flag the Vector3 as serializable, we
/// need to create our own version. This one will automatically convert
/// between Vector3 and SerializableVector3
/// </summary>
[Serializable]
public struct Vector2S {
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;



    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>

    public Vector2S(float rX, float rY) {
        x = rX;
        y = rY;

    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
        return String.Format("[{0}, {1}, {2}]", x, y);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(Vector2S rValue) {
        return new Vector3(rValue.x, rValue.y);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector2S(Vector3 rValue) {
        return new Vector2S(rValue.x, rValue.y);
    }
}
