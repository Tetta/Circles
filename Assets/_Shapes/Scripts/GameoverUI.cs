using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameoverUI : MonoBehaviour
{
    public Text gemsCountText;
    public GameObject tapText;
    public GameObject continueGO;
    public Image circle1;
    public Image circle2;

    bool shown;
    private void Awake() {
        shown = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() {
        gemsCountText.text = "+" + GemsController.gemsOnLevel;

        Debug.Log("shown: " + shown);
        if (!shown && AdController.IsVideoReady) {
            shown = true;
            continueGO.SetActive(true);
            tapText.SetActive(false);
            circle1.fillAmount = 1;
            circle2.fillAmount = 1;


            circle1.DOFillAmount(0, 5);
            circle2.DOFillAmount(0, 5).OnComplete(() => {
                continueGO.SetActive(false);
                tapText.SetActive(true);
            });
        }
        else {
            continueGO.SetActive(false);
            tapText.SetActive(true);
        }
        //StartCoroutine(enable());
    }
    /*
    IEnumerator enable () {
        for (float i = 0; i < 5; i +=0.1f) {
            yield return new WaitForSeconds(0.1f);
        }


    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }
    public void continueClick() {
        AdController.giveReward = () => {
            //TimerManager.timers["daily"].init(true);
            //collectx4Button.interactable = false;
            //CoinsController.instance.AddCoins(750, "DailyBonusForAd");
            Debug.Log("giveReward continue game");
            //StartCoroutine(Player.instance.revive());
            Player.instance.revive();

        };
        AdController.ShowRewarded();
    }

    public void noThanksClick() {
        GameController.instance.restart();
    }


}
