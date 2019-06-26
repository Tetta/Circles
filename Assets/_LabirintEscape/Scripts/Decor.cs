using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using System;
public class Decor : MonoBehaviour
{
    public static bool enable = true;

    //
    static int[,] all;
    static int xMin = 1000;
    static int xMax = -1000;
    static int yMin = 1000;
    static int yMax = -1000;
    static List<Vector2> dPos;
    static List<Vector2> dSize;

    
    public static void create2() {
        Debug.Log("Decor create");

        List<Vector2> tiles = LevelController.levelData.tiles;
        List<Vector2> dPos = new List<Vector2>();
        List<Vector2> dSize = new List<Vector2>();
        int xMin = 1000;
        int xMax = -1000;
        int yMin = 1000;
        int yMax = -1000;


        //find min and max 
        foreach (Vector2 tile in tiles) {
            if (tile.x < xMin) xMin = (int)tile.x;
            if (tile.x > xMax) xMax = (int)tile.x;
            if (tile.y < yMin) yMin = (int)tile.y;
            if (tile.y > yMax) yMax = (int)tile.y;
        }

        xMin -= 10;
        xMax += 10;
        yMin -= 10;
        yMax += 10;

        int[,] all = new int[xMax - xMin, yMax - yMin];
        foreach (Vector2 tile in tiles) {
            all[(int)tile.x - xMin, (int)tile.y - yMin] = 1;
        }

        bool exist = false;
        //Debug.Log(xMin);
        //Debug.Log(xMax);
        //Debug.Log(yMin);
        //Debug.Log(yMax);

        Debug.Log(Time.realtimeSinceStartup);
        for (int x = 0; x < xMax - xMin - 3; x++) {

            for (int y = 0; y < yMax - yMin - 3; y++) {
                exist = false;

                //check 4x4
                for (int xx = x; xx <= x + 3; xx++) {
                    for (int yy = y; yy <= y + 3; yy++) {


                        if (all[xx, yy] == 1) exist = true;
                    }
                }
                if (!exist) {
                    dPos.Add(new Vector2(x + 1.5f + UnityEngine.Random.Range(-0.5f, 0.5f) + xMin, y + 1.5f + UnityEngine.Random.Range(-0.5f, 0.5f) + yMin) );

                    float size = UnityEngine.Random.Range(1.7f, 2.5f);

                    dSize.Add(new Vector2(size, size));

                    for (int xx = x; xx <= x + 3; xx++) {
                        for (int yy = y; yy <= y + 3; yy++) {

                            all[xx, yy] = 1;
                        }
                    }

                }

                else {
                    exist = false;

                    //check 3x3
                    for (int xx = x; xx <= x + 2; xx++) {
                        for (int yy = y; yy <= y + 2; yy++) {
                            //if (tiles.Contains(new Vector2(xx, yy))) exist = true;
                            if (all[xx, yy] == 1) exist = true;
                        }
                    }
                    if (!exist) {
                        dPos.Add(new Vector2(x + 1 + UnityEngine.Random.Range(-0.2f, 0.2f) + xMin, y + 1 + UnityEngine.Random.Range(-0.2f, 0.2f) + yMin));
                        //dPos.Add(new Vector2(x + 1, y + 1 ));
                        float size = UnityEngine.Random.Range(1.2f, 1.7f);
                        //float size = 1;
                        dSize.Add(new Vector2(size, size));

                        for (int xx = x; xx <= x + 2; xx++) {
                            for (int yy = y; yy <= y + 2; yy++) {
                                //tiles.Add(new Vector2(xx, yy));
                                all[xx, yy] = 1;
                            }
                        }

                    }

                    else

                    //check 2x2
                    {
                        exist = false;
                        //check 2x2 around
                        for (int xx = x; xx <= x + 1; xx++) {
                            for (int yy = y; yy <= y + 1; yy++) {
                                //if (tiles.Contains(new Vector2(xx, yy))) exist = true;
                                if (all[xx, yy] == 1) exist = true;
                            }
                        }
                        if (!exist && UnityEngine.Random.Range(0, 1f) > 0.5f) {
                            dPos.Add(new Vector2(x + 0.5f + UnityEngine.Random.Range(-0.2f, 0.2f) + xMin, y + 0.5f + UnityEngine.Random.Range(-0.2f, 0.2f) + yMin));
                            float size = UnityEngine.Random.Range(0.5f, 1.5f);
                            dSize.Add(new Vector2(size, size));

                            for (int xx = x; xx <= x + 1; xx++) {
                                for (int yy = y; yy <= y + 1; yy++) {
                                    //tiles.Add(new Vector2(xx, yy));
                                    all[xx, yy] = 1;
                                }
                            }


                        }
                    }

                }
            }

        }


        //create
        LevelController.instance.create(dPos, LevelController.instance.prefabs.GetChild(5).gameObject, LevelController.instance.tileField, dSize);


    }


