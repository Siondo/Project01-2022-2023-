package com.common.manager;

import com.common.sdk.*;
import com.jiujiuji.CrushTask.openmediation.BannerVideo;
import com.jiujiuji.CrushTask.openmediation.ImpressionVideoData;
import com.unity3d.player.UnityPlayer;

import android.Manifest;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.util.Log;

import androidx.core.app.ActivityCompat;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import com.common.util.InstallActivity;

//管理器
public class Manager
{
    //TAG
    private static String TAG = "JJJ_Manager";

    public static final String META_DATA_RELEASE         = "jjj.release";
    public static final String META_DATA_UMAPPKEY        = "jjj.um.appKey";
    public static final String META_DATA_WXAPPID         = "jjj.wx.appId";
    public static final String META_DATA_WXAPPSECRET     = "jjj.wx.appSecret";
    public static final String META_DATA_TopOnAppId     = "jjj.topon.appId";
    public static final String META_DATA_TopOnAppKey     = "jjj.topon.appKey";

    //管理器单例
    private static final Manager g_instance = new Manager();

    //活动窗口
    private static Activity g_activity;

    //应用信息
    private ApplicationInfo m_appInfo = null;

    //是否是Release版本
    private static boolean g_release = true;

    //SDK管理器
    private SDKManager m_sdkManager;

    public String userId = "";
    public String channel = "default";
    public String subChannel = "default";

    /**
     * 得到单例对象
     * @return
     */
    public static Manager getInstance()
    {
        return g_instance;
    }

    /**
     * 得到当前活动窗口
     * @return
     */
    public static Activity getActivity()
    {
        return g_activity;
    }

    /**
     * 得到是否是Release模式
     * @return
     */
    public static boolean getReleaseBoolean()
    {
        return g_release;
    }

    /**
     * 得到MetaData数据boolean
     * @param key
     * @return
     */
    public boolean getMetaDataBoolean(String key)
    {
        boolean result = false;
        try
        {
            if (null != m_appInfo)
            {
                result = m_appInfo.metaData.getBoolean(key);
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, String.format("getBoolean(%s), %s", key, e.toString()));
        }
        return result;
    }

    /**
     * 得到MetaData数据String
     * @param key
     * @return
     */
    public String getMetaDataString(String key)
    {
        String result = "";
        try
        {
            if (null != m_appInfo)
            {
                result = m_appInfo.metaData.getString(key);
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, String.format("getString(%s), %s", key, e.toString()));
        }
        return result;
    }

    /**
     * 得到MetaData数据float
     * @param key
     * @return
     */
    public float getMetaDataFloat(String key)
    {
        float result = 0;
        try
        {
            if (null != m_appInfo)
            {
                result = m_appInfo.metaData.getFloat(key);
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, String.format("getFloat(%s), %s", key, e.toString()));
        }
        return result;
    }

    /**
     * 得到MetaData数据int
     * @param key
     * @return
     */
    public int getMetaDataInt(String key)
    {
        int result = 0;
        try
        {
            if (null != m_appInfo)
            {
                result = m_appInfo.metaData.getInt(key);
            }
        }
        catch (Exception e)
        {
            Log.e(TAG, String.format("getInt(%s), %s", key, e.toString()));
        }
        return result;
    }

    /**
     * 启动
     * @param activity
     */
    public void onCreate(Activity activity)
    {
        g_activity = activity;
        //获取App信息
        try
        {
            PackageManager pm = g_activity.getPackageManager();
            m_appInfo = pm.getApplicationInfo(g_activity.getPackageName(), PackageManager.GET_META_DATA);
        }
        catch (Exception e)
        {
            Log.e(TAG, String.format("getApplicationInfo(), %s", e.toString()));
        }
        //是否是Release版本
        g_release = getMetaDataBoolean(META_DATA_RELEASE);
        Log.i(TAG, String.format("release: %s", g_release));

        //权限请求
        String permission = null;
        requestPermission(permission);

        //启动SDK管理器
        m_sdkManager = new SDKManager();
        m_sdkManager.onCreate();
    }

    /**
     * 请求权限
     * @param permission
     */
    public void requestPermission(String permission) {
        if (null == permission || permission.length() == 0) {
            return;
        }
        permission = Manifest.permission.ACCESS_FINE_LOCATION
                + ";" + Manifest.permission.ACCESS_COARSE_LOCATION
                + ";" + Manifest.permission.ACCESS_NETWORK_STATE
                + ";" + Manifest.permission.READ_PHONE_STATE
                + ";" + Manifest.permission.WRITE_EXTERNAL_STORAGE;
        requestPermission(getActivity(), permission);
    }

    /**
     * 请求权限
     */
    private void requestPermission(Activity activity, String permission) {
        String[] permissionList = permission.split(";");

        ArrayList<String> arrayList = new ArrayList<>();
        if (permissionList != null) {
            Log.i(TAG,"需要权限：" + arrayList.size());
            for (int i = 0; i < permissionList.length; ++i) {
                permission = permissionList[i];
                int selfPermission = androidx.core.content.ContextCompat.checkSelfPermission(activity, permission);
                if (selfPermission != PackageManager.PERMISSION_GRANTED) {
                    arrayList.add(permission);
                }
            }
        }

        permissionList = new String[arrayList.size()];
        arrayList.toArray(permissionList);
        if (permissionList != null && permissionList.length > 0) {
            Log.i(TAG,"申请权限：" + permissionList.length);
            ActivityCompat.requestPermissions(activity, permissionList, 0);
        }
    }

