using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using XLua;
using Framework.Singleton;

public sealed class SDKManager : MonoBehaviour
{
    /// <summary>
    /// 登录方式
    /// </summary>
    public enum LoginType
    {
        WX = 1,
    }

    /// <summary>
    /// SDK管理器
    /// </summary>
    private static SDKManager m_instance = null;

    /// <summary>
    /// 当前SDK
    /// </summary>
    private SDKBase m_sdk = null;

    /// <summary>
    /// 得到当前SDK
    /// </summary>
    public static SDKManager instance
    {
        get { return m_instance; }
    }

    /// <summary>
    /// 实例化SDK
    /// </summary>
    public static void InstanceSDK()
    {
        if (null == m_instance)
        {
            GameObject sdkObj = new GameObject(typeof(SDKManager).Name);
            sdkObj.transform.SetParent(null, false);
            sdkObj.transform.localPosition = Vector3.zero;
            sdkObj.transform.localRotation = Quaternion.identity;
            sdkObj.transform.localScale = Vector3.one;
            m_instance = sdkObj.AddComponent<SDKManager>();
            GameObject.DontDestroyOnLoad(sdkObj);
        }
    }

    /// <summary>
    /// 启动
    /// </summary>
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
#if UNITY_EDITOR
        m_sdk = new EditorSDK();
#elif UNITY_ANDROID
        m_sdk = new AndroidSDK();
#elif UNITY_IOS
#endif
        m_sdk.Init();
    }

    /// <summary>
    /// 数据埋点
    /// </summary>
    /// <param name="eventkey"></param>
    /// <param name="eventValue"></param>
    public void OnEventObject(string eventkey, string eventValue = "")
    {
        m_sdk.OnEventObject(eventkey, eventValue);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="loginType"></param>
    /// <param name="callback"></param>
    public void OnLogin(LoginType loginType, Action<SDKBase.LoginCode> callback)
    {
        m_sdk.OnLogin(loginType, callback);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="loginType"></param>
    /// <param name="callback"></param>
    public void OnAutoLogin(LoginType loginType, Action<SDKBase.LoginCode> callback)
    {
        m_sdk.OnAutoLogin(loginType, callback);
    }

    /// <summary>
    /// 登录返回
    /// </summary>
    /// <param name="msg"></param>
    public void LoginBack(string msg)
    {
        m_sdk.OnLoginBack(msg);
    }

    /// <summary>
    /// 登录成功
    /// </summary>
    public void LoginSuccessed()
    {
        m_sdk.OnLoginSuccessed();
    }

    /// <summary>
    /// 请求权限
    /// </summary>
    /// <param name="permission"></param>
    public void RequestPermission(String permission)
    {
        m_sdk.RequestPermission(permission);
    }

    /// <summary>
    /// 显示激励广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public void RewardVideoAdShow(string scenario, Action<string> callback)
    {
        m_sdk.RewardVideoAdShow(scenario, callback);
    }

    /// <summary>
    /// 激励广告结束
    /// </summary>
    /// <param name="msg"></param>
    public void RewardVideoAdShowFinished(string msg)
    {
        m_sdk.RewardVideoAdShowFinished(msg);
    }

    /// <summary>
    /// 插屏激励广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public void InterstitialAdShow(string scenario, Action<string> callback)
    {
        m_sdk.InterstitialAdShow(scenario, callback);
    }

    /// <summary>
    /// 插屏广告结束
    /// </summary>
    /// <param name="msg"></param>
    public void InterstitialAdShowFinished(string msg)
    {
        m_sdk.InterstitialAdShowFinished(msg);
    }

    /// <summary>
    /// 展示横幅广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public void BannerAdShow(string scenario, Action<string> callback)
    {
        m_sdk.BannerAdShow(scenario, callback);
    }

    /// <summary>
    /// 横幅广告展示完毕
    /// </summary>
    /// <param name="msg"></param>
    public void BannerAdShowFinished(string msg)
    {
        m_sdk.BannerAdShowFinished(msg);
    }

    /// <summary>
    /// 隐藏横幅广告
    /// </summary>
    public void BannerAdHide()
    {
        m_sdk.BannerAdHide();
    }

    /// <summary>
    /// 展示开屏广告>
    /// </summary>
    public void SplashAdShow(Action<string> callback)
    {
        m_sdk.SplashAdShow(callback);
    }

    /// <summary>
    /// 开屏广告结束
    /// </summary>
    /// <param name="msg"></param>
    public void SplashAdShowFinished(string msg)
    {
        m_sdk.SplashAdShowFinished(msg);
    }

    /// <summary>
    /// 展示原生广告>
    /// </summary>
    public void NativeAdShow(Action<string> callback)
    {
        m_sdk.NativeAdShow(callback);
    }

    /// <summary>
    /// 原生广告结束
    /// </summary>
    /// <param name="msg"></param>
    public void NativeAdShowFinished(string msg)
    {
        m_sdk.NativeAdShowFinished(msg);
    }

    public void SendTenjinEvent(string msg)
    {
        m_sdk.SendTenjinEvent(msg);
    }


    /// <summary>
    /// 应用内安装包
    /// </summary>
    /// <param name="filePath"></param>
    public void InstallAPK(string filePath)
    {
        m_sdk.InstallAPK(filePath);
    }

    /// <summary>
    /// 退出应用
    /// </summary>
    /// <param name="filePath"></param>
    public void QuitAPP(string msg)
    {
        Application.Quit();
    }
}
