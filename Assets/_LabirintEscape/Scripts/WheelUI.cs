using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WheelUI : MonoBehaviour
{
    public static WheelUI instance;

    //25, 200, 25, shield, 25, 250, 25, char
    //public List<int> rewards = new List<int> { 500, 2000, 500, 0, 500, 2500, 500, 0 };

    
    public Text tapText;
    public Button bgButton;
    public GameObject ps;

    private void Awake() {
        instance = this;
    }

    private void OnEnable() {

        tapText.DOKill();
        tapText.color = new Color32(255, 255, 255, 0);
        bgButton.enabled = false;
        ps.SetActive(false);
        bgButton.transform.GetChild(0).gameObject.SetActive(!GameController.instance.skinsBg.gameObject.activeSelf);
        AnalyticsController.sendEvent("WheelSpin");

    }

    public void onStopWheel (int rewardId) {

        tapText.DOColor(new Color32(255, 255, 255, 255), 1);
        bgButton.enabled = true;
        ps.SetActive(true);
        StartCoroutine(playCompleteSounds());
        //point set reward
        switch (rewardId) {
            case 0:
                GemsController.AddGems(500, "Wheel");
                break;
            case 1:
                GemsController.AddGems(2000, "Wheel");
                break;
            case 2:
                GemsController.AddGems(500, "Wheel");
                break;
            case 3:
                PlayerPrefs.SetInt("SHIELD_WHEEL", PlayerPrefs.GetInt("SHIELD_WHEEL", 0) + 1);
                AnalyticsController.sendEvent("ShieldAdd", new Dictionary<string, object> { { "For", "Wheel" } });
                break;
            case 4:
                GemsController.AddGems(500, "Wheel");
                break;
            case 5:
                GemsController.AddGems(2500, "Wheel");
                break;
            case 6:
                GemsController.AddGems(500, "Wheel");
                break;
            case 7:
                

                PlayerPrefs.SetInt("CHAR_4", 1);
                GameController.charId = 4;
                PlayerPrefs.SetInt("SELECTED_CHAR_ID", GameController.charId);
                Player.instance.changeChar();
                break;

        }


        if (GameController.instance.skinsBg.gameObject.activeSelf) TimerManager.timers["gift"].init(true);
        GameController.instance.updateGiftButton();
    }

    IEnumerator playCompleteSounds () {
        AudioManager.instance.wheelRewardSound.pitch = 0.8f;
        AudioManager.instance.wheelRewardSound.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        AudioManager.instance.wheelRewardSound.pitch = 1f;
        AudioManager.instance.wheelRewardSound.Play();
        yield return new WaitForSecondsRealtime(0.3f);
        AudioManager.instance.wheelRewardSound.pitch = 1.2f;
        AudioManager.instance.wheelRewardSound.Play();
        
    }

    public void hideWheel () {
        if (GameController.instance.skinsBg.gameObject.activeSelf) GameController.instance.showScreen("MainUI");
        else
            GameController.instance.showScreen("WinUI");
        //GameController.instance.restart();
        //WinUI.instance.continueClick();
    }

}
