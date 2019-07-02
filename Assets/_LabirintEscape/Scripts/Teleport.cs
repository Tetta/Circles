using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using DG.Tweening;
public class Teleport : MonoBehaviour
{
    Color32 color1;
    Color32 color2;


    void Start()
    {
        //Debug.Log("Teleport start");
        //transform.GetChild(0).DOLocalRotate(new Vector3(0f, 0f, -360f), 5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
        //transform.GetChild(0).GetComponent<SpriteRenderer>().DOColor(new Color32(255, 131, 211, 255), 0.3f).SetEase(Ease.InBounce).SetLoops(-1, LoopType.Restart);
        //yield return new WaitForSeconds(Random.Range(0, 1.3f));
        if (LevelController.skin == 0) {
            color1 = new Color32(252, 255, 141, 255);
            //color2 = new Color32(255, 196, 85, 255);
            color2 = new Color32(199, 255, 247, 255);
        } else
            if (LevelController.skin == 1) {
            color1 = new Color32(250, 151, 196, 255);
            color2 = new Color32(255, 222, 241, 255);
        } else if (LevelController.skin == 2) {
            color1 = new Color32(145, 222, 255, 255);
            color2 = new Color32(0, 181, 255, 255);
            //color2 = new Color32(57, 79, 151, 255);
        }
        StartCoroutine(setColors());

    }

    IEnumerator setColors() {
        //yield return new WaitForSeconds(Random.Range(0, 0.3f));
        //transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 131, 211, 255);
        //yield return new WaitForSeconds(0.3f);
        //transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 221, 243, 255);
        //yield return new WaitForSeconds(0.3f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = color1;
        yield return new WaitForSeconds(0.3f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = color2;
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(setColors());

    }


    public static bool checkTeleport(string title) {
        Debug.Log("checkTeleport: " + title);

        bool isTeleport = false;
        if (title == "TeleportPrefab1(Clone)") {
            if (Player.dirTemp == Player.Direction.Up) Player.directionName = Player.Direction.Right;
            if (Player.dirTemp == Player.Direction.Left) Player.directionName = Player.Direction.Down;
            isTeleport = true;
        }
        else if (title == "TeleportPrefab2(Clone)") {
            if (Player.dirTemp == Player.Direction.Up) Player.directionName = Player.Direction.Left;
            if (Player.dirTemp == Player.Direction.Right) Player.directionName = Player.Direction.Down;

            isTeleport = true;
        }
        else if(title == "TeleportPrefab3(Clone)") {
            if (Player.dirTemp == Player.Direction.Down) Player.directionName = Player.Direction.Right;
            if (Player.dirTemp == Player.Direction.Left) Player.directionName = Player.Direction.Up;
            isTeleport = true;

        }
        else if(title == "TeleportPrefab4(Clone)") {
            if (Player.dirTemp == Player.Direction.Down) Player.directionName = Player.Direction.Left;
            if (Player.dirTemp == Player.Direction.Right) Player.directionName = Player.Direction.Up;
            isTeleport = true;

        }
        if (isTeleport) {
            Player.speed = 40;
            Player.state = Player.State.Stay;
            return true;
        }


        return false;

    }
    public static bool checkTeleport2 (Vector2 pos, Player.Direction dirTemp) {
        if (LevelController.levelData.teleport1.Contains(pos)) {
            if (dirTemp == Player.Direction.Up) Player.directionName = Player.Direction.Right;
            if (dirTemp == Player.Direction.Left) Player.directionName = Player.Direction.Down;
            return true;
        }
        if (LevelController.levelData.teleport2.Contains(pos)) {
            if (dirTemp == Player.Direction.Up) Player.directionName = Player.Direction.Left;
            if (dirTemp == Player.Direction.Right) Player.directionName = Player.Direction.Down;

            return true;
        }
        if (LevelController.levelData.teleport3.Contains(pos)) {
            if (dirTemp == Player.Direction.Down) Player.directionName = Player.Direction.Right;
            if (dirTemp == Player.Direction.Left) Player.directionName = Player.Direction.Up;
            return true;

        }
        if (LevelController.levelData.teleport4.Contains(pos)) {
            if (dirTemp == Player.Direction.Down) Player.directionName = Player.Direction.Left;
            if (dirTemp == Player.Direction.Right) Player.directionName = Player.Direction.Up;
            return true;

        }
        return false;

    }
}
