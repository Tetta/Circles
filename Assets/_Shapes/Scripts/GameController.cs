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

    [Header("MainUI")]
    public Text levelText;
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

        //gift wheel
        //fix 4 * 60 * 60
        TimerManager.timers["gift"] = new Timer("gift", 20, updateGiftButton);
        //first launch
        if (PlayerPrefs.GetInt("USER_GROUP", 0) == 0) {
            PlayerPrefs.SetInt("USER_GROUP", UnityEngine.Random.Range(1, 10));
            //gift
            TimerManager.timers["gift"].init(true);
            updateGiftButton();
        }

        //char badge
        //CharButtonBadge.SetActive(GemsController.availableBuyChar());
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
        //fix - for test complete level
        //StartCoroutine(complete());

        //tutorial
        TutorialManager.step = -1;
        tutorial.SetActive(LevelController.level == 1);

        Player.instance.showChar();
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
        if (title == "GameUI" && !levelStarted) startLevel();
        foreach (var screen in screens) {
            //if (screen.Key == "Level") screen.Value.transform.position = new Vector3(0, 0, 10000);
            //else 
            
                //if (!(screen.Key == "Level" && title == "GameoverUI"))
                   // if (title != "WheelUI")
                screen.Value.SetActive(false);
        }
        //if (title == "Level") screens[title].transform.position = new Vector3(0, 0, 0);
        screens[title].SetActive(true);
        //if (screens["GameoverUI"].activeSelf) screens["Level"].SetActive(true);
        //if (screens["Level"].transform.position == new Vector3(0, 0, 0)) screens["GameUI"].SetActive(true);
        
        //char badge
        if (title == "MainUI") CharButtonBadge.SetActive(GemsController.availableBuyChar());
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
        };
        AdController.ShowRewarded();
    }

    public void showLives () {
        Debug.Log("showLives");
        //fix - depend from characer
        lives += 0;
        foreach (Transform child in livesTransform) {
            child.gameObject.SetActive(lives > child.GetSiblingIndex());
        }
    }
    public void minusLives() {
        lives--;
        livesTransform.GetChild(lives).GetComponent<Image>().color = new Color32(255, 255, 255, 40);

    }

    public static void enableObg(Transform t, int id) {
        foreach (Transform child in t) {
            if (child.GetSiblingIndex() != id) child.gameObject.SetActive(false);
            else if (id != -1) child.gameObject.SetActive(true);
        }
    }

    public static void logTime (string from) {
        Debug.Log("__________" + from + ": " + Time.realtimeSinceStartup);
    }

    public void changeSkin () {
        LevelController.skin++;
        if (LevelController.skin > 2) LevelController.skin = 0;
        restart();
    }
    public void changeLevel() {
        LevelController.level++;
        if (LevelController.level > 19) LevelController.level = 1;
        restart();
    }
    public void disableDecor() {
        Decor.enable = !Decor.enable;

        restart();
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
