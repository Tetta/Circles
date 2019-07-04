using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using DG.Tweening;
public class TeleportAnother : MonoBehaviour
{
    public static bool enter;
    public static float enterTime;
    Color32 color1;
    Color32 color2;


    void Start()
    {

        
        foreach (Transform c in transform) {
            c.gameObject.SetActive(false);
        }
        transform.GetChild(LevelController.skin).gameObject.SetActive(true);
    }


    public static Vector3  checkTeleport(Vector2 pos, Player.Direction dirTemp) {
        //Debug.Log("checkTeleport Another");
        //Debug.Log(pos);


        
       if (LevelController.levelData.teleportAnother1.Contains(pos)) {
            int index = LevelController.levelData.teleportAnother1.IndexOf(pos);
            pos = LevelController.levelData.teleportAnother2[index];
            //move((Vector3)LevelController.levelData.teleportAnother2[index] + new Vector3(0, 0, 1));
            Player.directionName = dirTemp;
            return LevelController.levelData.teleportAnother2[index];
        }
        else if (LevelController.levelData.teleportAnother2.Contains(pos)) {
            int index = LevelController.levelData.teleportAnother2.IndexOf(pos);
            pos = LevelController.levelData.teleportAnother1[index];
            //move((Vector3)LevelController.levelData.teleportAnother1[index] + new Vector3(0, 0, 1));
            Player.directionName = dirTemp;
            return LevelController.levelData.teleportAnother1[index];

        }
        return new Vector3 (0, 0, 100);




    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("1name: " + name);
        Debug.Log("1collision.name: " + collision.gameObject.name);
    }
    }
