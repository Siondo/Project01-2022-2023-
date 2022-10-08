package com.jiujiuji.CrushTask.openmediation;

import android.app.Activity;
import android.util.Log;
import android.view.ViewGroup;
import android.widget.RelativeLayout;

import com.common.manager.Manager;
import com.common.sdk.OpenMediationSDK;
import com.openmediation.sdk.splash.SplashAd;
import com.openmediation.sdk.splash.SplashAdListener;
import com.openmediation.sdk.utils.error.Error;

import org.json.JSONObject;

public class SplashVideo extends Activity
{
    private static String placementId = "11585";
    public static ViewGroup container = null;

    public static void onSplashAdShow()
    {
        Log.e(OpenMediationSDK.Tag,"开屏广告开始拉取...");
        SplashAd.loadAd(placementId);
    }

    public static void onInitSplashVideoAd()
    {
        SplashAd.setSplashAdListener(placementId, new SplashAdListener() {
            @Override
            public void onSplashAdLoaded(String s) {
                if (SplashAd.isReady(placementId)) {
                    Log.e(OpenMediationSDK.Tag,"开屏广告拉取完毕可以播放");
                    if (container != null)
                    {
                        SplashAd.showAd(placementId, container);
                    }
                    else
                    {
                        SplashAd.showAd(placementId);
                    }
                }
                else {
                    Log.e(OpenMediationSDK.Tag,"开屏广告无法播放");
                    ImpressionVideoData.onSendCallBack("Splash", new JSONObject(), false);
                }
            }

            @Override
            public void onSplashAdFailed(String s, Error error) {
                Log.e(OpenMediationSDK.Tag,"开屏广告加载失败");
                ImpressionVideoData.onSendCallBack("Splash", new JSONObject(), false);
            }

            @Override
            public void onSplashAdClicked(String s) { }

            @Override
            public void onSplashAdShowed(String s) {
                ImpressionVideoData.onSendCallBack("Splash", ImpressionVideoData.SplashJsonInfo, true);
            }

            @Override
            public void onSplashAdShowFailed(String s, Error error) {
                ImpressionVideoData.onSendCallBack("Splash", new JSONObject(), false);
            }

            @Override
            public void onSplashAdTick(String s, long l) { }

            @Override
            public void onSplashAdDismissed(String s) { }
        });
    }
}
