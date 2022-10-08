package com.common.sdk;

import android.app.Activity;
import android.content.Intent;
import android.util.Log;

import com.common.manager.Manager;
import com.jiujiuji.CrushTask.openmediation.BannerVideo;
import com.jiujiuji.CrushTask.openmediation.InterstitialVideo;
import com.jiujiuji.CrushTask.openmediation.NativeVideo;
import com.jiujiuji.CrushTask.openmediation.RewardedVideo;
import com.jiujiuji.CrushTask.openmediation.SplashVideo;

import org.json.JSONObject;
import org.json.JSONException;

public class SDKManager
{
    //TAg
    private static String TAG = "JJJ_SDKManager";

    //Bugly
    private BuglySDK m_buglySDK;

    //OpenMediation
    private OpenMediationSDK m_openMediationSDK;

    //Tenjin
    public cTenjinSDK m_tenjinSDK;

    //UMengSDK
    private UMengSDK m_uMengSDK;

    //是否登录成功
    private boolean m_loginSccessed = false;

    /**
     * 启动
     */
    public void onCreate()
    {
        m_buglySDK = new BuglySDK();
        m_buglySDK.onCreate();

        m_openMediationSDK = new OpenMediationSDK();
        m_openMediationSDK.onCreate();

        m_tenjinSDK = new cTenjinSDK();
        m_tenjinSDK.onCreate();

        m_uMengSDK = new UMengSDK();
        m_uMengSDK.onCreate();
    }

    /**
     * 暂停
     */
    public void onPause()
    {

    }

    /**
     * 重启
     */
    public void onResume()
    {
        m_tenjinSDK.onResume();
    }

    /**
     * 登录
     * @param msg
     */
    public void onLogin(String msg)
    {
    }

    /**
     * 登录成功
     * @param msg
     */
    public void onLoginSuccess(String msg)
    {
        try
        {
            JSONObject json = new JSONObject(msg);

            m_uMengSDK.onLoginSuccess(json);

            m_loginSccessed = true;
        }
        catch (JSONException e)
        {
            BuglySDK.postCatchedException(e);
            Log.e(TAG, String.format("onLoginSuccess: error: %s", e.toString()));
        }
    }

    /**
     * 登出
     */
    public void onLogout()
    {
        m_uMengSDK.onLogout();
    }

    /**
     * 退出
     */
    public void onExit()
    {
        if (m_loginSccessed)
        {
            onLogout();
        }
        m_uMengSDK.onExit();
    }

    /**
     * 自定义数据埋点
     * @param context
     * @param msg
     */
    public void onEventObject(Activity context, String msg)
    {
        m_uMengSDK.onEventObject(context, msg);
    }

    /**
     * 上报数据
     * @param msg
     */
    public void onReceiveTenjinsEvent(String msg)
    {
        m_tenjinSDK.ReportData(msg);
    }

    /**
     * 展示激励广告
     */
    public void onRewardVideoAdShow(String scenario)
    {
        RewardedVideo.onPlayRewardedVideoAd(scenario);
    }

    /**
     * 显示插屏广告
     */
    public void onInterstitialAdShow(String scenario)
    {
        InterstitialVideo.onPlayInterstitialVideoAd(scenario);
    }

    /**
     * 显示横幅广告
     */
    public void onBannerAdShow(String scenario)
    {
        BannerVideo.onShowBannerAd();
    }

    /**
     * 隐藏横幅广告
     */
    public void BannerAdHide()
    {
        BannerVideo.BannerAdHide();
    }

    /**
     * 显示开屏广告
     */
    public void onSplashAdShow()
    {
        SplashVideo.onSplashAdShow();
    }

    /**
     * 显示原生广告
     */
    public void onNativeAdShow()
    {
        NativeVideo.onShowNativeAd();
    }
}