package com.jiujiuji.CrushTask.openmediation;

import android.app.Activity;
import android.util.Log;
import android.view.KeyEvent;
import android.view.Window;

import com.common.manager.Manager;
import com.common.sdk.BuglySDK;
import com.common.sdk.OpenMediationSDK;
import com.openmediation.sdk.utils.error.Error;
import com.openmediation.sdk.utils.model.Scene;
import com.openmediation.sdk.video.RewardedVideoAd;
import com.openmediation.sdk.video.RewardedVideoListener;

import org.json.JSONException;
import org.json.JSONObject;

public class RewardedVideo extends Activity
{
    //是否加载完毕
    private static boolean isInitCompletely = false;

    public static void onPlayRewardedVideoAd(String scenario)
    {
        if (isInitCompletely) {
            Log.e(OpenMediationSDK.Tag,"激励广告开始播放 (场景信息: " + scenario + ")");
            RewardedVideoAd.showAd(scenario);
        }
        else {
            Log.e(OpenMediationSDK.Tag,"无法播放激励广告, 没有初始化成功");
            ImpressionVideoData.onSendCallBack("Rewarded Video", new JSONObject(), false);
        }
    }

    public static void onInitRewardedVideoAd()
    {
        RewardedVideoAd.setAdListener(new RewardedVideoListener()
        {
            @Override
            //当激励广告预加载完毕后进行的事件监听
            public void onRewardedVideoAvailabilityChanged(boolean isSuccess) {
                if (isSuccess) {
                    isInitCompletely = true;
                }
            }
            @Override
            public void onRewardedVideoAdShowed(Scene scene) { }
            @Override
            public void onRewardedVideoAdShowFailed(Scene scene, Error error) {
                Log.e(OpenMediationSDK.Tag,"激励给广告播放失败");
                ImpressionVideoData.onSendCallBack("Rewarded Video", new JSONObject(), false);
            }
            @Override
            public void onRewardedVideoAdClicked(Scene scene) { }
            @Override
            public void onRewardedVideoAdClosed(Scene scene) { }
            @Override
            public void onRewardedVideoAdStarted(Scene scene) { }
            @Override
            public void onRewardedVideoAdEnded(Scene scene) { }
            @Override
            public void onRewardedVideoAdRewarded(Scene scene) {
                Log.e(OpenMediationSDK.Tag,"激励给广告观看完毕");
                ImpressionVideoData.onSendCallBack("Rewarded Video", ImpressionVideoData.RewardedJsonInfo, true);
            }
        });
    }
}
