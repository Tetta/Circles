﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
using DG.Tweening;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public static Player instance;
    public static State state;
    public static Direction directionName = Direction.None;
    

    public Transform images;
    public ParticleSystem psDeath;
    public Transform psTrail;
    public Transform cameraMain;


    Vector2 pos;
    Vector2 direction;
    //int tilesCount;
    // Start is called before the first frame update
    bool mouseDrag;
    Vector3 mousePos;

    Direction dirTemp = Direction.None;
    float speed = 25f;
    //public static int lives;

    void Start()
    {
        instance = this;
        pos = LevelController. levelData.start[0];
        move((Vector3)pos + new Vector3(0, 0, 1));
        state = State.Stay;

        GetComponent<Animator>().Play("PlayerIdle");

        changeChar();
    }
    void OnMouseDown() {
        if (state == State.Stay) {
            //Debug.Log(Input.mousePosition);
            mousePos = Input.mousePosition;

            mouseDrag = true;

        }
    }
    void OnMouseUp() {
        if (state == State.Stay) {
            //Debug.Log(Input.mousePosition);
            //mousePos = Input.mousePosition;
            Vector3 v = mousePos - Input.mousePosition;
            if (v.x > 0 && v.y < 0) directionName = Direction.Up;
            else if (v.x < 0 && v.y > 0) directionName = Direction.Down;
            else if (v.x > 0 && v.y > 0) directionName = Direction.Left;
            else if (v.x < 0 && v.y < 0) directionName = Direction.Right;
            mouseDrag = false;

        }
    }
    // Update is called once per frame
    void Update() {
        bool f = false;
        if (state == State.Stay) {
            if (Input.GetKeyDown(KeyCode.UpArrow) || directionName == Direction.Up) {
                f = true;
                direction = new Vector3(0, 1);

                changeSprite(0);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || directionName == Direction.Down) {
                f = true;
                direction = new Vector3(0,  -1);

                changeSprite(1);

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || directionName == Direction.Left) {
                f = true;
                direction = new Vector3(-1,  0);
                changeSprite(2);

            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || directionName == Direction.Right) {
                f = true;
                direction = new Vector3(1,  0);
                changeSprite(3);

            }
        }
        
        
        if (f) {
            dirTemp = directionName;
            directionName = Direction.None;
            Vector2 tempPos = new Vector3(0, 0, 0);
            bool flag = true;
            int c = 0;
            while (flag) {
                tempPos = pos + direction;
                if (LevelController. levelData.tiles.Contains(tempPos)) {
                    pos = tempPos;

                    c++;
                    if (c == 100) flag = false;
                }
                else if (LevelController.levelData.dangers.Contains(tempPos)) {
                    flag = false;
                    Debug.Log("------- Failed!!! -------------");
                    StartCoroutine(death());
                }
                else
                    flag = false;

            }

            f = false;
            state = State.Run;

        }
        if (state == State.Run) {

            Vector2 nextPos = (Vector2)GetComponent<IsoTransform>().Position + Time.deltaTime * direction * speed;
            Vector2 diffV = (pos - nextPos) * direction;


            if (diffV.x < 0 || diffV.y < 0) {
                move((Vector3)pos + new Vector3(0, 0, 1));
                //if teleport => change direction
                if (Teleport.checkTeleport(pos, dirTemp)) {
                    speed = 40;

                }
                else
                {

                    onStopPlayer();
                }

                
                state = State.Stay;
                
            } else
                move((Vector3)nextPos + new Vector3(0, 0, 1));
        }


    }
    private void move(Vector3 v) {

        GetComponent<IsoTransform>().Position = v;
        cameraMain.position = transform.position;
    }

    private void changeSprite(int s) {
        //Debug.Log("changeSprite");
        foreach (Transform child in images) {
            child.gameObject.SetActive(false);
        }
        images.GetChild(s).gameObject.SetActive(true);
        //ps trail
        foreach (Transform child in psTrail) {
            child.gameObject.SetActive(false);
        }
        psTrail.GetChild(s).gameObject.SetActive(true);

        //GetComponent<Animator>().Play("Empty");
        GetComponent<Animator>().enabled = false;
        //resize images
        images.DOScaleY(0.8f, 0.05f);
        images.gameObject.transform.DOLocalMoveY(-0.172f, 0.05f);
    }

    void onStopPlayer () {
        //ps trail
        foreach (Transform child in psTrail) {
            child.gameObject.SetActive(false);
        }

        //resize images
        images.gameObject.transform.DOScaleY(1, 0.05f).OnComplete(() => {
            GetComponent<Animator>().Rebind();
            GetComponent<Animator>().enabled = true;
            //GetComponent<Animator>().Play("PlayerIdle");
        });

        images.DOLocalMove(new Vector3(0, 0, 0), 0.05f);

        
        //GetComponent<Animator>().Play("PlayerIdle");
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log("collision.name: " + collision.gameObject.name);
        //star
        if (collision.gameObject.name == "TouchPanel") return;
        else if (collision.gameObject.name == "CoinPrefab(Clone)" || collision.gameObject.name == "DotPrefab(Clone)") {

            StartCoroutine(collision.gameObject.GetComponent<Coin>().effectCoroutine());

            if (collision.gameObject.name == "CoinPrefab(Clone)") GemsController.AddGems(10, "Level");
            else GemsController.AddGems(1, "Level");
        }
        else if (collision.gameObject.name == "BirdPrefab(Clone)") {
            Debug.Log("------- Failed!!! Bird -------------");

            collision.gameObject.SetActive(false);
            StartCoroutine(death());
        }
        else if (collision.gameObject.name == "Shoot") {
            Debug.Log("------- Failed!!! Shoot -------------");

            StartCoroutine( death());
        }
        else if (collision.transform.gameObject.name == "ExitPrefab(Clone)") {
            Debug.Log("------- Exit!!! -------------");

            GameController.instance.complete();
            
        }
        else if (collision.transform.parent.parent.gameObject.name == "FatPrefab(Clone)") {
            Debug.Log("------- Failed!!! Fat -------------");

            StartCoroutine(death());
        }

    }

    public IEnumerator death() {
        GetComponent<Collider2D>().enabled = false;

        if (GameController.instance.lives > 0) {
            Debug.Log("minusLives");
            GameController.instance.minusLives();
            revive();
        } else { 

            Debug.Log("IEnumerator death start");
            images.gameObject.SetActive(false);

            psDeath.Play();
            yield return new WaitForSeconds(0.7f);
            GameController.instance.showScreen("GameoverUI");
        }

    }

    public void revive() {
        StartCoroutine(reviveCoroutine());
    }
    public IEnumerator reviveCoroutine() {
        Debug.Log("IEnumerator revive start");
        GameController.instance.showScreen("GameUI");
        images.gameObject.SetActive(true);
        foreach (Transform child in images) {
            child.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 150);
        }
        yield return new WaitForSeconds(2f);
        foreach (Transform child in images) {
            child.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
        GetComponent<Collider2D>().enabled = true;
    }

    public void changeChar () {
        int i = 0;
        foreach (Transform child in images ) {
            child.GetComponent<SpriteRenderer>().sprite = GameController.instance.chars[GameController.charId].sprites[i];
            i++;
        }
        GameController.instance.playerMainMenu.sprite = GameController.instance.chars[GameController.charId].sprites[2];
    }

    public enum State {

        Stay = 0,

        Run = 1

    }
    public enum Direction {

        Up = 0,

        Down = 1,
        Left = 2,
        Right = 3,
        None = 4
    }
}