using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Linq;
using System;
public class AnalyticsController : MonoBehaviour {
    public static AnalyticsController instance;

    public static bool firstLaunch;
    public static bool awake;
    public static string subscriptionFrom = "";
    //point
    //for review AppStore vipUI off (on after "07/17/2019")
    string vip1AfterDate = "07/17/2019";

    void Awake() {
        if (FB.IsInitialized) {
            FB.ActivateApp();
        }
        else {
            //Handle FB.Init
            FB.Init(FB.ActivateApp);
        }
        
        if (instance == null)  {
            instance = this;
            
            PlayerPrefs.SetInt("SESSIONS_COUNT", PlayerPrefs.GetInt("SESSIONS_COUNT", 0) + 1);
            awake = true;
        }
        if (PlayerPrefs.GetInt("USER_GROUP", -1) == -1) {
            //fix start 0
            int r = UnityEngine.Random.Range(0, 10);
            PlayerPrefs.SetInt("USER_GROUP", r);
            sendEvent("UserGroup", new Dictionary<string, object>{{ "Group", r }});
            firstLaunch = true;
        }
        if (PlayerPrefs.GetInt("USER_GROUP_VIP", -1) == -1) {
            int r;
            if (DateTime.Now < DateTime.ParseExact(vip1AfterDate, "MM/dd/yyyy", null))
                r = 1;
            else
                r = UnityEngine.Random.Range(0, 2);
            PlayerPrefs.SetInt("USER_GROUP_VIP", r);
            sendEvent("UserGroupVip", new Dictionary<string, object> { { "GroupVip", r } });

        }
    }
    private void Start() {
        if (awake) {
            /* Mandatory - set your AppsFlyer’s Developer key. */
            AppsFlyer.setAppsFlyerKey("Ura5UVbFB3YXvaig2PnvPA");
            /* For detailed logging */
            /* AppsFlyer.setIsDebug (true); */
#if UNITY_IOS
           /* Mandatory - set your apple app ID
              NOTE: You should enter the number only and not the "ID" prefix */
           AppsFlyer.setAppID ("1468496375");
           AppsFlyer.trackAppLaunch ();
#elif UNITY_ANDROID
            /* Mandatory - set your Android package name */
            AppsFlyer.setAppID("com.gearsoffun.labirintescape");
            /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
            AppsFlyer.init("Ura5UVbFB3YXvaig2PnvPA", "AppsFlyerTrackerCallbacks");
#endif
        }
        awake = false;
    }



    public static void sendEvent(string eventName, Dictionary<string, object> params1 = null, Dictionary<string, object> params2 = null) {

        Dictionary<string, object> params3 = new Dictionary<string, object>();
        if (params1 != null) params3 = params1;
        if (params2 != null) params3 = params1.Concat(params2).ToDictionary(x => x.Key, x => x.Value);

        params3["Level"] = LevelController.level;
        params3["Char"] = GameController.charId;
        params3["Gems"] = (int)GemsController.gems;
        params3["Skin"] = LevelController.skin;

        //FB
        if (FB.IsInitialized) {
            FB.LogAppEvent(
                eventName,
                parameters: params3
            );
            //for Debug.Log
            string keys = eventName;
            foreach (KeyValuePair<string, object> param in params3) {
                keys += ":" + param.Key + "_" + param.Value;
            }
            Debug.Log("____________" + keys);

        } else {
            instance.StartCoroutine(sendEventCoroutine(eventName, params3));
        }

    }
    public static IEnumerator sendEventCoroutine(string eventName, Dictionary<string, object> params3 = null) {
        while (!FB.IsInitialized) {
            yield return null;
        }
        sendEvent(eventName, params3);
    }

    public static void LogPurchase(string contentData, string contentId, string contentType, string currency, float price) {
        Debug.Log("Send Event Purchase to Facebook\n" + contentId);
        var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.Description] = contentData;
        parameters[AppEventParameterName.ContentID] = contentId;
        parameters[AppEventParameterName.ContentType] = contentType;
        parameters[AppEventParameterName.Currency] = currency;
        FB.LogAppEvent(
            AppEventName.Purchased,
            price,
            parameters
        );
    }

    public static void LogAddedToCartEvent(string contentData, string contentId, string contentType, string currency, float price) {
        var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.Description] = contentData;
        parameters[AppEventParameterName.ContentID] = contentId;
        parameters[AppEventParameterName.ContentType] = contentType;
        parameters[AppEventParameterName.Currency] = currency;
        FB.LogAppEvent(
            AppEventName.AddedToCart,
            price,
            parameters
        );
    }
}