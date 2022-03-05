using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdController : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    float adCount;

    public static AdController instance;
    void Awake()
    {
        adCount = DataController.instance.adCount;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        MobileAds.Initialize(InitializationStatus => { });
        RequestRewarded();
        RequestInterstitial();
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    public void RequestBanner()
    {
#if UNITY_ANDROID
        string testBannerId = "ca-app-pub-3940256099942544/6300978111";
        bannerAd = new BannerView(testBannerId, AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd.LoadAd(CreateAdRequest());
#elif UNITY_IPHONE
        string testBannerId = "ca-app-pub-3940256099942544/2934735716";
                bannerAd = new BannerView(testBannerId, AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd.LoadAd(CreateAdRequest());
#endif
    }

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string testInterstitialAd = "ca-app-pub-3940256099942544/1033173712";
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(testInterstitialAd);
        interstitialAd.LoadAd(CreateAdRequest());
#elif UNITY_IPHONE
        string testInterstitialAd = "ca-app-pub-3940256099942544/4411468910";
                if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(testInterstitialAd);
        interstitialAd.LoadAd(CreateAdRequest());
#endif


    }

    public void ShowInterstitial()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("InterstitialAd");
        }
    }

    void RequestRewarded()
    {
#if UNITY_ANDROID
        string testRewardAd = "ca-app-pub-3940256099942544/5224354917";
        rewardedAd = new RewardedAd(testRewardAd);

        rewardedAd.LoadAd(CreateAdRequest());
#elif UNITY_IPHONE
        string testRewardAd = "ca-app-pub-3940256099942544/1712485313";
                rewardedAd = new RewardedAd(testRewardAd);

        rewardedAd.LoadAd(CreateAdRequest());
#endif
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }

    }

    public void ShowRewarded()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            Debug.Log("error loading Rewards");
        }

        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
    }

    void HandleUserEarnedReward(object sender, Reward args)
    {
        //Were not going to call this
        GameMenuController.gmmInstance.AwardResurrection();
        RequestInterstitial();
    }
    void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        GameMenuController.gmmInstance.GameOver();
    }
    void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // Eventually do Sorry for Inconvenience Pop Up
        GameMenuController.gmmInstance.GameOver();
    }
}
