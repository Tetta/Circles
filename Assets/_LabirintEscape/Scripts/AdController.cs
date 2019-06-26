using System;
using System.Collections;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;

[DisallowMultipleComponent]
public class AdController : MonoBehaviour, IInterstitialAdListener, IRewardedVideoAdListener {
    public static AdController instance = null;

    public readonly string IOS_APP_KEY = "8d22053f89e68de99c27e9adeaa38c0fa42aa0374a944de6";
    public readonly string ANDROID_APP_KEY = "772b5273436a4488f6ad99ae9612bc4aa9602258a9f8aa36";

    //public readonly float interstitialInterval = 30f;    
    //float timer;
    //bool timerTicked;

    public static Action giveReward;
    public event Action OnInterstitialWatched;

    public static bool IsVideoReady
    {
        get
        {
#if UNITY_EDITOR
            return true;
#endif

            return Appodeal.isLoaded(Appodeal.REWARDED_VIDEO);
        }
    }
    public static bool IsInterstitialReady { get { return Appodeal.isLoaded(Appodeal.INTERSTITIAL); } }

    //private AudioManager audioManager;

    #region Interstitial event handlers

    public void onInterstitialFailedToLoad() { }
    public void onInterstitialClicked() { }
    public void onInterstitialExpired() { }
    public void onInterstitialShown() { }
    public void onInterstitialLoaded(bool isPrecache) { }
    public void onInterstitialClosed() {
        StartCoroutine(InterstitialClosedCoroutine());
    }

    #endregion

    #region Rewarded event handlers

    public void onRewardedVideoExpired() { }
    public void onRewardedVideoFailedToLoad() { }
    public void onRewardedVideoFinished(double amount, string name) { }
    public void onRewardedVideoShown() { }
    public void onRewardedVideoLoaded(bool precache) { }
    public void onRewardedVideoClosed(bool finished) {
        StartCoroutine(RewardedVideoClosedCoroutine(finished));
    }

    #endregion

    public static void ShowInterstitial() {
        Debug.Log("ShowInterstitial");
        if (IsInterstitialReady && !IAPManager.vip) {
            Pause(true);
            Appodeal.show(Appodeal.INTERSTITIAL);
        }
    }

    public static void ShowRewarded() {
#if UNITY_EDITOR
        giveReward?.Invoke();
        //giveReward = null; 
#endif
        if (IsVideoReady) {
            Pause(true);
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }
    }

    private void Start() {
        if (instance == null) {
            instance = this;
        }
        else  {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        var appKey = "";
#if UNITY_IOS
        appKey = IOS_APP_KEY;
#elif UNITY_ANDROID
        appKey = ANDROID_APP_KEY;
#endif
        Appodeal.initialize(appKey, Appodeal.REWARDED_VIDEO | Appodeal.INTERSTITIAL);
        Appodeal.setLogLevel(Appodeal.LogLevel.Debug);
        Appodeal.setInterstitialCallbacks(this);
        Appodeal.setRewardedVideoCallbacks(this);

        //audioManager = AudioManager.instance;
    }

    private IEnumerator InterstitialClosedCoroutine() {
        yield return new WaitForSecondsRealtime(0.1f);
        Pause(false);
        OnInterstitialWatched?.Invoke();
    }

    private IEnumerator RewardedVideoClosedCoroutine(bool finished) {
        yield return new WaitForSecondsRealtime(0.1f);
        Pause(false);
        if (finished) {
            Debug.Log("OnRewardedVideoClosed and finished=true");
            giveReward?.Invoke();
        }
        else {
            Debug.Log("OnRewardedVideoClosed and finished=false");
        }
    }

    private static void Pause(bool isOn) {
        if (isOn) {
            //Time.timeScale = 0f;
            //audioManager.Mute();
        }
        else {
            //Time.timeScale = 1f;
            //audioManager.Unmute();
        }
    }
}