    /**
     * 暂停
     */
    public void onPause()
    {
        Log.i(TAG, "onPause");
        if (null != m_sdkManager) {
            m_sdkManager.onPause();
        }
    }

    /**
     * 重启
     */
    public void onResume()
    {
        Log.i(TAG, "onResume");
        m_sdkManager.onResume();
    }

    /**
     * 登录
     * @param msg
     */
    public void onLogin(String msg)
    {
        Log.i(TAG, "onLogin");
        m_sdkManager.onLogin(msg);
    }

    /**
     * 发送消息到Unity
     * @param msg
     */
    public void SendLoginBack(String msg)
    {
        UnityPlayer.UnitySendMessage("SDKManager", "LoginBack", msg);
    }

    /**
     * 登录成功
     * @param msg
     */
    public void onLoginSuccess(String msg)
    {
        Log.i(TAG, "onLoginSuccess: " + msg);
        m_sdkManager.onLoginSuccess(msg);
    }

    /**
     * 登出
     */
    public void onLogout()
    {
        Log.i(TAG, "onLogout");
        m_sdkManager.onLogout();
    }

    /**
     * 退出
     */
    public void onExit()
    {
        Log.i(TAG, "onExit");
        m_sdkManager.onExit();
    }

    /**
     * 自定义数据埋点
     * @param msg
     */
    public void onEventObject(String msg)
    {
        Log.i(TAG, "onEventObject: " + msg);
        m_sdkManager.onEventObject(g_activity, msg);
    }

    /**
     * 设备标识符
     * @param oaid
     */
    public void SendOaid(String oaid) {
		if (null == oaid) {
			oaid = "";
		}
        UnityPlayer.UnitySendMessage("SDKManager", "SetOaid", oaid);
    }

    /**
     * 设置用户的信息
     * @param msg
     */
    public void SetInfo(String msg) {
        if (msg != null) {
            Log.i(TAG, String.format("SetInfo: %s", msg));

            try {
                JSONObject json = new JSONObject(msg);
                if (json.has("userId")) {
                    userId = json.getString("userId");
                }
            }
            catch (JSONException e)
            {
                Log.e(TAG, String.format("SetInfo: %s", e.toString()));
            }
        }
    }

    /**
     * 显示激励广告
     */
    public void onRewardVideoAdShow(String scenario)
    {
        Log.i(TAG, "onRewardVideoAdShow");
        m_sdkManager.onRewardVideoAdShow(scenario);
    }

    /**
     * 激励广告显示完成
     * @param msg
     */
    public void OnRewardVideoAdShowFinished(String msg)
    {
        Log.i(TAG, "OnRewardVideoAdShowFinished: " + msg);
        UnityPlayer.UnitySendMessage("SDKManager", "RewardVideoAdShowFinished", msg);
    }

    /**
     * 显示插屏广告
     */
    public void onInterstitialAdShow(String scenario)
    {
        Log.i(TAG, "onInterstitialAdShow");
        m_sdkManager.onInterstitialAdShow(scenario);
    }

    /**
     * 插屏广告显示完成
     * @param msg
     */
    public void OnInterstitialAdShowFinished(String msg)
    {
        Log.i(TAG, "OnInterstitialAdShowFinished: " + msg);
        UnityPlayer.UnitySendMessage("SDKManager", "InterstitialAdShowFinished", msg);
    }

    /**
     * 显示横幅广告
     */
    public void onBannerAdShow(String msg)
    {
        m_sdkManager.onBannerAdShow(msg);
    }

    /**
     * 横幅广告显示完成
     * @param msg
     */
    public void OnBannerAdHide(String msg)
    {
        Log.i(TAG, "OnBannerAdHide: " + msg);
        UnityPlayer.UnitySendMessage("SDKManager", "BannerAdShowFinished", msg);
    }

    /**
     * 隐藏横幅广告
     */
    public void BannerAdHide()
    {
        Log.i(TAG, "BannerAdHide");
        m_sdkManager.BannerAdHide();
    }

    /**
     * 显示开屏广告
     */
    public void onSplashAdShow()
    {
        Log.i(TAG, "onSplashAdShow");
        m_sdkManager.onSplashAdShow();
    }

    /**
     * 开屏广告显示完成
     * @param msg
     */
    public void OnSplashAdShowFinished(String msg)
    {
        Log.i(TAG, "OnSplashAdShowFinished: " + msg);
        UnityPlayer.UnitySendMessage("SDKManager", "SplashAdShowFinished", msg);
    }

    /**
     * 显示原生广告
     */
    public void OnNativeAdShow()
    {
        m_sdkManager.onNativeAdShow();
    }

    /**
     * 原生广告显示完成
     * @param msg
     */
    public void OnNativeAdShowFinished(String msg)
    {
        UnityPlayer.UnitySendMessage("SDKManager", "NativeAdShowFinished", msg);
    }

    /**
     * 应用内安装
     * @param apkInfo
     */
    public void InstallAPK(String apkInfo)
    {
        Log.i(TAG, "InstallAPK: " + apkInfo);
        Intent intent = new Intent(g_activity, InstallActivity.class);
        intent.putExtra("apkInfo", apkInfo);
        g_activity.startActivity(intent);
    }

    /**
     * 退出应用
     */
    public void QuitAPP() {
        Log.i(TAG, "QuitAPP");
        BannerVideo.onDestroyBannerAd();
        ImpressionVideoData.onRelease();
        UnityPlayer.UnitySendMessage("SDKManager", "QuitAPP", "");
    }

    //Tenjin自定义事件触发器
    public void onReceiveTenjinsEvent(String msg)
    {
        m_sdkManager.onReceiveTenjinsEvent(msg);
    }
}