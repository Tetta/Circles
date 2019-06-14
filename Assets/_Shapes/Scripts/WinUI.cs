using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WinUI : MonoBehaviour
{
    public static WinUI instance;
    public Text levelText;
    public Text gemsCountText;
    public Text tapText;
    public Transform giftButtons;

    public GameObject hand;

    //public GameObject continueGO;
    //public Image circle1;
    //public Image circle2;

    //bool opened = false;
    //bool shown;
    private void Awake() {
        //shown = false;
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Win start");
        completeLevel();
    }

    private void OnEnable() {
        
    }

    public void completeLevel () {
        GameController.levelPaused = true;
        levelText.text = "LEVEL " + LevelController.level;
        gemsCountText.text = "+" + GemsController.gemsOnLevel;

        //bool freeGift = LevelController.level == 1 && PlayerPrefs.GetInt("FREE_GIFT_1", 0) == 1 || LevelController.level == 2 && PlayerPrefs.GetInt("FREE_GIFT_1", 0) == 2;
        bool freeGift = LevelController.level == 1 || LevelController.level == 2;
        int buttonId = 0;
        if (freeGift) buttonId = 1;
        GameController.enableObg(giftButtons, buttonId);

        tapText.color = new Color32(255, 255, 255, 0);
        tapText.DOColor(new Color32(255, 255, 255, 0), 2).OnComplete(() => {

            Debug.Log("tapText.DOColor OnComplete");
            if (!freeGift) tapText.DOColor(new Color32(255, 255, 255, 255), 1);
        });

        hand.SetActive(LevelController.level == 1);

        //fix save
        LevelController.addLevel();
    }

    public void giftAdClick() {
        AdController.giveReward = () => {

            Debug.Log("giveReward giftAdClick");
            addGift();

        };
        AdController.ShowRewarded();
    }
    public void addGift() {

        Debug.Log("addGift");

        GameController.enableObg(giftButtons, 2);
        //if (LevelController.level == 1 || LevelController.level == 2) PlayerPrefs.SetInt("FREE_GIFT_" + LevelController.level, 1);
        tapText.DOKill();
        tapText.color = new Color32(255, 255, 255, 255);

        GameController.instance.showScreen("WheelUI");
    }


    public void continueClick() {
        //after 3 and  20
        if (LevelController.level == 4 || LevelController.level == 21) iOSReviewRequest.Request();
        else if (LevelController.level >= 5) AdController.ShowInterstitial();
        GameController.instance.restart();
       
    }


}
