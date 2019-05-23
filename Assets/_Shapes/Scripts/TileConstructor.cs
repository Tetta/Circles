using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConstructor : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject tileConstructPrefab;
    public GameObject starPrefab;
    public GameObject coinPrefab;
    public GameObject dotPrefab;
    public Transform tileField;

    //public Transform player;



    //public Dictionary<int, List<int>> tiles = new Dictionary<int, List<int>> ();
    [HideInInspector]
    public static  List<Vector2> tiles = new List<Vector2>();
    public List <Vector2 > stars = new List<Vector2>();
    //public List<KeyValuePair<int, int>> tiles = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(0, 0 ) };
    //public KeyValuePair<int, int> tiles2 = new KeyValuePair<int, int>  ( 0, 0) ;
    public static int tileSize = 30;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = -100; i < 100; i ++) {
            for (int j = -100; j < 100; j++) {
                tiles.Add(new Vector2(i, j));
                create(tiles, tileConstructPrefab, tileField);
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



}
