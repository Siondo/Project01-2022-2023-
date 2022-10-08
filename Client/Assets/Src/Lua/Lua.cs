using XLua;
using System.Collections.Generic;
using Framework.Event;
using Framework.Singleton;
using Framework.UnityAsset;
using Framework;
using UnityEngine;

public class Lua : MonoBehaviourSingleton<Lua>
{
    #region Variable
    /// <summary>
    /// Lua虚拟机
    /// </summary>
    private LuaEnv m_luaEnv = null;

    /// <summary>
    /// Lua表
    /// </summary>
    private LuaTable m_table = null;

    /// <summary>
    /// 开始
    /// </summary>
    private LuaFunction m_start = null;

    /// <summary>
    /// 更新
    /// </summary>
    private LuaFunction m_update = null;

    /// <summary>
    /// 延迟更新
    /// </summary>
    private LuaFunction m_lateUpdate = null;

    /// <summary>
    /// 物理更新
    /// </summary>
    private LuaFunction m_fixedUpdate = null;

    /// <summary>
    /// 是否暂停
    /// </summary>
    private LuaFunction m_pause = null;

    /// <summary>
    /// 销毁
    /// </summary>
    private LuaFunction m_destroy = null;

    /// <summary>
    /// 得到脚本方法
    /// </summary>
    private LuaFunction m_script = null;

    /// <summary>
    /// 得到多语言
    /// </summary>
    private LuaFunction m_language = null;

    /// <summary>
    /// 音效播放
    /// </summary>
    private LuaFunction m_sound = null;

	public LuaTable m_matchLuaTable = null;

    /// <summary>
    /// 初始化是否完成
    /// </summary>
    private bool m_initFinish = false;


    /// <summary>
    /// 上一次标记时间
    /// </summary>
    private float m_lastTickTime = 0f;
    #endregion

    /// <summary>
    /// Lua全局表
    /// </summary>
    public LuaTable Global => m_luaEnv.Global;

    /// <summary>
    /// 初始化是否完成
    /// </summary>
    public bool initFinish => m_initFinish;

    #region Function
    /// <summary>
    /// 开始
    /// </summary>
    private void Awake()
    {
        m_luaEnv = new LuaEnv();
#if UNITY_EDITOR
        m_luaEnv.Global.SetInPath<bool>("EDITOR", true);
#endif
        if (!string.IsNullOrEmpty(App.scriptingDefineSymbols))
        {
            string[] symbols = App.scriptingDefineSymbols.Split(';');
            for (int i = 0; i < symbols.Length; ++i)
            {
                m_luaEnv.Global.SetInPath<bool>(symbols[i], true);
#if UNITY_EDITOR
                Debug.Log("Add Symbols: " + symbols[i]);
#endif
            }
        }
        //m_luaEnv.AddBuildin("protobuf.c", XLua.LuaDLL.Lua.LoadProtobufC);
        m_luaEnv.AddLoader(Loader);
        m_table = m_luaEnv.NewTable();
        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = m_luaEnv.NewTable();
        meta.Set("__index", m_luaEnv.Global);
        m_table.SetMetaTable(meta);
        meta.Dispose();

        m_table.Set("self", this);
        m_luaEnv.DoString("require('Main')", "XLua", m_table);

        m_start = m_table.GetInPath<LuaFunction>("Start");
        m_update = m_table.GetInPath<LuaFunction>("Update");
        m_lateUpdate = m_table.GetInPath<LuaFunction>("LateUpdate");
        m_fixedUpdate = m_table.GetInPath<LuaFunction>("FixedUpdate");
        m_pause = m_table.GetInPath<LuaFunction>("OnApplicationPause");
        m_destroy = m_table.GetInPath<LuaFunction>("Destroy");
        m_script = m_table.GetInPath<LuaFunction>("GetScript");
        m_language = m_table.GetInPath<LuaFunction>("GetLanguage");
        m_sound = m_table.GetInPath<LuaFunction>("PlaySound");
        //m_matchLuaTable = GetScript("MatchGame.MatchItem");
    }

    /// <summary>
    /// 准备开始
    /// </summary>
    public void OnStart() { }

    /// <summary>
    /// 停止并销毁
    /// </summary>
    public void OnStopAndDestroy() {
        DestroyImmediate(gameObject);
    }

    /// <summary>
    /// 开始
    /// </summary>
    private void Start()
    {
        m_start.Call();
        m_initFinish = true;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        m_update.Call(Time.realtimeSinceStartup, Time.frameCount, Time.deltaTime, Time.unscaledDeltaTime);

        if (Time.realtimeSinceStartup - m_lastTickTime > 1)
        {
            m_luaEnv.Tick();
            m_lastTickTime = Time.realtimeSinceStartup;
        }
    }

    /// <summary>
    /// 延迟更新
    /// </summary>
    private void LateUpdate()
    {
        m_lateUpdate.Call();
    }

    /// <summary>
    /// 物理更新
    /// </summary>
    private void FixedUpdate()
    {
        m_fixedUpdate.Call();
    }

    /// <summary>
    /// 应用暂停
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        m_pause?.Call(pause);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    private void OnDestroy()
    {
        m_destroy?.Call();
        m_start = null;
        m_update = null;
        m_lateUpdate = null;
        m_fixedUpdate = null;
        m_pause = null;
        m_destroy = null;
        m_script = null;
        m_language = null;
        m_luaEnv = null;
    }

    /// <summary>
    /// Lua加载器
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private byte[] Loader(ref string fileName)
    {
#if UNITY_EDITOR_WIN
        if (fileName.Contains("emmy_core"))
        {
            return null;
        }
#endif

        byte[] result = null;

        fileName = fileName.Replace(".", "/");
        AssetManager.instance.LoadLua(fileName, (bResult, asyncAsset) =>
        {
            if (bResult)
            {
                if (App.abLua)
                {
                    result = Util.SimpleDecrypt(asyncAsset.bytes);
                }
                else
                {
                    result = asyncAsset.bytes;
                }
            }
            AssetManager.instance.UnloadAsset(asyncAsset, true);
        }, async: false);

        return result;
    }

    /// <summary>
    /// 创建一个新的Lua表
    /// </summary>
    /// <returns></returns>
    public LuaTable NewTable()
    {
        return m_luaEnv.NewTable();
    }

    /// <summary>
    /// 执行脚本
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public LuaTable GetScript(string path)
    {
        path = path.Replace("/", ".");
        return m_script.Call(path)[0] as LuaTable;
    }

    /// <summary>
    /// 得到多语言
    /// </summary>
    /// <param name="languageId"></param>
    /// <returns></returns>
    public string GetLanguage(string languageId)
    {
        return m_language.Call(languageId)[0] as string;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            m_sound.Call(name);
        }
    }
#endregion
}
