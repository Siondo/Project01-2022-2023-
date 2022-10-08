package com.common.sdk;

import android.util.Log;

import com.jiujiuji.CrushTask.openmediation.BannerVideo;
import com.jiujiuji.CrushTask.openmediation.ImpressionVideoData;
import com.jiujiuji.CrushTask.openmediation.InterstitialVideo;
import com.jiujiuji.CrushTask.openmediation.NativeVideo;
import com.jiujiuji.CrushTask.openmediation.RewardedVideo;
import com.jiujiuji.CrushTask.openmediation.SplashVideo;
import com.openmediation.sdk.InitCallback;
import com.openmediation.sdk.InitConfiguration;
import com.openmediation.sdk.OmAds;
import com.openmediation.sdk.mobileads.IronSourceSetting;
import com.openmediation.sdk.utils.error.Error;

public class OpenMediationSDK extends SDKBase
{
    public static String Tag = "OpenMediationSDK";
    public static String AppKey = "2zfrgcifaCBZnluazpLKdxiDbeqB3qJh";

    @Override
    public void onCreate() {
        Log.e(Tag, "初始化开始...");

        IronSourceSetting.setMediationMode(true);
        InitConfiguration configuration = new InitConfiguration.Builder()
                .appKey(AppKey)
                .logEnable(false)
                .preloadAdTypes(OmAds.AD_TYPE.INTERSTITIAL, OmAds.AD_TYPE.REWARDED_VIDEO)
                .build();
        OmAds.init(configuration, new InitCallback() {
            @Override
            public void onSuccess() {
                RewardedVideo.onInitRewardedVideoAd();     //激励广告初始化
                InterstitialVideo.onInitInterstitialAd();  //插屏广告初始化
                BannerVideo.onInitBannerAd();              //横幅广告初始化
                SplashVideo.onInitSplashVideoAd();         //开屏广告初始化
                NativeVideo.onInitNativeVideoAd();         //原生广告初始化
                ImpressionVideoData.onInitImpressionVideoData(); //广告数据监听器初始化
                Log.e(Tag, "初始化完毕");
            }

            @Override
            public void onError(Error error) {
                Log.e(Tag, "错误报告" + error);
            }
        });
    }

    @Override
    public void onExit() {
        super.onExit();
    }
}
