package com.jiujiuji.CrushTask.openmediation;

import android.app.Activity;
import android.util.Log;

import com.common.manager.Manager;
import com.common.sdk.BuglySDK;
import com.common.sdk.OpenMediationSDK;
import com.openmediation.sdk.interstitial.InterstitialAd;
import com.openmediation.sdk.interstitial.InterstitialAdListener;
import com.openmediation.sdk.utils.error.Error;
import com.openmediation.sdk.utils.model.Scene;
import com.openmediation.sdk.video.RewardedVideoAd;

import org.json.JSONException;
import org.json.JSONObject;

public class InterstitialVideo extends Activity
{
    //是否加载完毕
    private static boolean isInitCompletely = false;

    public static void onPlayInterstitialVideoAd(String scenario)
    {
        if (isInitCompletely) {
            Log.e(OpenMediationSDK.Tag,"插屏广告开始播放 (场景信息: " + scenario + ")");
            InterstitialAd.showAd(scenario);
        }
        else {
            Log.e(OpenMediationSDK.Tag, "无法播放插屏广告, 没有初始化成功");
            ImpressionVideoData.onSendCallBack("Interstitial", new JSONObject(), false);
        }
    }

    public static void onInitInterstitialAd()
    {
        InterstitialAd.addAdListener(new InterstitialAdListener() {
            @Override
            public void onInterstitialAdAvailabilityChanged(boolean isSuccess) {
                if (isSuccess) {
                    isInitCompletely = true;
                }
            }

            @Override
            public void onInterstitialAdShowed(Scene scene) {
                ImpressionVideoData.onSendCallBack("Interstitial", ImpressionVideoData.InterstitialJsonInfo, true);
            }

            @Override
            public void onInterstitialAdShowFailed(Scene scene, Error error) {
                ImpressionVideoData.onSendCallBack("Interstitial", new JSONObject(), false);
            }
            @Override
            public void onInterstitialAdClosed(Scene scene) { }
            @Override
            public void onInterstitialAdClicked(Scene entity) { }
        });
    }
}
