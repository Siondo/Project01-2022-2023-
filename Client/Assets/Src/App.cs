using UnityEngine;
using System;
using System.Collections.Generic;
using Framework;
using Framework.IO;

[Serializable]
public class AppInfo
{
    [SerializeField]
    private string m_productName = string.Empty;

    [SerializeField]
    private string m_assetVersion = string.Empty;

    [SerializeField]
    private string m_scriptingDefineSymbols = string.Empty;

    [SerializeField]
    private string m_url = string.Empty;

    [SerializeField]
    private bool m_openGuide = false;

    [SerializeField]
    private bool m_openUpdate = false;

    [SerializeField]
    private Debugger.LogLevel m_logLevel = Debugger.LogLevel.None;

    [SerializeField]
    private bool m_abMode = false;

    [SerializeField]
    private bool m_abLua = false;

    [SerializeField]
    private bool m_debugMode = false;

    [SerializeField]
    private bool m_checkMode = false;

    public string productName
    {
        get { return m_productName; }
        set { m_productName = value; }
    }

    public string assetVersion
    {
        get { return m_assetVersion; }
        set { m_assetVersion = value; }
    }

    public string scriptingDefineSymbols
    {
        get { return m_scriptingDefineSymbols; }
        set { m_scriptingDefineSymbols = value; }
    }

    public string url
    {
        get { return m_url; }
        set { m_url = value; }
    }

    public bool openGuide
    {
        get { return m_openGuide; }
        set { m_openGuide = value; }
    }

    public bool openUpdate
    {
        get { return m_openUpdate; }
        set { m_openUpdate = value; }
    }

    public Debugger.LogLevel logLevel
    {
        get { return m_logLevel; }
        set { m_logLevel = value; }
    }

    public bool abMode
    {
        get { return m_abMode; }
        set { m_abMode = value; }
    }

    public bool abLua
    {
        get { return m_abLua; }
        set { m_abLua = value; }
    }

    public bool debugMode
    {
        get { return m_debugMode; }
        set { m_debugMode = value; }
    }

    public bool checkMode
    {
        get { return m_checkMode; }
        set { m_checkMode = value; }
    }
}

/// <summary>
/// App
/// </summary>
public sealed class App
{
    #region Variable
    /// <summary>
    /// 版本信息
    /// </summary>
    private static AppInfo m_info = null;

    /// <summary>
    /// 资源清单
    /// </summary>
    private static ManifestConfig m_manifest = new ManifestConfig();

    /// <summary>
    /// 资源清单映射表
    /// </summary>
    private static ManifestMappingConfig m_manifestMapping = new ManifestMappingConfig(true);

    /// <summary>
    /// 远程资源目录
    /// </summary>
    private static string m_assetDir = string.Empty;

    /// <summary>
    /// 使用地址
    /// </summary>
    private static string m_address = string.Empty;

    /// <summary>
    /// 新版本下载地址
    /// </summary>
    private static string m_newVersionDownloadUrl = string.Empty;

    /// <summary>
    /// 新版本内容
    /// </summary>
    private static string m_newVersionContent = string.Empty;

    /// <summary>
    /// Token登录唯一
    /// </summary>
    private static string m_token = string.Empty;
    #endregion

    #region Property
    /// <summary>
    /// 产品名
    /// </summary>
    public static string productName
    {
        get { return Application.productName; }
    }

    /// <summary>
    /// 游戏内部版本
    /// </summary>
    public static string innerVersion
    {
        get { return version; }
    }

    /// <summary>
    /// 游戏App版本
    /// </summary>
    public static string version
    {
        get { return Application.version; }
    }

    /// <summary>
    /// 资源版本
    /// </summary>
    public static string assetVersion
    {
        get { return m_info.assetVersion; }
        set { m_info.assetVersion = value; }
    }

    /// <summary>
    /// 中心Url
    /// </summary>
    /// <value>URL.</value>
    public static string url
    {
        get { return m_info.url; }
        set { m_info.url = value; }
    }

    /// <summary>
    /// 是否开启引导
    /// </summary>
    public static bool openGuide
    {
        get { return m_info.openGuide; }
    }

    /// <summary>
    /// 是否开启更新功能
    /// </summary>
    public static bool openUpdate
    {
        get { return m_info.openUpdate; }
    }

    /// <summary>
    /// 是否开启日志
    /// </summary>
    public static bool logEnabled
    {
        get { return Debugger.LogLevel.None == m_info.logLevel; }
    }

