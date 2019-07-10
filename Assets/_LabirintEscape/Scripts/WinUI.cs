using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WinUI : MonoBehaviour {
    public static WinUI instance;
    public Text levelText;
    public GameObject ps;
    public Text gemsCountText;
    public Text tapText;
    public Transform giftButtons;

    public GameObject hand;
    public Button tapButton;

    public Slider slider;
    public GameObject gemsPS;

    public Animator button1Animator;
    public Animator button2Animator;

    int level;
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
    void Start() {
        Debug.Log("Win start");
        StartCoroutine(completeLevel());
        tapButton.interactable = false;
    }

    private void OnEnable() {
        endingParams();
    }

    public IEnumerator completeLevel() {
        ps.SetActive(true);
        level = LevelController.level;
        AudioManager.instance.levelCompleteSound.Play();
        AnalyticsController.sendEvent("LevelComplete", new Dictionary<string, object> { { "GemsPercent", Player.instance.gemsCollected * 100 / LevelController.levelData.coins.Count }, { "DotsPercent", Player.instance.dotsCollected * 100 / LevelController.levelData.dots.Count } });
        
        float gemsCollectedCount = Player.instance.gemsCollected + Player.instance.dotsCollected;
        Debug.Log(level);
        Debug.Log(LevelController.allGems);
        Debug.Log(gemsCollectedCount);
        //gemsCollectedCount always >= LevelController.allGems. Why? Physics 2d?
        if (level >= 2 && gemsCollectedCount >= LevelController.allGems) GemsController.AddGems((int)GemsController.gemsOnLevel * 2, "AllGems");

        
        //default
        gemsPS.SetActive(false);
        tapText.color = new Color32(255, 255, 255, 0);
        //giftButtons.gameObject.SetActive(false);
        //button2Animator.gameObject.SetActive(false);
        button1Animator.enabled = false;
        button2Animator.enabled = false;
        hand.SetActive(level == 1);
        if (level == 1) slider.transform.parent.gameObject.SetActive(false);
        //defaultParams();

        GameController.levelPaused = true;
        levelText.text = "LEVEL " + level;
        //gemsCountText.text = "+" + (int)GemsController.gemsOnLevel;

        //bool freeGift = LevelController.level == 1 && PlayerPrefs.GetInt("FREE_GIFT_1", 0) == 1 || LevelController.level == 2 && PlayerPrefs.GetInt("FREE_GIFT_1", 0) == 2;
        bool freeGift = level == 1 || level == 2;
        int buttonId = 0;
        if (freeGift) buttonId = 1;
        GameController.enableObg(giftButtons, buttonId);

        //all gems

        slider.maxValue = LevelController.allGems;
        slider.value = 0;

        yield return StartCoroutine(gemsCount(0, GemsController.gemsOnLevel, 0.4f));

        if (level >= 2) {
            slider.DOValue(gemsCollectedCount, 0.4f).OnComplete(() => {

                Debug.Log("slider OnComplete");
                if (slider.maxValue == slider.value) {
                    gemsPS.SetActive(true);
                    StartCoroutine(gemsCount(GemsController.gemsOnLevel, (int)GemsController.gemsOnLevel * 3, 0.4f));

                }

            });
            if (slider.maxValue == slider.value) yield return new WaitForSecondsRealtime(1.5f);
            else yield return new WaitForSecondsRealtime(1f);
        }



        button1Animator.enabled = true;
        button2Animator.enabled = true;
        //giftButtons.gameObject.SetActive(true);
        //button2Animator.gameObject.SetActive(true);

        tapText.DOColor(new Color32(255, 255, 255, 0), 2).OnComplete(() => {

            Debug.Log("tapText.DOColor OnComplete");
            if (!freeGift) {
                tapText.DOColor(new Color32(255, 255, 255, 255), 1);
                tapButton.interactable = true;
                gemsPS.SetActive(false);
            }
        });



        //StartCoroutine(playCompleteSounds());
        //AudioManager.instance.levelCompleteSound.pitch = 1f;


        Debug.Log("completeLevel UI end");

    }

    void endingParams () {
        gemsPS.SetActive(false);
        gemsCountText.text = "+" + (int)GemsController.gemsOnLevel;

        float gemsCollectedCount = Player.instance.gemsCollected + Player.instance.dotsCollected;

        //slider.maxValue = LevelController.allGems;
        slider.value = gemsCollectedCount;

        button1Animator.enabled = true;
        button2Animator.enabled = true;
        if (slider.maxValue == slider.value && level >= 2) gemsCountText.text = "+" + (int)GemsController.gemsOnLevel * 3;

    
        //bool freeGift = level == 1 || level == 2;

        //if (!freeGift) {
        tapText.color = new Color32(255, 255, 255, 255);
            tapButton.interactable = true;

        //}
        ps.SetActive(false);

    }

    IEnumerator gemsCount (float start, float end, float time) {
        gemsCountText.text = "+" + (int)start;
        float portion = (end - start) / (time * 1);
        for (float i = 0; i <= time; i += 0.01f) {
            yield return new WaitForSeconds(0.01f);
            gemsCountText.text = "+" + ((int)start + (int) (portion * i));
        }
        gemsCountText.text = "+" + (int)end;
    } 
    /*
    IEnumerator playCompleteSounds() {
        AudioManager.instance.levelCompleteSound.pitch = 0.8f;
        AudioManager.instance.levelCompleteSound.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        AudioManager.instance.levelCompleteSound.pitch = 1f;
        AudioManager.instance.levelCompleteSound.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        AudioManager.instance.levelCompleteSound.pitch = 1.2f;
        AudioManager.instance.levelCompleteSound.Play();

    }
    */
    public void giftAdClick() {
        AdController.giveReward = () => {

            Debug.Log("giveReward giftAdClick");
            addGift();
            AnalyticsController.sendEvent("RewardedAd", new Dictionary<string, object> { { "For", "Gift" } });

        };
        AdController.ShowRewarded();
    }
    public void addGift() {

        Debug.Log("addGift");

        GameController.enableObg(giftButtons, 2);
        //if (LevelController.level == 1 || LevelController.level == 2) PlayerPrefs.SetInt("FREE_GIFT_" + LevelController.level, 1);
        tapText.DOKill();
        tapText.color = new Color32(255, 255, 255, 255);
        tapButton.interactable = true;
        GameController.instance.showScreen("WheelUI");
    }


    public void continueClick() {
        //after 3 and  20
        if (LevelController.level == 3 || LevelController.level == 20) iOSReviewRequest.Request();
        //after 5
        else if (LevelController.level >= 5) AdController.ShowInterstitial();
        LevelController.addLevel();
        GameController.instance.restart();

    }


}
