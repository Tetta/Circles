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
    public Button tapButton;

    [Header("Shield Offer")]
    public GameObject shieldOffer;
    public GameObject shieldOfferAdButton;
    public GameObject shieldOfferVipButton;


    bool shown;
    bool levelFailed;

    private void Awake() {
        shown = false;
        levelFailed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable() {
        GameController.levelPaused = true;
        gemsCountText.text = "+" + (int)GemsController.gemsOnLevel;
        shieldOffer.SetActive(false);
        tapButton.enabled = false;
        Debug.Log("shown: " + shown);
        if (!shown && AdController.IsVideoReady) {
            shown = true;
            continueGO.SetActive(true);
            tapText.SetActive(false);
            circle1.fillAmount = 1;
            circle2.fillAmount = 1;


            circle1.DOFillAmount(0, 5);
            circle2.DOFillAmount(0, 5).OnComplete(() => {
                levelFail();
            });
        }
        else {
            levelFail();
        }

    }

    public void levelFail() {
        levelFailed = true;
        circle2.DOKill();
        continueGO.SetActive(false);
        tapText.SetActive(true);
        tapButton.enabled = true;

        if (!levelFailed) {

            AnalyticsController.sendEvent("LevelFail", new Dictionary<string, object> { { "GemsPercent", Player.instance.gemsCollected * 100 / LevelController.levelData.coins.Count }, { "DotsPercent", Player.instance.dotsCollected * 100 / LevelController.levelData.dots.Count } });


        }
        //shield offer
        bool timePass = !TimerManager.timers["gameoverOffer"].enable;
        int group = PlayerPrefs.GetInt("USER_GROUP_GAMEOVER_OFFER", 1);
        Debug.Log("USER_GROUP_GAMEOVER_OFFER: " + group);
        shieldOffer.SetActive(((group == 1 && AdController.IsVideoReady) || (group == 2 && !IAPManager.vip)) && timePass);
        shieldOfferAdButton.SetActive(group == 1);
        shieldOfferVipButton.SetActive(group == 2 && !IAPManager.vip);
        GameController. isPrevGameOver = true;
        if(shieldOffer.activeSelf) TimerManager.timers["gameoverOffer"].init(true);
    }


    public void continueClick() {
        AdController.giveReward = () => {

            Debug.Log("giveReward continue game");

            Player.instance.revive();
            AnalyticsController.sendEvent("RewardedAd", new Dictionary<string, object> { { "For", "Revive" } });

        };
        AdController.ShowRewarded();
    }

    public void tapToRestartClick() {
        //AdController.ShowInterstitial();
        GameController.instance.restart();
    }

    public void shieldAdClick() {
        AdController.giveReward = () => {

            StartCoroutine(AdController.instance.shieldAdCoroutine());
            
        };
        AdController.ShowRewarded();
    }



}