    /// <summary>
    /// 日志等级
    /// </summary>
    public static Debugger.LogLevel logLevel
    {
        get { return m_info.logLevel; }
    }

    /// <summary>
    /// ABMode?
    /// </summary>
    public static bool abMode
    {
        get { return m_info.abMode; }
    }

    /// <summary>
    /// ABLua?
    /// </summary>
    public static bool abLua
    {
        get { return m_info.abLua; }
    }

    /// <summary>
    /// DebugMode?
    /// </summary>
    public static bool debugMode
    {
        get { return m_info.debugMode; }
    }

    /// <summary>
    /// CheckMode?
    /// </summary>
    public static bool checkMode
    {
        get { return m_info.checkMode; }
    }

    /// <summary>
    /// 平台标签
    /// </summary>
    public static string platformTag
    {
        get
        {
            return Util.GetPlatform();
        }
    }

    /// <summary>
    /// 得到宏定义
    /// </summary>
    public static string scriptingDefineSymbols
    {
        get { return m_info.scriptingDefineSymbols; }
    }

    /// <summary>
    /// 资源清单文件
    /// </summary>
    public static ManifestConfig manifest
    {
        get { return m_manifest; }
        set { m_manifest = value; }
    }

    /// <summary>
    /// 资源清单映射表
    /// </summary>
    public static ManifestMappingConfig manifestMapping
    {
        get { return m_manifestMapping; }
        set { m_manifestMapping = value; }
    }

    /// <summary>
    /// 远程资源目录
    /// </summary>
    public static string assetDir
    {
        get { return m_assetDir; }
    }

    /// <summary>
    /// 使用地址
    /// </summary>
    public static string address
    {
        get { return m_address; }
    }

    /// <summary>
    /// 新版本下载地址
    /// </summary>
    public static string newVersionDownloadUrl
    {
        get { return m_newVersionDownloadUrl; }
    }

    /// <summary>
    /// 新版本升级内容
    /// </summary>
    public static string newVersionContent
    {
        get { return m_newVersionContent; }
    }

    /// <summary>
    /// 得到当前平台
    /// </summary>
    public static string platform
    {
        get
        {
            return Application.platform.ToString();
        }
    }

    /// <summary>
    /// 得到下一步状态
    /// </summary>
    public static string nextState
    {
        get; set;
    }

    /// <summary>
    /// 是否是流量网络
    /// </summary>
    public static bool dataNetwork => Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork;

    /// <summary>
    /// 设备Token，如果有微信登录使用微信Token
    /// </summary>
    public static string token
    {
        get
        {
            if (string.IsNullOrWhiteSpace(m_token))
            {
                m_token = SystemInfo.deviceUniqueIdentifier;
            }
            return m_token;
        }
        set
        {
            m_token = value;
        }
    }

    /// <summary>
    /// 设备唯一ID
    /// </summary>
    public static string deviceId
    {
        get
        {
            return Util.GetDeviceUniqueIdentifier();
        }
    }

    /// <summary>
    /// 设备产品品牌
    /// </summary>
    public static string deviceBrand
    {
        get
        {
            return SystemInfo.deviceModel;
        }
    }

    /// <summary>
    /// 连接服务器host
    /// </summary>
    public static string host
    {
        get;set;
    }

    /// <summary>
    /// 连接服务器端口
    /// </summary>
    public static int port
    {
        get; set;
    }

    #endregion

    #region Function
    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init(AppInfo data)
    {
#if !UNITY_EDITOR
        data.abMode = true;
        data.abLua = true;
#endif
        m_info = data;
        m_address = url;
    }

    /// <summary>
    /// 更新使用
    /// </summary>
    /// <param name="jsonText"></param>
    public static void Update(string jsonText)
    {
        Dictionary<string, object> data = Framework.JsonFx.JsonReader.Deserialize<Dictionary<string, object>>(jsonText);
        //远程更新资源目录
        if (data.ContainsKey("assetDir"))
        {
            m_assetDir = data["assetDir"].ToString();
        }
        //版本升级内容
        if (data.ContainsKey("upgradeContent"))
        {
            m_newVersionContent = data["upgradeContent"].ToString();
        }
        //新版本下载地址
        if (data.ContainsKey("newVersionDownloadUrl"))
        {
            m_newVersionDownloadUrl = data["newVersionDownloadUrl"].ToString();
        }
        //服务器地址
        if (data.ContainsKey("address"))
        {
            //m_address = data["address"].ToString();
        }

        //Debugger.logEnabled = logEnabled;
        //Debugger.logLevel = logLevel;
    }
    #endregion
}
