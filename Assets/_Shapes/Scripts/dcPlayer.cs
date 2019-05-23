using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
public class dcPlayer : MonoBehaviour
{
    Vector2 endPos;
    State state;
    Vector2 direction;
    //int tilesCount;
    // Start is called before the first frame update
    bool mouseDrag;
    Vector3 mousePos;
    Direction directionName = Direction.None;
    bool isDestroyCube;
    void Start()
    {
        endPos = new Vector2(0, 0);
        //transform.localPosition = pos * LevelController.tileSize;
        state = State.Stay;
    }
    void OnMouseDown() {
        if (state == State.Stay) {
            Debug.Log(Input.mousePosition);
            mousePos = Input.mousePosition;

            mouseDrag = true;

        }
    }
    void OnMouseUp() {
        if (state == State.Stay) {
            Debug.Log(Input.mousePosition);
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

                changeSprite(3);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || directionName == Direction.Down) {
                f = true;
                direction = new Vector3(0,  -1);

                changeSprite(1);

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || directionName == Direction.Left) {
                f = true;
                direction = new Vector3(-1,  0);
                changeSprite(0);

            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || directionName == Direction.Right) {
                f = true;
                direction = new Vector3(1,  0);
                changeSprite(2);

            }
        }
        if (f) {
           
            //tilesCount = 0;
            Vector2 tempPos = new Vector3(0, 0, 0);
            bool flag = true;
            while (flag) {
                tempPos = endPos + direction;
                if (dcLevelController.cubes.Contains(tempPos)) {
                    isDestroyCube = true;
                    endPos = tempPos - direction;
                    flag = false;
                }
                else if (dcLevelController.tiles.Contains(tempPos)) {
                    //if (tiles.ContainsKey((int)tempPos.x) && tiles[(int)tempPos.x].Contains((int)tempPos.y)) {
                    endPos = tempPos;
                    //tilesCount++;
                    //transform.localPosition = pos * LevelController.tileSize;
                }
                else
                    flag = false;

            }

            f = false;
            state = State.Run;
        }
        if (state == State.Run) {
            //for (int i = 0; i < tilesCount; i ++) {

            //}
            float speed = 15f;
            //gameObject. GetComponent<IsoTransform>().Translate(new Vector3(pos.x, 0, -pos.y) * Time.deltaTime * speed);
            //state = State.Stay;
            //GetComponent<IsoTransform>().Position = (Vector3)pos + new Vector3(0,0,1);

            Vector2 nextPos = (Vector2)GetComponent<IsoTransform>().Position + Time.deltaTime * direction * speed;
            Vector2 diffV = (endPos - nextPos) * direction;
            //Debug.Log(diffV);
            //diffV = new Vector3 (diffV.x * direction.x, diffV.y * direction.y, diffV.z * direction.z) ;
            //state = State.Stay;

            if (diffV.x < 0 || diffV.y < 0) {
                GetComponent<IsoTransform>().Position = (Vector3)endPos + new Vector3(0, 0, 1);
                if (isDestroyCube) {
                    Debug.Log("Destroing cube: " + (endPos + direction));
                    dcLevelController.cubes.Remove(endPos + direction );
                    dcLevelController.cubesGO[endPos + direction].gameObject.SetActive(false);
                    dcLevelController.cubesGO.Remove(endPos + direction );

                }
                isDestroyCube = false;
                stay();
                dcLevelController.instance.createRandomCube();
                //transform.localPosition = pos ;
            } else
                GetComponent<IsoTransform>().Position = (Vector3)nextPos + new Vector3(0, 0, 1);

        }


    }
    private void stay() {
        state = State.Stay;
        directionName = Direction.None;
    }



    private void changeSprite(int s) {
        Debug.Log("changeSprite");
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        transform.GetChild(s).gameObject.SetActive(true);
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name);
    }*/

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.gameObject.name);
        //star
        if (collision.gameObject.name == "StarPrefab(Clone)") {
            //GameController. stars++;
            collision.gameObject.SetActive(false);
        }
        //cube
        /*
        if (collision.gameObject.name == "CubePrefab(Clone)") {
            GameController.stars++;
            Debug.Log(collision.GetComponent<IsoTransform>().Position);
            Debug.Log(transform.GetComponent<IsoTransform>().Position);
            transform.GetComponent<IsoTransform>().Position = collision.GetComponent<IsoTransform>().Position - (Vector3)direction;
            collision.gameObject.SetActive(false);
            stay();

        }
        */
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
