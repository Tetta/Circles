using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System.Linq;
public class AnalyticsController : MonoBehaviour {

    void Awake() {
        if (FB.IsInitialized) {
            FB.ActivateApp();
        }
        else {
            //Handle FB.Init
            FB.Init(FB.ActivateApp);
        }
        //fix delete
        if (AdController.instance == null) {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("SESSIONS_COUNT", PlayerPrefs.GetInt("SESSIONS_COUNT", 0) + 1);
        }
        if (PlayerPrefs.GetInt("USER_GROUP", 0) == 0) {
            int r = UnityEngine.Random.Range(1, 9);
            PlayerPrefs.SetInt("USER_GROUP", r);
            sendEvent("UserGroup", new Dictionary<string, object>{{ "Group", r }});
        }

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
        if (FB.IsInitialized)
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