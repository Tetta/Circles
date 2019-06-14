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
    int speed = 12;
    Vector2 pos;
    Vector2 direction;
    bool state = true;
    float timeShoot = 0;
    private void OnEnable() {
        state = true;
        //Time.timeScale = 0.1f;
        //timeShoot = 0;
    }

    void Update() {
        //Debug.Log(state);
        timeShoot += Time.deltaTime;
        if (state) {
            Vector2 nextPos = (Vector2)transform.GetChild(1).GetComponent<IsoTransform>().Position + Time.deltaTime * direction * speed;
            Vector2 diffV = (pos - nextPos) * direction;


            if (diffV.x < 0 || diffV.y < 0) {



                transform.GetChild(1).GetComponent<IsoTransform>().Position = pos1;


                //transform.GetChild(1).gameObject.SetActive(false);
                state = false;
                transform.GetChild(1).GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
                transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = 0;

                transform.GetChild(1).GetChild(0).localScale = new Vector3(0, 0, 1);
                transform.GetChild(1).GetChild(1).localScale = new Vector3(0, 0, 1);
                transform.GetChild(1).GetChild(2).localScale = new Vector3(0, 0, 1);
                transform.GetChild(1).GetChild(3).localScale = new Vector3(0, 0, 1);
                transform.GetChild(1).GetChild(4).localScale = new Vector3(0, 0, 1);
                //StartCoroutine(wait());
            }
            else {
                transform.GetChild(1).GetComponent<IsoTransform>().Position = nextPos;

            }
        } else {
            //timeShoot += Time.deltaTime;
            if (timeShoot >= 1) StartCoroutine(wait());
        }
    }

    IEnumerator wait () {
        state = false;

        //transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = 0;
        //transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TrailRenderer>().time = 0;
        //transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TrailRenderer>().time = 0;
        //transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TrailRenderer>().time = 0;
        //transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<TrailRenderer>().time = 0;
        //yield return new WaitForSeconds(0.2f);
        //transform.GetChild(2).gameObject.SetActive(false);
        //yield return new WaitForSeconds(0.3f);
        //transform.GetChild(1).gameObject.SetActive(true);
        //transform.GetChild(1).gameObject.SetActive(true);
        //transform.GetChild(2).gameObject.SetActive(true);
        //transform.GetChild(1).GetChild(0).localScale = new Vector3(0, 0, 1);
        //transform.GetChild(1).GetChild(1).localScale = new Vector3(0, 0, 1);
        //transform.GetChild(1).GetChild(2).localScale = new Vector3(0, 0, 1);
        //transform.GetChild(1).GetChild(3).localScale = new Vector3(0, 0, 1);
        //transform.GetChild(1).GetChild(4).localScale = new Vector3(0, 0, 1);

        transform.GetChild(1).GetChild(1).DOScale(0.05f, 0.4f);
        transform.GetChild(1).GetChild(2).DOScale(0.05f, 0.4f);
        transform.GetChild(1).GetChild(3).DOScale(0.05f, 0.4f);
        transform.GetChild(1).GetChild(4).DOScale(0.05f, 0.4f);
        timeShoot = 0;

        transform.GetChild(1).GetChild(0).DOScale(0.1f, 0.4f).OnComplete(() => {
            state = true;
            //timeShoot = 0;
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TrailRenderer>().time = 0.1f;
            transform.GetChild(1).GetChild(0).GetComponent<CircleCollider2D>().enabled = true;

            //transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TrailRenderer>().time = 0.1f;
            //transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<TrailRenderer>().time = 0.1f;
            //transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TrailRenderer>().time = 0.1f;
            //transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<TrailRenderer>().time = 0.1f;
        });
        //transform.GetChild(1).GetChild(0).DOScale(0.1f, 0.2f).OnComplete(() => {
        //    state = true;
            
        //});
        yield return new WaitForSeconds(0.4f);
       // state = true;
        //transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TrailRenderer>().emitting = true;

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
        direction = (pos2 - pos1).normalized;
        pos = pos2;

    }

}
