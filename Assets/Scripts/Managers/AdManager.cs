using System;
using Tools;
using Tools.ScriptableObjects.References;
using UnityEngine;

namespace Managers
{
    public class AdManager : MonoBehaviour
    {
        public IntLinearCurve rewardCurve;
        public IntReference maxAvailableLevel;

#if UNITY_ANDROID
        private const string AppKey = "194a10405";
#elif UNITY_IOS
        private const string AppKey = "";
#else
        private const string AppKey = "Unknown platform";
#endif
        
        private void Awake()
        {
            IronSource.Agent.init(AppKey);
        }

        private void OnEnable()
        {
            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;

            //Add AdInfo Banner Events
            IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
            IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
            IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
            IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
            IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
            IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
            
            //Add AdInfo Interstitial Events
            IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
            IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
            IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
            IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;
            
            // Add AdInfo Rewarded Video Events
            IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
            IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
            IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
        }

        private void OnApplicationPause(bool isPaused) {                 
            IronSource.Agent.onApplicationPause(isPaused);
        }

        private void SdkInitializationCompletedEvent() { 
            IronSource.Agent.validateIntegration();
        }
        
        // Buttons methods

        public void ShowBannerAd(bool show)
        {
            if (show)
            {
                IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
                Debug.Log("Banner ad is shown");
            }
            else
            {
                IronSource.Agent.destroyBanner();
                Debug.Log("Banner ad is not shown");
            }
        }

        public void ShowInterstitialAd()
        {
            if (IronSource.Agent.isInterstitialReady())
            {
                IronSource.Agent.showInterstitial();
                Debug.Log("Interstitial ad is ready and shown");
            }
            else
            {
                Debug.Log("Interstitial ad is not ready");
            }
        }

        public void ShowRewardedAd()
        {
            if (IronSource.Agent.isRewardedVideoAvailable())
            {
                IronSource.Agent.showRewardedVideo();
                Debug.Log("Rewarded ad is ready and shown");
            }
            else
            {
                Debug.Log("Rewarded ad is not ready");
            }
        }

        /****************** Ad Event handlers ******************/
        
        #region Banner Callbacks
        
        /************* Banner AdInfo Delegates *************/
        //Invoked once the banner has loaded
        private void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo) { }
        //Invoked when the banner loading process has failed.
        private void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError) { }
        // Invoked when end user clicks on the banner ad
        private void BannerOnAdClickedEvent(IronSourceAdInfo adInfo) { }
        //Notifies the presentation of a full screen content following user click
        private void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo) { }
        //Notifies the presented screen has been dismissed
        private void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo) { }
        //Invoked when the user leaves the app
        private void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo) { }        

        #endregion
        
        #region Interstitial Callbacks
        
        /************* Interstitial AdInfo Delegates *************/
        // Invoked when the interstitial ad was loaded successfully.
        private void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo) { }
        // Invoked when the initialization process has failed.
        private void InterstitialOnAdLoadFailed(IronSourceError ironSourceError) { }
        // Invoked when the Interstitial Ad Unit has opened. This is the impression indication. 
        private void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo) { }
        // Invoked when end user clicked on the interstitial ad
        private void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo) { }
        // Invoked when the ad failed to show.
        private void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo) { }
        // Invoked when the interstitial ad closed and the user went back to the application screen.
        private void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo) { }
        // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
        // This callback is not supported by all networks, and we recommend using it only if  
        // it's supported by all networks you included in your build. 
        private void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo) { }
        
        #endregion
        
        #region Rewarded Callbacks
        
        /************* RewardedVideo AdInfo Delegates *************/
        // Indicates that there’s an available ad.
        // The adInfo object includes information about the ad that was loaded successfully
        // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
        private void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo) { }
        // Indicates that no ads are available to be displayed
        // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
        private void RewardedVideoOnAdUnavailable() { }
        // The Rewarded Video ad view has opened. Your activity will loose focus.
        private void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo) { }
        // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
        private void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo) { }
        // The user completed to watch the video, and should be rewarded.
        // The placement parameter will include the reward data.
        // When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
        private void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            MoneyManager.DepositMoney(rewardCurve.ForceEvaluate(maxAvailableLevel.Value));
            
            Debug.Log("Reward is got");
        }
        // The rewarded video ad was failed to show.
        private void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo) { }
        // Invoked when the video ad was clicked.
        // This callback is not supported by all networks, and we recommend using it only if
        // it’s supported by all networks you included in your build.
        private void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo) { }

        #endregion
    }
}
