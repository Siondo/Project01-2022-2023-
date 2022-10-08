package com.common.sdk;

import android.app.Activity;
import org.json.JSONObject;

public abstract class SDKBase
{
    public abstract void onCreate();
    public void onPause() {}
    public void onResume() {}
    public void onLogin(String msg) {}
    public void onLoginSuccess(JSONObject json) {}
    public void onLogout() {}
    public void onExit() {}
    public void onEventObject(Activity context, String msg) {}
    public void onSplashAdShow() {}
    public void onBannerAdShow(String placementId) {}
    public void onBannerAdHide() {}
    public void onInterstitialAdShow(String scenario) {}
    public void onRewardVideoAdShow(String scenario) {}
}