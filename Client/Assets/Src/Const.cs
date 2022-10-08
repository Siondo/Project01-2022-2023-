public class Const
{
    #region 启动信息
    public const string NAME = "name";                                          //定义配置组名字
    public const string PRODUCT_NAME = "productName";                           //产品的名字
    public const string BUNDLE_IDENTIFIER = "bundleIdentifier";                 //包名
    public const string VERSION = "version";                                    //版本
    public const string INNERVERSION = "innerVersion";                          //内部版本
    public const string ASSETVERSION = "assetVersion";                          //资源版本
    public const string BUNDLE_VERSION_CODE = "bundleVersionCode";              //Code版本
    public const string SCRIPTING_DEFINE_SYMBOLS = "scriptingDefineSymbols";    //宏定义
    public const string URL = "Url";                                            //中心Url
    public const string OPEN_GUIDE = "openGuide";                               //是否开启引导
    public const string OPEN_UPDATE = "openUpdate";                             //是否开启更新
    public const string LOGLEVEL = "logLevel";                                  //日志等级
    public const string AB_MODE = "abMode";                                     //是否是AB模式
    public const string AB_LUA = "abLua";                                       //是否使用ABLua
    public const string ANDROID_PLATFORM_NAME = "androidPlatformName";          //Android平台名
    public const string IOS_PLATFORM_NAME = "iOSPlatformName";                  //iOS平台名
    public const string DEFAULT_PLATFORM_NAME = "defaultPlatformName";          //默认平台名
    public const string NEWVERSIONDOWNLOADURL = "newVersionDownloadUrl";        //新版本下载地址
    #endregion

    #region
    public const string OPENUI = "openUI";                                      //打开UI
    public const string CLOSEUI = "closeUI";                                    //关闭UI
    public const string ONENABLE = "onEnable";                                  //激活
    public const string START = "start";                                        //开始
    public const string FIXEDUPDATE = "fixedUpdate";                            //固定更新
    public const string UPDATE = "update";                                      //更新
    public const string LATEUPDATE = "lateUpdate";                              //延迟更新
    public const string ONDISABLE = "onDisable";                                //失活
    public const string ONDESTROY = "onDestroy";                                //销毁
    #endregion

    #region 资源加载(短路径补充)
    public const string LOADUI = "res/ui/prefab/";                              //UI资源加载
    public const string LOADLUA = "res/lua/";                                   //LUA资源加载
    public const string LOADCONF = "res/conf/";                                 //Config资源加载
    #endregion

    public const int MAX_LOADER = 2;                                            //最大同时加载资源数
    public const string ASSETBUNDLEVARIANT = "unity3d";                         //捆绑资源的后缀名

    #region UI界面
    public const string UI_ROOT = "UIRoot";                                     //UIRoot
    public const string UI_MAIN = "UIMain";                                     //大厅界面
    #endregion

    public const string REMOTEVERSION = "version_V{0}.json";                    //远程版本文件
    public const string SANDBOX_VERSION = "SandboxVersion";                     //沙盒版本
    public const string REMOTE_DIRECTORY = "v{0}";                              //远程目录

    public const int UI_START_SORTINGORDER = 2000;                              //UI窗口起点层级
    public const int UI_INTERVAL_SORTINGORDER = 200;                            //UI窗口间隔

    public const int Pool_GC_TIME = 60;                                         //资源池回收时间
}