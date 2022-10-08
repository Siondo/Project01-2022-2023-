using UnityEngine;
using System.Collections;
using System;
using XLua;

[ReflectionUse, Hotfix]
public abstract class SDKBase
{
    /// <summary>
    /// 登录结果
    /// </summary>
    public enum LoginCode
    {
        SUCCESSFUL = 0,
        FAILED = 1,
    }

    /// <summary>
    /// 开屏广告返回
    /// </summary>
    private Action<string> splashAdCallBack
    {
        get; set;
    }

    /// <summary>
    /// 激励广告返回
    /// </summary>
    private Action<string> rewardVideoAdCallBack
    {
        get; set;
    }

    /// <summary>
    /// 插屏广告返回
    /// </summary>
    private Action<string> interstitialAdCallBack
    {
        get; set;
    }

    /// <summary>
    /// 横幅广告返回
    /// </summary>
    private Action<string> bannerAdCallBack
    {
        get; set;
    }

    /// <summary>
    /// 原生广告返回
    /// </summary>
    private Action<string> nativeAdCallBack
    {
        get; set;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init() { }

    /// <summary>
    /// 权限请求
    /// </summary>
    /// <param name="permission"></param>
    public virtual void RequestPermission(String permission) { }

    /// <summary>
    /// 友盟数据埋点
    /// </summary>
    /// <param name="eventkey"></param>
    /// <param name="eventValue"></param>
    public virtual void OnEventObject(string eventkey, string eventValue) { }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="loginType"></param>
    /// <param name="callback"></param>
    public virtual void OnLogin(SDKManager.LoginType loginType, System.Action<LoginCode> callback) { }

    /// <summary>
    /// 自动登录
    /// </summary>
    /// <param name="loginType"></param>
    /// <param name="callback"></param>
    public virtual void OnAutoLogin(SDKManager.LoginType loginType, System.Action<LoginCode> callback) { }

    /// <summary>
    /// 登录返回
    /// </summary>
    /// <param name="msg"></param>
    public virtual void OnLoginBack(string msg) { }

    /// <summary>
    /// 登录成功
    /// </summary>
    public virtual void OnLoginSuccessed() { }

    /// <summary>
    /// 展示激励广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public virtual void RewardVideoAdShow(string scenario, Action<string> callback)
    {
        rewardVideoAdCallBack = callback;
    }

    /// <summary>
    /// 激励广告结束
    /// </summary>
    public virtual void RewardVideoAdShowFinished(string msg)
    {
        rewardVideoAdCallBack?.Invoke(msg);
        rewardVideoAdCallBack = null;
    }

    /// <summary>
    /// 展示插屏广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public virtual void InterstitialAdShow(string scenario, Action<string> callback)
    {
        interstitialAdCallBack = callback;
    }

    /// <summary>
    /// 插屏广告结束
    /// </summary>
    public virtual void InterstitialAdShowFinished(string msg)
    {
        interstitialAdCallBack?.Invoke(msg);
        interstitialAdCallBack = null;
    }

    /// <summary>
    /// 展示横幅广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public virtual void BannerAdShow(string scenario, Action<string> callback)
    {
        bannerAdCallBack = callback;
    }

    /// <summary>
    /// 横幅广告展示完毕
    /// </summary>
    public virtual void BannerAdShowFinished(string msg)
    {
        bannerAdCallBack?.Invoke(msg);
        bannerAdCallBack = null;
    }

    /// <summary>
    /// 隐藏横幅广告
    /// </summary>
    public virtual void BannerAdHide()
    {

    }

    /// <summary>
    /// 展示开屏广告
    /// </summary>
    public virtual void SplashAdShow(Action<string> callback)
    {
        splashAdCallBack = callback;
    }

    /// <summary>
    /// 开屏广告结束
    /// </summary>
    public virtual void SplashAdShowFinished(string msg)
    {
        splashAdCallBack?.Invoke(msg);
        splashAdCallBack = null;
    }

    /// <summary>
    /// 展示原生广告
    /// </summary>
    public virtual void NativeAdShow(Action<string> callback)
    {
        nativeAdCallBack = callback;
    }

    /// <summary>
    /// 开屏原生结束
    /// </summary>
    public virtual void NativeAdShowFinished(string msg)
    {
        nativeAdCallBack?.Invoke(msg);
        nativeAdCallBack = null;
    }

    /// <summary>
    /// Tenjin根据类型触发事件
    /// </summary>
    /// <param name="msg"></param>
    public virtual void SendTenjinEvent(string msg)
    {
        Debug.Log("开始执行Tenjin事件: " + msg);
    }

    /// <summary>
    /// 应用内安装
    /// </summary>
    /// <param name="filePath"></param>
    public virtual void InstallAPK(string filePath) { }
}