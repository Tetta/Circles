using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameController : MonoBehaviour {
    public static GameController instance;
    public static int charId;
    public static bool levelPaused;
    public static int maxLevels = 30;
    public static bool isPrevGameOver;
    public static bool vibro;
    //for Lion Studios
    public static bool lion = true;

    public List<GameObject> screensList;
    public Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();
    bool levelStarted;
    string currentScreen;
    string previousScreen;


    [Header("MainUI")]

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
    public Text levelText;
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

    }

    void Start()
    {
        awakeScene();
    }
    private void Update() {
        //GameController.logTime("update");
        if (TimerManager.timers["gift"].enable)
            giftTimerText.text = TimerManager.timers["gift"].timer.Hours.ToString("00") + ":" + TimerManager.timers["gift"].timer.Minutes.ToString("00") + ":" + TimerManager.timers["gift"].timer.Seconds.ToString("00");
        //Debug.Log(TimerManager.timers["gameoverOffer"].enable);
    }


    void awakeScene() {
        logTime("Awake");
        //fix uncomment
        LevelController.level = PlayerPrefs.GetInt("LEVEL", 1);
        //LevelController.level = 8;

        setSkin();
        foreach (var p in screensList) {
            //Debug.Log(p.name);
            screens.Add(p.name, p);
        }

        //showScreen("MainUI");


        GemsController.gemsOnLevel = 0;
        levelStarted = false;
        charId = PlayerPrefs.GetInt("SELECTED_CHAR_ID", 0);
        levelPaused = true;
        //SkinBg
        skinsBg.gameObject.SetActive(true);
        enableObg(skinsBg, LevelController.skin);
        enableObg(tileMainMenu, LevelController.skin);


        levelText.text = "LEVEL " + LevelController.level;
        newLevelsText.gameObject.SetActive(LevelController.level >= maxLevels);
        //gift wheel
        //point 4 * 60 * 60
        TimerManager.timers["gift"] = new Timer("gift", 4 * 60 * 60, updateGiftButton);
        //point 45
        if (!TimerManager.timers.ContainsKey("gameoverOffer")) TimerManager.timers["gameoverOffer"] = new Timer("gameoverOffer", 45, null);

        //first launch
        if (AnalyticsController.awake && PlayerPrefs.GetInt("SESSIONS_COUNT", 0) == 1) {
            //PlayerPrefs.SetInt("USER_GROUP", UnityEngine.Random.Range(1, 10));
            //gift
            TimerManager.timers["gift"].init(true);
            
        }
        updateGiftButton();
        Debug.Log("----" + !TimerManager.timers["gameoverOffer"].enable);
        //Debug.Log(isPrevGameOver);
        //on 2 session
        if ((AnalyticsController.awake && PlayerPrefs.GetInt("SESSIONS_COUNT", 0) >= 2 || 
            PlayerPrefs.GetInt("USER_GROUP_GAMEOVER_OFFER", 1) == 3 && isPrevGameOver && !TimerManager.timers["gameoverOffer"].enable) 
            && !IAPManager.vip) {
            //fix uncomment
            if (!GameController.lion) showScreen("VipUI");
            logSubscriptionShown("Start");
            TimerManager.timers["gameoverOffer"].init(true);
        }
        //char badge ?
        //CharButtonBadge.SetActive(GemsController.availableBuyChar());
        isPrevGameOver = false;
        vibro = Convert.ToBoolean(PlayerPrefs.GetInt("VIBRO", 1));

        //color - show GameUI
        showScreen("GameUI");
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
        //0;    1;  2;  0,1,2;  0,2,1;  1,0,2;  1,2,0;  2,0,1;  2,1,0
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
            case 10:
                LevelController.skin = 3;
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
        Debug.Log("ffffffff" + LevelController.level);
        tutorial.SetActive(LevelController.level == 1);

        Player.instance.showChar();
        AnalyticsController.sendEvent("LevelStart");

        TeleportAnother.enter = false;
        Tile.tilesUncolored.Clear();
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
        if (title == "VipUI" /*&& PlayerPrefs.GetInt("USER_GROUP_VIP", -1) == 1*/) title = "Vip2UI";
        //if (title == "VipUI" && PlayerPrefs.GetInt("USER_GROUP_VIP", -1) == 2) title = "Vip3UI";

        previousScreen = currentScreen;
        currentScreen = title;
        if (title == "GameUI" && !levelStarted) startLevel();
        foreach (var screen in screens) {
                screen.Value.SetActive(false);
        }

        screens[title].SetActive(true);
        //char badge
        //if (title == "MainUI") 

            CharButtonBadge.SetActive(GemsController.availableBuyChar());
    }

    public void showPreviousScreen() {
        Debug.Log("showPreviousScreen: " + previousScreen);
        if (previousScreen != "" && previousScreen != null) showScreen(previousScreen);
    }

    public void onBackLevel() {
        AdController.ShowInterstitial();
         restart();
    }

    public void restart () {
        GameController.logTime("restart click");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public IEnumerator shieldAdCoroutine() {
        GameController.logTime("restartCoroutine click");
        Debug.Log("restartCoroutine1");

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        Debug.Log("restartCoroutine2");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
        Debug.Log("restartCoroutine3");
        GameController.instance.lives++;
        GameController.instance.showScreen("GameUI");

        AnalyticsController.sendEvent("RewardedAd", new Dictionary<string, object> { { "For", "ShieldGameover" } });
        AnalyticsController.sendEvent("ShieldAdd", new Dictionary<string, object> { { "For", "AdGameover" } });

    }
    public void plusLiveClick() {
        AdController.giveReward = () => {
            Debug.Log("giveReward plusLive");
            lives ++;
            showScreen("GameUI");

            AnalyticsController.sendEvent("RewardedAd", new Dictionary<string, object> { { "For", "Shield" } });
            AnalyticsController.sendEvent("ShieldAdd", new Dictionary<string, object> { { "For", "Ad" } });

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
        if (group > 10) group = 1;
        PlayerPrefs.SetInt("USER_GROUP", group);
        restart();
    }
    public void changeLevel() {
        //LevelController.level++;

        if (LevelController.level >= maxLevels) LevelController.level = 0;
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
    public void deletePrefs() {
        PlayerPrefs.DeleteAll();
        restart();
    }
    public void changeVibro() {
        vibro = !vibro;
        PlayerPrefs.SetInt("VIBRO", Convert.ToInt32(vibro));
        GameObject.Find("CanvasUI/GameUI/VibroButton/").transform.GetChild(0).gameObject.SetActive(vibro);
        GameObject.Find("CanvasUI/GameUI/VibroButton/").transform.GetChild(1).gameObject.SetActive(!vibro);
        //AudioListener.volume = PlayerPrefs.GetInt("AUDIO", 1);
    }
    public void settings() {
        GameObject.Find("CanvasUI/GameUI/AudioButton/").SetActive(!GameObject.Find("CanvasUI/GameUI/AudioButton/").activeSelf);
        GameObject.Find("CanvasUI/GameUI/VibroButton/").SetActive(!GameObject.Find("CanvasUI/GameUI/VibroButton/").activeSelf);

        GameObject.Find("CanvasUI/GameUI/AudioButton/").transform.GetChild(0).gameObject.SetActive(AudioManager. audioFlag);
        GameObject.Find("CanvasUI/GameUI/AudioButton/").transform.GetChild(1).gameObject.SetActive(!AudioManager.audioFlag);

        GameObject.Find("CanvasUI/GameUI/VibroButton/").transform.GetChild(0).gameObject.SetActive(vibro);
        GameObject.Find("CanvasUI/GameUI/VibroButton/").transform.GetChild(1).gameObject.SetActive(!vibro);

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
