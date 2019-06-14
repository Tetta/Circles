using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WheelUI : MonoBehaviour
{
    public static WheelUI instance;

    //25, 200, 25, shield, 25, 250, 25, char
    public List<int> rewards = new List<int> { 25, 200, 25, 0, 25, 250, 25, 0 };



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
    }

    public void onStopWheel () {

        tapText.DOColor(new Color32(255, 255, 255, 255), 1);
        bgButton.enabled = true;
        ps.SetActive(true);
        //fix set reward
        //fix timer on mainUI
        if (GameController.instance.skinsBg.gameObject.activeSelf) TimerManager.timers["gift"].init(true);
        GameController.instance.updateGiftButton();
    }

    public void hideWheel () {
        if (GameController.instance.skinsBg.gameObject.activeSelf) GameController.instance.showScreen("MainUI");
        else
            GameController.instance.showScreen("WinUI");
        //GameController.instance.restart();
        //WinUI.instance.continueClick();
    }

}
