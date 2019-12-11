using System;
using System.Collections;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;
using System.Collections.Generic;
[DisallowMultipleComponent]
public class AdController : MonoBehaviour, IInterstitialAdListener, IRewardedVideoAdListener, IBannerAdListener
{
    public static AdController instance = null;

    public readonly string IOS_APP_KEY = "8d22053f89e68de99c27e9adeaa38c0fa42aa0374a944de6";
    public readonly string ANDROID_APP_KEY = "51960c416717ce5e3d99a3404aabbf5b7a1beb8bdd42ddcd";



    public static Action giveReward;
    public event Action OnInterstitialWatched;
    public GameObject bannerDebug;
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
    private float timer;
    public static bool timerTicked = true;
    public readonly float interstitialInterval = 40f;
    //private AudioManager audioManager;
    public void showBanner()
    {
        if (!IAPManager.vip && !GameController.instance.screens["VipUI"].activeSelf)
        {
            Debug.Log("showBanner"); 
            Appodeal.show(Appodeal.BANNER_BOTTOM);
            //instance.bannerDebug.SetActive(true);
        }
    }
    public void hideBanner()
    {
        Debug.Log("hideBanner");
        Appodeal.hide(Appodeal.BANNER);
        //instance.bannerDebug.SetActive(false);
    }

    #region Interstitial event handlers

    public void onInterstitialFailedToLoad() { }
    public void onInterstitialClicked() { }
    public void onInterstitialExpired() { }
    public void onInterstitialShown() {
        AnalyticsController.sendEvent("InterstitialShown");
    }
    public void onInterstitialLoaded(bool isPrecache) { }
    public void onInterstitialClosed() {
        //StartCoroutine(InterstitialClosedCoroutine());
        Pause(false);
        //point
        //StartCoroutine(UpdateTimer());
        timer = 0f;
        timerTicked = false;
        AnalyticsController.sendEvent("InterstitialClosed");

    }
    public void onInterstitialShowFailed()
    {
        Debug.Log("onInterstitialShowFailed");
    }
    #endregion

    #region Rewarded event handlers

    public void onRewardedVideoExpired() { }
    public void onRewardedVideoFailedToLoad() { }
    public void onRewardedVideoFinished(double amount, string name) { }
    public void onRewardedVideoShown() { }
    public void onRewardedVideoLoaded(bool precache) { }
    public void onRewardedVideoClosed(bool finished) {
        //StartCoroutine(RewardedVideoClosedCoroutine(finished));
        RewardedVideoClosed(finished);
    }
    public void onRewardedVideoClicked() { }
    public void onRewardedVideoShowFailed() { }
    #endregion
    #region Banner callback handlers
    public void onBannerLoaded(bool precache) { showBanner(); }
    public void onBannerFailedToLoad() { print("banner failed"); }
    public void onBannerShown() { print("banner opened"); }
    public void onBannerClicked() { print("banner clicked"); }
    public void onBannerExpired() { print("banner expired"); }
    #endregion

    public static void ShowInterstitial() {
        Debug.Log("Interstitial ShowInterstitial");
        Debug.Log("Interstitial timerTicked: " + timerTicked);
        Debug.Log("Interstitial USER_GROUP_AD: " + PlayerPrefs.GetInt("USER_GROUP_AD", -1));
        Debug.Log("Interstitial IAPManager.vip: " + IAPManager.vip);
        Debug.Log("Interstitial ready: " + IsInterstitialReady);
        if (IsInterstitialReady && !IAPManager.vip && (timerTicked || PlayerPrefs.GetInt("USER_GROUP_AD", -1) < 3) && LevelController.level >= PlayerPrefs.GetInt("USER_GROUP_AD", -1)) {
            Debug.Log("ShowInterstitial 2");
            Pause(true);
            AnalyticsController.sendEvent("InterstitialShow");

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

            DontDestroyOnLoad(gameObject);

            var appKey = "";
#if UNITY_IOS
        appKey = IOS_APP_KEY;
#elif UNITY_ANDROID
            appKey = ANDROID_APP_KEY;
#endif
            Appodeal.initialize(appKey, Appodeal.REWARDED_VIDEO | Appodeal.INTERSTITIAL | Appodeal.BANNER);

            Appodeal.setInterstitialCallbacks(this);
            Appodeal.setRewardedVideoCallbacks(this);
            Appodeal.setBannerCallbacks(this);

            showBanner();

        }
        else  {
            Destroy(gameObject);
        }

    }

    private IEnumerator InterstitialClosedCoroutine() {
        yield return new WaitForSecondsRealtime(0.1f);
        Pause(false);
        OnInterstitialWatched?.Invoke();
    }
    private void RewardedVideoClosed(bool finished) {
        Pause(false);
        if (finished) {
            Debug.Log("OnRewardedVideoClosed and finished=true");
            giveReward?.Invoke();
        }
        else {
            Debug.Log("OnRewardedVideoClosed and finished=false");
        }
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
        AudioListener.pause = isOn;
        if (isOn) {
            //Time.timeScale = 0f;
            //audioManager.Mute();
            
        }
        else {
            //Time.timeScale = 1f;
            //audioManager.Unmute();
        }
    }
    private void Update() {
        if (!timerTicked) {
            if (timer < interstitialInterval) {
                timer += Time.deltaTime;
            }
            else
                timerTicked = true;
        }
    }

    //this because AdController dont destroy
    public IEnumerator shieldAdCoroutine() {
        Debug.Log("giveReward shieldAdClick");
        yield return StartCoroutine(GameController.instance.shieldAdCoroutine());

    }
}
