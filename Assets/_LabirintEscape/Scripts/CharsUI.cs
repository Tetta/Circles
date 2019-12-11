using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using event data
using System;
public class CharsUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler {
    // Start is called before the first frame update
    public static CharsUI instance;

    bool isDraging = false;
    int velocityMin = 500;
    int skinWidth = 550;
    int skinSpacing = 0;
    public ScrollRect scroll;

    public GameObject skinPrefab;
    public Transform buttons;
    public Text titleText;
    public Text gemsText;
    public Text shieldText;
    public Text descText;
    public GameObject paramTexts0;
    public GameObject paramTexts1;
    //public GameObject skinCenter;
    //private Vector3 startPosTitle = new Vector3(37, 57);
    int charsCount = 7;
   // List<int> charPrices = new List<int> { 0, 100, 1000, 5000, -1 }; //-1 = is vip
    int centerId;
    List<Character> chars;
    public void Start()
    {


        instance = this;
        chars = GameController.instance.chars;
        scroll.content.GetChild(GameController.charId).localScale = new Vector3(1.55f, 1.55f, 1);
        //scroll.content.localPosition = new Vector3(GameController.charId * (-skinWidth - skinSpacing), 0, 0);
        scroll.content.localPosition = new Vector3(GameController.charId * (-skinWidth - skinSpacing) + (charsCount-4) * (skinWidth + skinSpacing), 0, 0) ;

        updateSkinPricesButtons();

        //skin tiles
        foreach (Transform child in scroll.content) {
            GameController.enableObg(child.GetChild(0), LevelController.skin);
        }
        
    }
    private void OnEnable() {
        if (instance != null) updateSkinPricesButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDraging || Mathf.Abs(scroll.velocity.x) > 0) {
            updateSkinSize();
        }

        if (!isDraging && Mathf.Abs(scroll.velocity.x) < velocityMin && scroll.velocity.x != 0) {
            updateSkinPricesButtons();
            updateSkinSize();

        }
    }

    public void updateSkinSize() {
        for (int i = 0; i < charsCount; i++) {
            float s = scroll.content.localPosition.x + scroll.content.GetChild(i).localPosition.x /*- 2* skinWidth */- transform.localPosition.x;
            if (Mathf.Abs(s) < skinWidth) {
                //scroll.content.GetChild(i).localScale = new Vector3(1, 1, 1) * ((skinWidth - Mathf.Abs(s)) / 3 + 100) / 100;
                scroll.content.GetChild(i).localScale = new Vector3(1, 1, 1) * ((skinWidth - Mathf.Abs(s)) / 10 + 100) / 100;
                //titles pos y
                //Debug.Log(scroll.content.GetChild(i).GetChild(1).localPosition);
                //Debug.Log(((((skinWidth - Mathf.Abs(s)) / 3 + 100) / 100) - 1) * 47);
                //scroll.content.GetChild(i).GetChild(1).localPosition = startPosTitle - new Vector3(0, ((((skinWidth - Mathf.Abs(s)) / 3 + 100) / 100) - 1) * 39);

            }
            else
                scroll.content.GetChild(i).localScale = new Vector3(1, 1, 1);
            //if (scroll.content.GetChild(i).localPosition == transform.localPosition + new Vector3(-skinSpacing, 0, 0)) {
           //}
        }
    }

    public void updateButtons(int id) {
        foreach (Transform child in buttons) child.gameObject.SetActive(false);
        if (id == GameController.charId) buttons.Find("Selected").gameObject.SetActive(true);
        else if (isAvailable(id)) buttons.Find("Select").gameObject.SetActive(true);
        else {
            if (GameController.instance.chars[id].vip)
                buttons.Find("VipButton").gameObject.SetActive(true);
            else {
                buttons.Find("Price").gameObject.SetActive(true);
                buttons.Find("Price/Price/Text").GetComponent<Text>().text = GameController.instance.chars[id].price.ToString();
            }


        }


    }

    public void updateSkinPricesButtons() {
        //centerId = - (skinWidth + skinSpacing) * charsCount / 2
        //centerId = (skinWidth + skinSpacing) * charsCount
        //    Mathf.RoundToInt(scroll.content.transform.localPosition.x / (skinWidth + skinSpacing)) + chars2;
        int centerPos = Mathf.RoundToInt(scroll.content.transform.localPosition.x / (skinWidth + skinSpacing));
        //Debug.Log("//");
        //Debug.Log(centerPos);

        centerId = Mathf.Abs(centerPos - 3);
        //centerId = Mathf.RoundToInt(scroll.content.transform.localPosition.x / (skinWidth + skinSpacing)) + chars2;
        //Debug.Log(centerId);
        if (centerId >= charsCount) centerId = charsCount - 1;
        scroll.StopMovement();

        scroll.content.transform.localPosition = new Vector3(centerPos * (skinWidth + skinSpacing), 0, 0);


        updateButtons(centerId);
        string txt = "";
        if (centerId == 0) {
            txt = "DEFAULT CHARACTER";
        } else {
            txt += "+" + (chars[centerId].addGems * 100) + "% GEMS";
            //if (centerId > 1) txt += "\n+" + chars[centerId].lives + " FREE SHIELD ON START LEVEL";
        }


        titleText.text = chars[centerId].title;
        descText.text = txt;
        //paramTexts0.SetActive(centerId == 0);
        //paramTexts1.SetActive(centerId != 0);
        //gemsText.text = "+" + (chars[centerId].addGems * 100);
        //shieldText.text = "+" + chars[centerId].lives;
}

    //Do this when the user stops dragging this UI Element.
    public void OnBeginDrag(PointerEventData data) {
        isDraging = true;
    }
    //Do this when the user stops dragging this UI Element.
    public void OnEndDrag(PointerEventData data) {
        isDraging = false;
    }
    public void onSelectSkinClick() {
        Debug.Log("onSelectSkinClick");
        GameController.charId = centerId;
        PlayerPrefs.SetInt("SELECTED_CHAR_ID", centerId);
        updateButtons(centerId);
        Player.instance.changeChar();
        AnalyticsController.sendEvent("CharSelect");

    }
    public void buyCharClick() {
        Debug.Log("buyCharClick");
        if (GemsController.SubtractGems(chars[centerId].price, "Char" + centerId)) {
            PlayerPrefs.SetInt("CHAR_" + centerId, 1);
            onSelectSkinClick();

            AnalyticsController.sendEvent("CharBuy");

        }

    }
    public static bool isAvailable (int id) {
        if (id == 0) return true;
        if (GameController.instance.chars[id].vip) return IAPManager.vip;
        

        return Convert.ToBoolean( PlayerPrefs.GetInt("CHAR_" + id, 0));

    }
}
