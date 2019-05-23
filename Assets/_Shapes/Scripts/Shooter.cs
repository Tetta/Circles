using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using System;
using DG.Tweening;
public class Shooter : MonoBehaviour
{
    Vector2 pos1;
    Vector2 pos2;
    int speed = 4;
    Vector2 pos;
    Vector2 direction;
    bool state = true;
    private void OnEnable() {
        state = true;
    }

    void Update() {
        //Debug.Log(state);
        if (state) {
            Vector2 nextPos = (Vector2)transform.GetChild(1).GetComponent<IsoTransform>().Position + Time.deltaTime * direction * speed;
            Vector2 diffV = (pos - nextPos) * direction;


            if (diffV.x < 0 || diffV.y < 0) {
                //transform.GetChild(1). GetComponent<IsoTransform>().Position = (Vector3)pos - GetComponent<IsoTransform>().Position  + new Vector3(0, 0, 1);
                //direction = -direction;
                //if (pos == pos2) pos = pos1;
                //else pos = pos2;
                transform.GetChild(1).GetComponent<IsoTransform>().Position = pos1;
                transform.GetChild(1).gameObject.SetActive(false);
                StartCoroutine(wait());
            }
            else {
                transform.GetChild(1).GetComponent<IsoTransform>().Position = nextPos;
                //transform.GetChild(2).position = transform.GetChild(1).position;
            }
        }
    }

    IEnumerator wait () {
        state = false;
        yield return new WaitForSeconds(0.2f);
        //transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        transform.GetChild(1).gameObject.SetActive(true);
        //transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(0).localScale = new Vector3(0, 0, 1);
        transform.GetChild(1).GetChild(0).DOScale(0.1f, 0.2f).OnComplete(() => {
            state = true;
            
        });
        
    }

    public void create(int id, Vector2 pStart) {
        GetComponent<IsoTransform>().Position += new Vector3(0, 0, 1);
        switch (id) {
            case 0:
                direction = new Vector3(0, -1);
                break;
            case 1:
                direction = new Vector3(0, 1);
                break;
            case 2:
                direction = new Vector3(1, 0);
                break;
            case 3:
                direction = new Vector3(-1, 0);
                break;
        }

        int c = 0;

        bool flag = true;
        Vector2 tempPos;
        pos2 = pStart;

        while (flag) {
            tempPos = pos2 + direction;
            if (LevelController.levelData.tiles.Contains(tempPos)) {
                pos2 = tempPos;


                c++;
                if (c == 100) flag = false;
            }

            else {
                //pos2 += direction / 5;
                flag = false;
            }

        }


        pos1 = pStart;
        //pos2 = p2;
        direction = pos2 - pos1;
        pos = pos2;

    }

}
