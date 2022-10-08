package com.jiujiuji.CrushTask.openmediation;

import android.app.Activity;
import android.util.Log;

import com.common.manager.Manager;
import com.common.sdk.BuglySDK;
import com.common.sdk.OpenMediationSDK;
import com.openmediation.sdk.OmAds;
import com.openmediation.sdk.ImpressionData;
import com.openmediation.sdk.ImpressionDataListener;
import com.openmediation.sdk.utils.error.Error;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

public class ImpressionVideoData extends Activity
{
    private static ImpressionDataListener listener;
    public static JSONObject RewardedJsonInfo = new JSONObject();
    public static JSONObject InterstitialJsonInfo = new JSONObject();
    public static JSONObject BannerJsonInfo = new JSONObject();
    public static JSONObject NativeJsonInfo = new JSONObject();
    public static JSONObject SplashJsonInfo = new JSONObject();

    public static void onInitImpressionVideoData()
    {
        listener = new ImpressionDataListener() {
            @Override
            public void onImpression(Error error, ImpressionData impressionData) {
                if (impressionData != null) {
                    String type = impressionData.getPlacementAdType();
                    JSONObject json = impressionData.getAllData();
                    switch (type)
                    {
                        case "Rewarded Video":
                            RewardedJsonInfo = json;
                            break;
                        case "Interstitial":
                            InterstitialJsonInfo = json;
                            break;
                        case "Banner":
                            BannerJsonInfo = json;
                            break;
                        case "Native":
                            NativeJsonInfo = json;
                            break;
                        case "Splash":
                            SplashJsonInfo = json;
                            break;
                    }
                    Log.e(OpenMediationSDK.Tag + "- 存储时广告类型: " + type + ": ", json.toString());
                }
            }
        };
        OmAds.addImpressionDataListener(listener);
    }

    public static void onSendCallBack(String type, JSONObject json, boolean isSuccess)
    {
        try
        {
            JSONObject info = new JSONObject();
            info.put("status", isSuccess);
            info.put("adInfo", json);

            switch (type)
            {
                case "Rewarded Video":
                    Log.e(OpenMediationSDK.Tag, "执行Java层Call Unity步骤");
                    Manager.getInstance().OnRewardVideoAdShowFinished(info.toString());
                    break;

                case "Interstitial":
                    Manager.getInstance().OnInterstitialAdShowFinished(info.toString());
                    break;

                case "Banner":
                    Manager.getInstance().OnBannerAdHide(info.toString());
                    break;

                case "Native":
                    Manager.getInstance().OnNativeAdShowFinished(info.toString());
                    break;

                case "Splash":
                    Manager.getInstance().OnSplashAdShowFinished(info.toString());
                    break;
            }

            Log.e(OpenMediationSDK.Tag + "- 读取时广告类型: " + type + ": ", info.toString());
        }
        catch (JSONException e) {
            BuglySDK.postCatchedException(e);
        }
    }

    public static void onRelease()
    {
        OmAds.removeImpressionDataListener(listener);
    }
}
