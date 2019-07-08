using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsController : Singleton<GemsController> {

    public static float gems
    {

        get { if (_gems == -1) return PlayerPrefs.GetFloat("GEMS", 0); else return _gems; }
        private set
        {
            PlayerPrefs.SetFloat("GEMS", value);
            if (UpdateGems != null) UpdateGems(value);
            _gems = value;
        }
    }
    static float _gems = - 1;
    
    public static Action<float> UpdateGems;
    public static float LastAdd { get; private set; }
    public static float gemsOnLevel;

    public static void AddGems(float amount, string src, bool multi = false) {
        if (amount < 0) return;
        //if (SubscribeController.LocalState) amount = amount * 2;
        float multiplicator = 1;
        if (multi) multiplicator += GameController.instance.chars[GameController.charId].addGems;
        multiplicator *= 1 + Convert.ToInt32(IAPManager.vip);
        
        float amountMulti = amount *multiplicator;
        
        LastAdd = amountMulti;
        gems += amountMulti;
        if (src == "Level") gemsOnLevel += amountMulti;
        //AnalyticsController.sendEvent("CoinsGet", new Dictionary<string, object> { { "Car", SkinsController.unlocked + 1 }, { "Level", GameController.instance.nextLevel }, { "Src", src }, { "Num", amount } });

    }

    public static bool SubtractGems(int amount, string src) {
        if (amount < 0) return false;
        if (gems < amount) return false;
        LastAdd = -amount;
        gems -= amount;
        //AnalyticsController.sendEvent("CoinsSpend", new Dictionary<string, object> { { "Car", SkinsController.unlocked + 1 }, { "Level", GameController.instance.nextLevel }, { "Src", src }, { "Num", amount } });

        return true;
    }

    public static bool availableBuyChar () {
        foreach (Character character in GameController.instance.chars) {
            if (!CharsUI.isAvailable(character.id) && !character.vip && gems >= character.price) return true;
        }
        return false;
    }
}
