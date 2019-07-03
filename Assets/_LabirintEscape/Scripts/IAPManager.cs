using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Purchasing;
public class IAPManager : MonoBehaviour
{
    public static bool vip = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }



    public void buySubscription( Product product) {
        PlayerPrefs.SetInt("CHAR_5", 1);
        GameController. charId = 5;
        PlayerPrefs.SetInt("SELECTED_CHAR_ID", GameController.charId);
        Player.instance.changeChar();

        setVip(1);



        AnalyticsController.sendEvent("SubscriptionBought");
        AnalyticsController.LogAddedToCartEvent(product.definition.storeSpecificId, product.definition.storeSpecificId, "Subscribe", product.metadata.isoCurrencyCode, (float)product.metadata.localizedPrice);

    }

    public static  void setVip(int i) {
        Debug.Log("setVip: " + i);
        PlayerPrefs.SetInt("VIP", i);
        vip = Convert.ToBoolean(i);
        if (vip) GameController.instance.showPreviousScreen();
        //setVipFeatures();

        //if (vip == 0 && PlayerPrefs.GetString("selectedBarName") == "vip")
        //    BarGUI.instance.updateBarPricesButtons();
    }
    public void onPrivacyClick() {
        Application.OpenURL("https://docs.google.com/document/d/1FkOeftcs8gF9gZYcTyEXGgihmkdCb60x7Z1Q1o8ixbo/edit?usp=sharing");
    }
    public void onTermsClick() {
        Application.OpenURL("https://docs.google.com/document/d/16AMooYyxeEODGMWGF48IScPSj2w3dPtJ7qgowRByYb8/edit?usp=sharing");
    }
    //public void purchase
}
