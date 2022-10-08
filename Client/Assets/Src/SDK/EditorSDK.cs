using UnityEngine;
using System.Collections;
using XLua;
using System;

[ReflectionUse, Hotfix]
public sealed class EditorSDK : SDKBase
{
    /// <summary>
    /// 登录回调
    /// </summary>
    private System.Action<LoginCode> m_loginCallback = null;

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="loginType"></param>
    /// <param name="callback"></param>
    public override void OnLogin(SDKManager.LoginType loginType, System.Action<LoginCode> callback)
    {
        m_loginCallback = callback;
        m_loginCallback.Invoke(LoginCode.SUCCESSFUL);
    }

    /// <summary>
    /// 自动登陆
    /// </summary>
    /// <param name="loginType"></param>
    /// <param name="callback"></param>
    public override void OnAutoLogin(SDKManager.LoginType loginType, System.Action<SDKBase.LoginCode> callback)
    {
        callback?.Invoke(SDKBase.LoginCode.SUCCESSFUL);
    }

    /// <summary>
    /// 开屏广告
    /// </summary>
    public override void SplashAdShow(Action<string> callback)
    {
        base.SplashAdShow(callback);
        SimpleJson.JsonObject json = new SimpleJson.JsonObject();
        json.Add("status", true);
        json.Add("adInfo", "{}");
        SplashAdShowFinished(SimpleJson.SimpleJson.SerializeObject(json));
    }

    /// <summary>
    /// 开屏广告结束
    /// </summary>
    /// <param name="msg"></param>
    public override void SplashAdShowFinished(string msg)
    {
        base.SplashAdShowFinished(msg);
    }

    /// <summary>
    /// 展示激励广告
    /// </summary>
    public override void RewardVideoAdShow(string scenario, Action<string> callback)
    {
        base.RewardVideoAdShow(scenario, callback);
        SimpleJson.JsonObject json = new SimpleJson.JsonObject();
        json.Add("status", true);
        json.Add("adInfo", "{}");
        RewardVideoAdShowFinished(SimpleJson.SimpleJson.SerializeObject(json));
    }

    /// <summary>
    /// 激励广告结束
    /// </summary>
    public override void RewardVideoAdShowFinished(string msg)
    {
        base.RewardVideoAdShowFinished(msg);
    }

    /// <summary>
    /// 展示插屏广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public override void InterstitialAdShow(string scenario, Action<string> callback)
    {
        base.InterstitialAdShow(scenario, callback);
        SimpleJson.JsonObject json = new SimpleJson.JsonObject();
        json.Add("status", true);
        json.Add("adInfo", "{}");
        InterstitialAdShowFinished(SimpleJson.SimpleJson.SerializeObject(json));
    }

    /// <summary>
    /// 插屏广告结束
    /// </summary>
    public override void InterstitialAdShowFinished(string msg)
    {
        base.InterstitialAdShowFinished(msg);
    }

    /// <summary>
    /// 展示原生广告
    /// </summary>
    public override void NativeAdShow(Action<string> callback)
    {
        base.NativeAdShow(callback);
        SimpleJson.JsonObject json = new SimpleJson.JsonObject();
        json.Add("status", true);
        json.Add("adInfo", "{}");
        NativeAdShowFinished(SimpleJson.SimpleJson.SerializeObject(json));
    }

    /// <summary>
    /// 展示横幅广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public override void BannerAdShow(string scenario, Action<string> callback)
    {
        base.BannerAdShow(scenario, callback);
        SimpleJson.JsonObject json = new SimpleJson.JsonObject();
        json.Add("status", true);
        json.Add("adInfo", "{}");
        BannerAdShowFinished(SimpleJson.SimpleJson.SerializeObject(json));
    }
}