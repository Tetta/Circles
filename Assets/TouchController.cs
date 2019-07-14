using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    bool mouseDrag;
    Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.instance.transform.position + new Vector3(0, 0, -100);
        
    }

    void OnMouseDown() {
        if (Player.state == Player.State.Stay && !GameController.levelPaused) {
            //Debug.Log(Input.mousePosition);
            mousePos = Input.mousePosition;

            mouseDrag = true;

        }
    }
    void OnMouseUp() {
        if (Player.state == Player.State.Stay && !GameController.levelPaused) {
            //mousePos = Input.mousePosition;
            Vector3 v = mousePos - Input.mousePosition;
            if (v.x > 0 && v.y < 0 && (TutorialManager.step == - 1 || TutorialManager.step == 1)) Player.directionName = Player.Direction.Up;
            else if (v.x < 0 && v.y > 0 && (TutorialManager.step == -1 || TutorialManager.step == 3)) Player.directionName = Player.Direction.Down;
            else if (v.x > 0 && v.y > 0 && (TutorialManager.step == -1 || TutorialManager.step == 2)) Player.directionName = Player.Direction.Left;
            else if (v.x < 0 && v.y < 0 && (TutorialManager.step == -1 || TutorialManager.step == 0)) Player.directionName = Player.Direction.Right;
            mouseDrag = false;
            //Debug.Log("OnMouseUp");
            //Debug.Log(TutorialManager.step);
            //Debug.Log(Player.directionName);

            if (Player.directionName != Player.Direction.None && TutorialManager.instance != null && TutorialManager.step != -1) {
                AnalyticsController.sendEvent("Move" + TutorialManager.step);
                TutorialManager.instance.addStep();
            }
        }


    }
}
