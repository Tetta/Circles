using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WheelUI : MonoBehaviour
{

    public Rigidbody2D wheelCircle;
    public Transform peaces;

    //25, 200, 25, shield, 25, 250, 25, char
    public List<int> rewards = new List<int> { 25, 200, 25, 0, 25, 250, 25, 0 };





    bool started = false;


    private void OnEnable() {
        wheelCircle.angularVelocity = 4000;
        wheelCircle.angularDrag = 2;

        started = true;
    }

    private void Update() {
        if (started && wheelCircle.angularVelocity < 0.01f) {
            wheelCircle.angularDrag = 5;
            started = false;
            Debug.Log("Stop wheel");
            Debug.Log(wheelCircle.transform.localRotation.eulerAngles);

            int id = Mathf.FloorToInt(wheelCircle.transform.localRotation.eulerAngles.z / 45);
            Debug.Log("id: " + id);
            Debug.Log("id: " + (int)wheelCircle.transform.localRotation.eulerAngles.z / 45);

            if (id == 3) Debug.Log("reward: shield");
            else if (id == 7) Debug.Log("reward: char");
            else Debug.Log("reward: " + rewards[id]);

            peaces.GetChild(id).GetComponent<Image>().DOColor(Color.blue, 0.1f).SetLoops(10, LoopType.Yoyo).OnComplete(() =>{
                //GameController.instance.restart();
            });

        }
    }


}
