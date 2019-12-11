using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public static int step = -1;


    public GameObject hand;

    // Start is called before the first frame update
    void OnEnable()
    {
        instance = this;

        step = 0;

        animHand();

        Debug.Log("OnEnable step: " + step);
        if(LevelController.skin == 3) hand.GetComponent<Image>().color = new Color32( 248, 0, 0, 255);
    }

    public void animHand () {
        hand.SetActive(true);
        if (step == 0) {
            hand.transform.DOKill();
            hand.transform.localPosition = new Vector3(-73, -723, 0);
            //hand.transform.DOKill();
            
            hand.transform.DOLocalMove( new Vector3(210f, -580f, 0f), 0.8f).SetEase(Ease.Linear).SetLoops(-1);
        }
        else if (step == 1) {
            hand.transform.DOKill();
            hand.transform.localPosition = new Vector3(210, -723, 0);
            hand.transform.DOLocalMove(new Vector3(-73f, -580f, 0f), 0.8f).SetEase(Ease.Linear).SetLoops(-1);
        }
        else if (step == 2) {
            hand.transform.DOKill();
            hand.transform.localPosition = new Vector3(210, -580f, 0);
            hand.transform.DOLocalMove(new Vector3(-73f, -723, 0f), 0.8f).SetEase(Ease.Linear).SetLoops(-1);
        }
        else if (step == 3) {
            hand.transform.DOKill();
            hand.transform.localPosition = new Vector3(-73f, -580f, 0);
            hand.transform.DOLocalMove(new Vector3(210f, -723, 0f), 0.8f).SetEase(Ease.Linear).SetLoops(-1);
        }

    }

    public void addStep() {
        step++;
        
        animHand();
        if (step == 4) hideHand();
    }

    public void hideHand() {
        step = -1;
        hand.transform.DOKill();
        hand.SetActive(false);
    }

}
