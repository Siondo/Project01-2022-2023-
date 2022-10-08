using UnityEngine;
using Framework;
using Framework.Event;
using Framework.Pool;
using Framework.UnityAsset;

public class Launch : MonoBehaviour
{
    /// <summary>
    /// 启动信息
    /// </summary>
    [SerializeField]
    private AppInfo m_data = new AppInfo();
    private GUIStyle fontStyle = new GUIStyle();

    /// <summary>
    /// 启动信息
    /// </summary>
    public AppInfo data => m_data;

    /// <summary>
    /// 启动
    /// </summary>
    void Awake()
    {
        App.Init(data);
        // 配置参数设置
        Debugger.Start(App.logLevel);
        AssetManager.instance.maxLoader = Const.MAX_LOADER;
        // 启动定时器
        Schedule.instance.Start();
        // 准备对象池
        PoolManager.instance.Create();
        //切换场景不销毁
        DontDestroyOnLoad(gameObject);
        //实例化SDK
        SDKManager.InstanceSDK();
    }

    /// <summary>
    /// 开始
    /// </summary>
    void Start()
    {
        Lua.instance.OnStart();
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        AssetManager.instance.Update();
        Schedule.instance.Update(Time.deltaTime);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    private void OnDestroy()
    {
        
    }

    private void OnGUI()
    {
#if DEBUG_GMODE
        fontStyle.fontSize = 36;
        if (Reporter.Instance.fps > 45) fontStyle.normal.textColor = Color.green;
        else fontStyle.normal.textColor = Color.red;
        if (GUILayout.Button("ReportLog", GUILayout.Width(120), GUILayout.Height(50)))
            LuaHelper.ShowReporter();
        GUILayout.Label("FPS:" + (int)Reporter.Instance.fps, fontStyle);
#endif
    }
}