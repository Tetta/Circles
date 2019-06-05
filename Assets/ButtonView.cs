using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ButtonView : MonoBehaviour
{
    public Image bg;
    public Transform vipLine;


    // Start is called before the first frame update
    void OnEnable()
    {
        updateView();
    }
    void updateView () {
        if (name == "VipButton") {
            bg.color = GameController.instance. colorsBgVip[LevelController.skin];
            StartCoroutine(vipLineCoroutine());
        } else {
            bg.color = GameController.instance.colorsBg[LevelController.skin];

        }
    }

    IEnumerator vipLineCoroutine() {
        vipLine.transform.localPosition = new Vector3(-300, 0, 0);
        vipLine.DOLocalMoveX(370, 0.5f);
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1f, 3f));
        StartCoroutine(vipLineCoroutine());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
