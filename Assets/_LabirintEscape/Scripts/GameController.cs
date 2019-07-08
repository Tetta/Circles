using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameController : MonoBehaviour {
    public static GameController instance;
    public static int charId;
    public static bool levelPaused;



    public List<GameObject> screensList;
    public Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();
    bool levelStarted;
    string currentScreen;
    string previousScreen;

    [Header("MainUI")]
    public Text levelText;
    public Text newLevelsText;
    public Button giftButton;
    public Text giftTimerText;
    public GameObject CharButtonBadge;

    [Header("UI Skins")]
    //UI Skins
    public Transform skinsBg;
    public Transform tileMainMenu;
    public SpriteRenderer playerMainMenu;

    [Header ("GameUI")]
    //GameUI
    public int lives;
    public Transform livesTransform;
    public GameObject tutorial;
    public List<Character> chars;

    [Header("Buttons")]
    public Color32[] colorsBg;
    public Color32[] colorsBgVip;

    // Start is called before the first frame update
    private void Awake() {
        Debug.Log("GameController Awake");
        instance = this;
        awakeScene();
    }

    void Start()
    {
       
    }
    private void Update() {
        //GameController.logTime("update");
        if (TimerManager.timers["gift"].enable)
            giftTimerText.text = TimerManager.timers["gift"].timer.Hours.ToString("00") + ":" + TimerManager.timers["gift"].timer.Minutes.ToString("00") + ":" + TimerManager.timers["gift"].timer.Seconds.ToString("00");

    }


    void awakeScene() {
        logTime("Awake");
        //fix uncomment
        //LevelController.level = PlayerPrefs.GetInt("LEVEL", 1);
        LevelController.level = 26;

        setSkin();
        foreach (var p in screensList) {
            //Debug.Log(p.name);
            screens.Add(p.name, p);
        }
        //showScreen("Level");
        showScreen("MainUI");

        GemsController.gemsOnLevel = 0;
        levelStarted = false;
        charId = PlayerPrefs.GetInt("SELECTED_CHAR_ID", 0);
        levelPaused = true;
        //SkinBg
        skinsBg.gameObject.SetActive(true);
        enableObg(skinsBg, LevelController.skin);
        enableObg(tileMainMenu, LevelController.skin);


        levelText.text = "LEVEL " + LevelController.level;
        newLevelsText.gameObject.SetActive(LevelController.level >= 30);
        //gift wheel
        //point 4 * 60 * 60
        TimerManager.timers["gift"] = new Timer("gift", 4 * 60 * 60, updateGiftButton);

        //first launch
        if (AnalyticsController.awake && PlayerPrefs.GetInt("SESSIONS_COUNT", 0) == 1) {
            //PlayerPrefs.SetInt("USER_GROUP", UnityEngine.Random.Range(1, 10));
            //gift
            TimerManager.timers["gift"].init(true);
            updateGiftButton();
        }

        //on 2 session
        if (AnalyticsController.awake && !IAPManager.vip && PlayerPrefs.GetInt("SESSIONS_COUNT", 0) >= 2) {
            //fix uncomment
            //showScreen("VipUI");
            logSubscriptionShown("Start");
        }
        //char badge ?
        //CharButtonBadge.SetActive(GemsController.availableBuyChar());
    }

    void setSkin() {
        //Debug.Log("setSkin");

        int group = PlayerPrefs.GetInt("USER_GROUP", 0);
        //Debug.Log("group: " + group);
        int category = ((int) (LevelController.level / 10)) * 9;
        int location = 0;
        if (LevelController.level - category >= 1 && LevelController.level - category <= 3)
            location = 0;
        else if (LevelController.level - category >= 4 && LevelController.level - category <= 6)
            location = 1;
        else if (LevelController.level - category >= 7 && LevelController.level - category <= 9)
            location = 2;
        //Debug.Log("location: " + location);
        //Debug.Log("category: " + category);
        //Debug.Log("level: " + LevelController.level);

        switch (group) {
            case 1:
            case 2:
            case 3:
                LevelController.skin = group - 1;
                break;
            case 4:
                LevelController.skin = location;
                break;
            case 5:
                LevelController.skin = location;
                if (location == 1) LevelController.skin = 2;
                else if (location == 2) LevelController.skin = 1;
                break;
            case 6:
                if (location == 0) LevelController.skin = 1;
                else if(location == 1) LevelController.skin = 0;
                else if (location == 2) LevelController.skin = 2;
                break;
            case 7:
                if (location == 0) LevelController.skin = 1;
                else if (location == 1) LevelController.skin = 2;
                else if (location == 2) LevelController.skin = 0;
                break;
            case 8:
                if (location == 0) LevelController.skin = 2;
                else if (location == 1) LevelController.skin = 0;
                else if (location == 2) LevelController.skin = 1;
                break;
            case 9:
                if (location == 0) LevelController.skin = 2;
                else if (location == 1) LevelController.skin = 1;
                else if (location == 2) LevelController.skin = 0;
                break;
        }
        //Debug.Log("LevelController.skin: " + LevelController.skin);


    }

    public void updateGiftButton () {
        bool giftEnable = !TimerManager.timers["gift"].enable;
        giftButton.interactable = giftEnable;
        giftTimerText.text = "GET GIFT!";
        giftButton.GetComponent<Animator>().enabled = giftEnable;
    }

    void startLevel () {
        Debug.Log("startLevel");
        levelStarted = true;
        showLives();
        levelPaused = false;
        skinsBg.gameObject.SetActive(false);


        //tutorial
        TutorialManager.step = -1;
        tutorial.SetActive(LevelController.level == 1);

        Player.instance.showChar();
        AnalyticsController.sendEvent("LevelStart");

        TeleportAnother.enter = false;
    }

    //public IEnumerator complete () {
    //    yield return new WaitForSeconds(3);
    //    showScreen("WinUI");

    //}
    public void complete() {
        showScreen("WinUI");

    }

    public void showScreen (string title) {
        Debug.Log("showScreen: " + title);
        previousScreen = currentScreen;
        currentScreen = title;
        if (title == "GameUI" && !levelStarted) startLevel();
        foreach (var screen in screens) {
                screen.Value.SetActive(false);
        }

        screens[title].SetActive(true);
        //char badge
        if (title == "MainUI") CharButtonBadge.SetActive(GemsController.availableBuyChar());
    }

    public void showPreviousScreen() {
        Debug.Log("showPreviousScreen: " + previousScreen);
        showScreen(previousScreen);
    }

    public void onBackLevel() {
        if (LevelController.level >= 5) AdController.ShowInterstitial();
        restart();
    }

    public void restart () {
        GameController.logTime("restart click");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void plusLiveClick() {
        AdController.giveReward = () => {
            Debug.Log("giveReward plusLive");
            lives ++;
            showScreen("GameUI");

            AnalyticsController.sendEvent("ShieldAdd", new Dictionary<string, object> { { "For", "Ad" } });
            AnalyticsController.sendEvent("RewardedAd", new Dictionary<string, object> { { "For", "Shield" } });

        };
        AdController.ShowRewarded();
    }

    public void showLives () {
        Debug.Log("showLives");
        lives += chars[charId].lives +  PlayerPrefs.GetInt("SHIELD_WHEEL", 0) + Convert.ToInt32(IAPManager.vip);
        PlayerPrefs.SetInt("SHIELD_WHEEL", 0);
        foreach (Transform child in livesTransform) {
            child.gameObject.SetActive(lives > child.GetSiblingIndex());
        }
    }
    public void minusLives() {
        lives--;
        livesTransform.GetChild(lives).GetComponent<Image>().color = new Color32(255, 255, 255, 40);
        AnalyticsController.sendEvent("ShieldRemove");

    }

    public static void enableObg(Transform t, int id) {
        foreach (Transform child in t) {
            if (child.GetSiblingIndex() != id) child.gameObject.SetActive(false);
            else if (id != -1) child.gameObject.SetActive(true);
        }
    }

    public static void logTime (string from) {
        //Debug.Log("__________" + from + ": " + Time.realtimeSinceStartup);
    }

    public void changeSkin () {
        int group = PlayerPrefs.GetInt("USER_GROUP", 0);

        group++;
        if (group > 9) group = 1;
        PlayerPrefs.SetInt("USER_GROUP", group);
        restart();
    }
    public void changeLevel() {
        //LevelController.level++;

        if (LevelController.level >= 30) LevelController.level = 0;
        LevelController.addLevel();
        restart();
    }
    public void disableDecor() {
        Decor.enable = !Decor.enable;

        restart();
    }

    public void changeSound () {
        AudioManager.instance.audioOnOff();
    }

public void onPrivacyClick() {
    Application.OpenURL("https://docs.google.com/document/d/1FkOeftcs8gF9gZYcTyEXGgihmkdCb60x7Z1Q1o8ixbo/edit?usp=sharing");
}
public void onTermsClick() {
    Application.OpenURL("https://docs.google.com/document/d/16AMooYyxeEODGMWGF48IScPSj2w3dPtJ7qgowRByYb8/edit?usp=sharing");
}

    public void logSubscriptionShown(string from) {
        AnalyticsController.subscriptionFrom = from;
        AnalyticsController.sendEvent("SubscriptionShown", new Dictionary<string, object> { { "from", AnalyticsController.subscriptionFrom } });

    }
}

[Serializable]
public class Character {
    public int id;
    public string title;
    public List<Sprite> sprites;
    public int price;
    public bool vip;
    public int lives;
    public float addGems;



}
