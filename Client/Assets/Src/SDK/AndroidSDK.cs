using UnityEngine;
using System.Collections;
using XLua;
using LitJson;
using SimpleJson;
using System;

[ReflectionUse, Hotfix]
public sealed class AndroidSDK : SDKBase
{
    /// <summary>
    /// Java类
    /// </summary>
    private AndroidJavaClass m_javaClass = null;

    /// <summary>
    /// Java类对象
    /// </summary>
    private AndroidJavaObject m_javaObject = null;

    /// <summary>
    /// 登录回调
    /// </summary>
    private System.Action<LoginCode> m_loginCallback = null;

    private string appId
    {
        get;set;
    }

    private string appSecret
    {
        get;set;
    }

    private string code
    {
        get; set;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init()
    {
        m_javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        m_javaObject = m_javaClass.GetStatic<AndroidJavaObject>("currentActivity");
    }

    /// <summary>
    /// 权限请求
    /// </summary>
    /// <param name="permission"></param>
    public override void RequestPermission(String permission) 
    {
        m_javaObject.Call("RequestPermission", permission);
    }

    /// <summary>
    /// 友盟数据埋点
    /// </summary>
    /// <param name="eventkey"></param>
    /// <param name="eventValue"></param>
    public override void OnEventObject(string eventkey, string eventValue)
    {
        m_javaObject.Call("MobOnEventObj", eventkey);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="loginType"></param>
    /// <param name="callback"></param>
    public override void OnLogin(SDKManager.LoginType loginType, System.Action<SDKBase.LoginCode> callback)
    {
        m_loginCallback = callback;
        m_javaObject.Call("OnLogin", loginType.ToString());
    }

    /// <summary>
    /// 自动登录
    /// </summary>
    /// <param name="loginType"></param>
    /// <param name="callback"></param>
    public override void OnAutoLogin(SDKManager.LoginType loginType, System.Action<SDKBase.LoginCode> callback)
    {
        //string openId = PlayerPrefs.GetString(Util.GetMd5("openId"), "");
        //string accessToken = PlayerPrefs.GetString(Util.GetMd5("accessToken"), "");
        //if (string.IsNullOrWhiteSpace(openId) || string.IsNullOrWhiteSpace(accessToken))
        //{
        //    callback?.Invoke(SDKBase.LoginCode.FAILED);
        //    return;
        //}

        //m_loginCallback = callback;
        //string url = string.Format("https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}", accessToken, openId);
        ////检查accessToken是否还有效
        //Util.WWWGetText(url, (checkTokenErr, checkTokenObj) => {
        //    if (string.IsNullOrWhiteSpace(checkTokenErr))
        //    {
        //        Util.Log("OnAutoLogin 检查accessToken是否还有效: " + checkTokenObj);
        //        JsonObject json = Util.GetJsonObject(checkTokenObj as string);
        //        int errCode = Util.GetInt(json["errcode"]);
        //        if (0 == errCode)
        //        {
        //            url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", accessToken, openId);
        //            //得到玩家信息
        //            Util.WWWGetText(url, (getInfoErr, getInfoObj) =>
        //            {
        //                Util.Log("OnAutoLogin 得到玩家信息: " + getInfoObj);
        //                if (string.IsNullOrWhiteSpace(getInfoErr))
        //                {
        //                    json = Util.GetJsonObject(getInfoObj as string);
        //                    string nickname = Util.GetString(json["nickname"]);
        //                    string sex = Util.GetString(json["sex"]);
        //                    string province = Util.GetString(json["province"]);
        //                    string city = Util.GetString(json["city"]);
        //                    string country = Util.GetString(json["country"]);
        //                    string headimgurl = Util.GetString(json["headimgurl"]);

        //                    PlayerPrefs.SetString(Util.GetMd5("nickname"), nickname);
        //                    PlayerPrefs.SetString(Util.GetMd5("headimgurl"), headimgurl);
        //                    m_loginCallback?.Invoke(LoginCode.SUCCESSFUL);
        //                }
        //                else
        //                {
        //                    m_loginCallback?.Invoke(LoginCode.FAILED);
        //                }
        //            });
        //        }
        //        else
        //        {
        //            m_loginCallback?.Invoke(LoginCode.FAILED);
        //        }
        //    }
        //    else
        //    {
        //        m_loginCallback?.Invoke(LoginCode.FAILED);
        //    }
        //});
    }

    public override void OnLoginBack(string msg)
    {
        base.OnLoginBack(msg);
        //Util.Log("OnLoginBack: " + msg);

        //if (string.IsNullOrWhiteSpace(msg))
        //{
        //    m_loginCallback?.Invoke(LoginCode.FAILED);
        //}
        //else
        //{
        //    JsonObject json = Util.GetJsonObject(msg);

        //    string openId = Util.GetString(json["openId"]);
        //    string accessToken = Util.GetString(json["accessToken"]);
        //    string nickname = Util.GetString(json["nickname"]);
        //    string headimgurl = Util.GetString(json["headimgurl"]);

        //    PlayerPrefs.SetString(Util.GetMd5("openId"), openId);
        //    PlayerPrefs.SetString(Util.GetMd5("accessToken"), accessToken);
        //    PlayerPrefs.SetString(Util.GetMd5("nickname"), nickname);
        //    PlayerPrefs.SetString(Util.GetMd5("headimgurl"), headimgurl);
        //    m_loginCallback?.Invoke(LoginCode.SUCCESSFUL);
        //}
    }

    /// <summary>
    /// 登录成功
    /// </summary>
    public override void OnLoginSuccessed()
    {
        base.OnLoginSuccessed();
        //string openId = PlayerPrefs.GetString(Util.GetMd5("openId"), "");

        //JsonObject json = new JsonObject();
        //json.Add("provider", "wx");
        //json.Add("ID", openId);
        //m_javaObject.Call("OnLoginSuccess", SimpleJson.SimpleJson.SerializeObject(json));
    }

    /// <summary>
    /// 展示激励广告
    /// </summary>
    public override void RewardVideoAdShow(string scenario, Action<string> callback)
    {
        base.RewardVideoAdShow(scenario, callback);
#if UNITY_ANDROID && !UNITY_EDITOR
        m_javaObject.Call("RewardVideoAdShow", scenario);
#endif
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
#if UNITY_ANDROID && !UNITY_EDITOR
        m_javaObject.Call("InterstitialAdShow", scenario);
#endif
    }

    /// <summary>
    /// 插屏广告结束
    /// </summary>
    public override void InterstitialAdShowFinished(string msg)
    {
        base.InterstitialAdShowFinished(msg);
    }

    /// <summary>
    /// 展示横幅广告
    /// </summary>
    /// <param name="scenario"></param>
    /// <param name="callback"></param>
    public override void BannerAdShow(string scenario, Action<string> callback)
    {
        base.BannerAdShow(scenario, callback);
#if UNITY_ANDROID && !UNITY_EDITOR
        m_javaObject.Call("BannerAdShow", scenario);
#endif
    }

    /// <summary>
    /// 横幅广告结束
    /// </summary>
    public override void BannerAdShowFinished(string msg)
    {
        base.BannerAdShowFinished(msg);
    }

    /// <summary>
    /// 隐藏横幅广告
    /// </summary>
    public override void BannerAdHide()
    {
        base.BannerAdHide();
#if UNITY_ANDROID && !UNITY_EDITOR
        m_javaObject.Call("BannerAdHide");
#endif
    }

    /// <summary>
    /// 开屏广告
    /// </summary>
    public override void SplashAdShow(Action<string> callback)
    {
        base.SplashAdShow(callback);
#if UNITY_ANDROID && !UNITY_EDITOR
        m_javaObject.Call("SplashAdShow");
#endif
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
    /// 原生广告
    /// </summary>
    public override void NativeAdShow(Action<string> callback)
    {
        base.NativeAdShow(callback);
#if UNITY_ANDROID && !UNITY_EDITOR
        m_javaObject.Call("NativeAdShow");
#endif
    }

    /// <summary>
    /// 原生广告结束
    /// </summary>
    /// <param name="msg"></param>
    public override void NativeAdShowFinished(string msg)
    {
        base.NativeAdShowFinished(msg);
    }

    /// <summary>
    /// Tenjin根据类型触发事件
    /// </summary>
    /// <param name="type"></param>
    public override void SendTenjinEvent(string msg)
    {
        base.SendTenjinEvent(msg);
        m_javaObject.Call("onTouchTenjinEvent", msg);
    }
    /// <summary>
    /// 应用内安装
    /// </summary>
    /// <param name="filePath"></param>
    public override void InstallAPK(string filePath) 
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        m_javaObject.Call("InstallAPK", filePath);
#endif
    }
}