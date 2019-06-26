using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;

using System;
public class dcLevelController : MonoBehaviour
{
    public static dcLevelController instance;

    public GameObject tilePrefab;
    public GameObject starPrefab;
    public GameObject coinPrefab;
    public GameObject dotPrefab;
    public GameObject cubePrefab;
    public Transform tileField;

    //public Transform player;



    //public Dictionary<int, List<int>> tiles = new Dictionary<int, List<int>> ();
    [HideInInspector]
    public static  List<Vector2> tiles = new List<Vector2>();
    public static List<Vector2> cubes = new List<Vector2>();
    public List <Vector2 > stars = new List<Vector2>();
    public static Dictionary<Vector2, GameObject> cubesGO = new Dictionary<Vector2, GameObject>();
    //public List<KeyValuePair<int, int>> tiles = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(0, 0 ) };
    //public KeyValuePair<int, int> tiles2 = new KeyValuePair<int, int>  ( 0, 0) ;
    public static int tileSize = 30;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        /*
        tiles.Add(new Vector2(-1, 0));
        tiles.Add(new Vector2(-1, 1));
        tiles.Add(new Vector2(-1, 2));
        tiles.Add(new Vector2(0, 0));
        tiles.Add(new Vector2(0, 1));
        tiles.Add(new Vector2(0, 2));
        tiles.Add(new Vector2(0, 3));
        tiles.Add(new Vector2(0, 4));
        tiles.Add(new Vector2(1, 0));
        tiles.Add(new Vector2(1, 1));
        tiles.Add(new Vector2(1, 2));
        tiles.Add(new Vector2(2, -3));
        tiles.Add(new Vector2(2, -2));
        tiles.Add(new Vector2(2, -1));
        tiles.Add(new Vector2(2, -0));
        */
        for (int i = 0; i < 5; i ++) {
            for (int j = 0; j < 5; j ++) {
                tiles.Add(new Vector2(i, j));
            }
        }


        cubes.Add(new Vector2(0, 1));
        cubes.Add(new Vector2(1, 0));
        cubes.Add(new Vector2(3, 0));
        cubes.Add(new Vector2(1, 1));
        cubes.Add(new Vector2(2, 3));
        cubes.Add(new Vector2(4, 2));

        stars.Add(new Vector2(1, 1));
        stars.Add(new Vector2(0, 1));

        create(tiles, tilePrefab, tileField);
        create(cubes, cubePrefab, tileField);
        //create(stars, starPrefab, tileField);


    }
    
    void create (List<Vector2> list, GameObject prefab, Transform parent ) {

        foreach (Vector2 point in list) {
            GameObject t = Instantiate(prefab);
            t.transform.SetParent(parent);
            t.transform.localScale = new Vector3(1, 1, 1);
            //t.transform.localPosition = new Vector3(point.x * tileSize, point.y * tileSize);
            t.GetComponent<IsoTransform>().Position = new Vector3(point.x, point.y, 0);


            if (prefab == cubePrefab) {
                t.GetComponent<IsoTransform>().Position = new Vector3(point.x, point.y, 1);
                
                cubesGO.Add(point, t);
            }
        }
    }


    public  void createRandomCube() {
        
        for (int i = 0; i < 5; i++) {
            int r = UnityEngine.Random.Range(0, tiles.Count);
            if (!cubes.Contains((Vector3)tiles[r] )) {
                //create
                GameObject t = Instantiate(cubePrefab);
                t.transform.SetParent(tileField);
                t.transform.localScale = new Vector3(1, 1, 1);
                //t.transform.localPosition = new Vector3(point.x * tileSize, point.y * tileSize);
                t.GetComponent<IsoTransform>().Position = (Vector3)tiles[r] + new Vector3(0, 0, 1);

                cubesGO.Add(tiles[r], t);
                cubes.Add(tiles[r]);
                i = 6;
            }
        }
    }
}
[Serializable]
public class dcLevelData {
    [SerializeField] public List<Vector2> tiles = new List<Vector2>();
    [SerializeField] public List<Vector2> stars = new List<Vector2>();
}



