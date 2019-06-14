using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.UltimateIsometricToolkit.Scripts.Core;
public class BirdManager : MonoBehaviour
{
    Vector2 pos1;
    Vector2 pos2;
    float speed = 4f; 
    Vector2 pos;
    Vector2 direction;

    bool isMove = true;

    void Update()
    {
        if (isMove) {
            Vector2 nextPos = (Vector2)GetComponent<IsoTransform>().Position + Time.deltaTime * direction * speed;
            Vector2 diffV = (pos - nextPos) * direction;

            if (diffV.x < 0 || diffV.y < 0) {
                GetComponent<IsoTransform>().Position = (Vector3)pos + new Vector3(0, 0, 1);
                direction = -direction;
                if (pos == pos2) pos = pos1;
                else pos = pos2;
                StartCoroutine(wait());
                //Debug.Log(direction.normalized);
                
            }
            else
                GetComponent<IsoTransform>().Position = (Vector3)nextPos + new Vector3(0, 0, 1);
        }
    }
    IEnumerator wait () {
        isMove = false;
        yield return new WaitForSeconds(0.15f);
        changeSprite(direction);
        yield return new WaitForSeconds(0.15f);
        isMove = true;
    }

    public void create (Vector2 p1, Vector2 p2) {
        pos1 = p1;
        pos2 = p2;
        direction = (pos2 - pos1).normalized;
        pos = pos2;
        changeSprite(direction);
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
