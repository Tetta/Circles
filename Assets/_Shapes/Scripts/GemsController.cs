using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsController : Singleton<GemsController> {

    public static int gems
    {

        get { if (_gems == -1) return PlayerPrefs.GetInt("GEMS", 0); else return _gems; }
        private set
        {
            PlayerPrefs.SetInt("GEMS", value);
            if (UpdateGems != null) UpdateGems(value);
            _gems = value;
        }
    }
    static int _gems = - 1;
    
    public static Action<int> UpdateGems;
    public static int LastAdd { get; private set; }
    public static int gemsOnLevel;

    public static void AddGems(int amount, string src) {
        if (amount < 0) return;
        //if (SubscribeController.LocalState) amount = amount * 2;
        LastAdd = amount;
        gems += amount;
        gemsOnLevel += amount;
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
