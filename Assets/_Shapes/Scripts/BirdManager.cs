using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
public class BirdManager : MonoBehaviour
{
    Vector2 pos1;
    Vector2 pos2;
    int speed = 1;
    Vector2 pos;
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {




    }


    void Update()
    {
        Vector2 nextPos = (Vector2)GetComponent<IsoTransform>().Position + Time.deltaTime * direction * speed;
        Vector2 diffV = (pos - nextPos) * direction;

        if (diffV.x < 0 || diffV.y < 0) {
            GetComponent<IsoTransform>().Position = (Vector3)pos + new Vector3(0, 0, 1);
            direction = -direction;
            if (pos == pos2) pos = pos1;
            else pos = pos2;
            //Debug.Log(direction.normalized);
            changeSprite(direction.normalized);
        }
        else
            GetComponent<IsoTransform>().Position = (Vector3)nextPos + new Vector3(0, 0, 1);
    }

    public void create (Vector2 p1, Vector2 p2) {
        pos1 = p1;
        pos2 = p2;
        direction = pos2 - pos1;
        pos = pos2;
    }
    private void changeSprite(Vector2 dir) {
        int s = 0;
        if (dir == new Vector2(0, 1)) s = 3;
        if (dir == new Vector2(0, -1)) s = 1;
        if (dir == new Vector2(-1, 0)) s = 0;
        if (dir == new Vector2(1, 0)) s = 2;

        //Debug.Log("changeSprite");
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        transform.GetChild(s).gameObject.SetActive(true);
    }
}