    public static void create() {
        Debug.Log("Decor create");
        GameController.logTime("decor create start");

        List<Vector2> tiles = LevelController.levelData.tiles;
        tiles.AddRange(LevelController.levelData.dangers);

        dPos = new List<Vector2>();
        dSize = new List<Vector2>();
        xMin = 0;
        xMax = 0;
        yMin = 0;
        yMax = 0;

        //find min and max 
        foreach (Vector2 tile in tiles) {
            if (tile.x < xMin) xMin = (int)tile.x;
            if (tile.x > xMax) xMax = (int)tile.x;
            if (tile.y < yMin) yMin = (int)tile.y;
            if (tile.y > yMax) yMax = (int)tile.y;
        }

        LevelController.instance.centerLevelPS(new Vector3((xMax + xMin) / 2, (yMax + yMin) / 2, 0));

        xMin -= 10;
        xMax += 10;
        yMin -= 10;
        yMax += 10;

        all = new int[xMax - xMin, yMax - yMin];
        foreach (Vector2 tile in tiles) {
            all[(int)tile.x - xMin, (int)tile.y - yMin] = 1;
        }

        bool exist = false;
        //Debug.Log(xMin);
        //Debug.Log(xMax);
        //Debug.Log(yMin);
        //Debug.Log(yMax);

        for (int x = 0; x < xMax - xMin - 3; x++) {

            for (int y = 0; y < yMax - yMin - 3; y++) {
                if (LevelController.skin == 0) {
                    exist = check(x, y, 3, 0.5f, 0.8f, 1.2f, 1, 0.5f);
                    if (exist) exist = check(x, y, 2, 0.2f, 0.7f, 1.1f, 1, 0.3f);
                    if (exist) exist = check(x, y, 1, 0.1f, 0.7f, 1.1f, 1, 0.3f);
                }
                else if (LevelController.skin == 1) {
                    exist = check(x, y, 3, 0.5f, 1.7f, 2.5f, 1, 1f);
                    if (exist) exist = check(x, y, 2, 0.2f, 1.2f, 1.7f, 1, 1f);
                    if (exist) exist = check(x, y, 1, 0.2f, 0.5f, 1.5f, 1, 1f);
                }
                if (LevelController.skin == 2) {
                    //exist = check(x, y, 3, 0.5f, 0.8f, 1.2f, 4, 0.5f);
                    //if (exist) exist = check(x, y, 2, 0.2f, 0.7f, 1.1f, 3, 0.3f);
                    //if (exist) exist = check(x, y, 1, 0.1f, 0.7f, 1.1f, 2, 0.3f);

                    exist = check(x, y, 3, 0.5f, 0.8f, 1.2f, 3, 0.2f);
                    if (exist) exist = check(x, y, 2, 0.2f, 0.7f, 1.1f, 2, 0.1f);
                    if (exist) exist = check(x, y, 1, 0.1f, 0.7f, 1.1f, 1, 0.1f);
                }


            }

        }


        //create

        if (enable) LevelController.instance.create(dPos, LevelController.instance.prefabs.GetChild(5).gameObject, LevelController.instance.tileField, dSize);


    }

    static bool check (int x, int y, int s, float diffPos, float sizeMin, float sizeMax, int amount, float createChance) {
        bool exist = false;
        //check 4x4
        for (int xx = x; xx <= x + s; xx++) {
            for (int yy = y; yy <= y + s; yy++) {


                if (all[xx, yy] == 1) exist = true;
            }
        }
        if (!exist) exist = UnityEngine.Random.Range(0, 1f) > createChance;
        if (!exist) {
            float xPosDiff = UnityEngine.Random.Range(-diffPos, diffPos);
            float yPosDiff = UnityEngine.Random.Range(-diffPos, diffPos);



            dPos.Add(new Vector2(x + s / 2f+ xPosDiff + xMin, y + s / 2f + yPosDiff + yMin));

            float size = UnityEngine.Random.Range(sizeMin, sizeMax);

            dSize.Add(new Vector2(size, size));


            for (int i = 0; i < UnityEngine.Random.Range(0, amount); i++) {
                //Debug.Log(i);
                xPosDiff = xPosDiff + UnityEngine.Random.Range(-1, 1f);
                yPosDiff = yPosDiff + UnityEngine.Random.Range(-1, 1f);

                dPos.Add(new Vector2(x + s / 2f + xPosDiff + xMin, y + s / 2f  + yPosDiff + yMin));

                size = UnityEngine.Random.Range(sizeMin, sizeMax);

                dSize.Add(new Vector2(size, size));
            }

            for (int xx = x; xx <= x + s; xx++) {
                for (int yy = y; yy <= y + s; yy++) {

                    all[xx, yy] = 1;
                }
            }

        }


        return exist;
    }

}
