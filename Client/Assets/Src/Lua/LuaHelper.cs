using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;
using Framework;
using Framework.UnityAsset;
using Framework.JsonFx;
using Framework.IO;
using Framework.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Framework.Pool;
using System.Collections.Generic;
using System.Threading;

public partial class LuaHelper
{
    /// <summary>
    /// 是否是编辑器
    /// </summary>
    public static bool isEditor
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }

    /// <summary>
    /// 得到持久化路径
    /// </summary>
    public static string persistentDataPath => PathUtil.persistentDataPath;

    /// <summary>
    /// 目标帧率
    /// </summary>
    public static int targetFrameRate
    {
        get
        {
            return Application.targetFrameRate;
        }
        set
        {
            Application.targetFrameRate = value;
        }
    }

    /// <summary>
    /// 显示报告
    /// </summary>
    public static void ShowReporter()
    {
        Reporter.Instance.show = false;
        Reporter.Instance.doShow();
    }

    /// <summary>
    /// 获取发布平台
    /// </summary>
    public static string GetAppPlatform()
    {
#if TC
       return "tc";
#elif EN
       return "en";
#else
       return "en";
#endif
    }

    public static bool GetDebugModeStatus()
    {
#if DEBUG_GMODE
        return true;
#else
        return false;
#endif
    }

    /// <summary>
    /// 得到持久化字符串数据
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetString(string key, string defaultValue)
    {
        return Util.GetString(key, defaultValue);
    }

    /// <summary>
    /// 得到持久化整型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int GetInt(string key, int defaultValue)
    {
        return Util.GetInt(key, defaultValue);
    }

    /// <summary>
    /// 得到持久化浮点数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static float GetFloat(string key, float defaultValue)
    {
        return Util.GetFloat(key, defaultValue);
    }

    /// <summary>
    /// 得到持久化布尔数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool GetBool(string key, bool defaultValue)
    {
        return Util.GetString(key, defaultValue.ToString()).Equals(bool.TrueString);
    }

    /// <summary>
    /// 设置持久化字符串数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetString(string key, string value)
    {
        Util.SetString(key, value);
    }

    /// <summary>
    /// 设置持久化整型数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetInt(string key, int value)
    {
        Util.SetInt(key, value);
    }

    /// <summary>
    /// 设置持久化浮点数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetFloat(string key, float value)
    {
        Util.SetFloat(key, value);
    }

    /// <summary>
    /// 设置持久化布尔数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SetBool(string key, bool value)
    {
        Util.SetString(key, value.ToString());
    }

    /// <summary>
    /// 删除一条持久化数据
    /// </summary>
    /// <param name="key"></param>
    public static void DeleteKey(string key)
    {
        Util.DeleteKey(key);
    }

    /// <summary>
    /// 删除所有持久化数据
    /// </summary>
    public static void DeleteAll()
    {
        Util.DeleteAll();
    }

    /// <summary>
    /// 保存持久化数据
    /// </summary>
    public static void Save()
    {
        Util.Save();
    }

    /// <summary>
    /// 删除文件或目录
    /// </summary>
    /// <param name="path"></param>
    public static void DeleteFileOrDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        else if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// 得到不带扩展的文件名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetFileNameWithoutExtension(string path)
    {
        return PathUtil.GetFileNameWithoutExtension(path);
    }

    /// <summary>
    /// 更新Manifest
    /// </summary>
    /// <param name="text"></param>
    public static void UpdateManifestConfig(string text)
    {
        App.manifest = JsonReader.Deserialize<ManifestConfig>(text);
        App.assetVersion = App.manifest.assetVersion;
    }

    /// <summary>
    /// 字符串转Manifest
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static ManifestConfig StringToManifestConfig(string text)
    {
        return JsonReader.Deserialize<ManifestConfig>(text);
    }

    /// <summary>
    /// 更新Manifest映射表
    /// </summary>
    /// <param name="text"></param>
    public static void UpdateManifestMappingConfig(string text)
    {
        App.manifestMapping = JsonReader.Deserialize<ManifestMappingConfig>(text);
    }

    /// <summary>
    /// 得到文件MD5
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetMD5(string path)
    {
        return Util.GetMD5(path);
    }

    /// <summary>
    /// 路径拼接
    /// </summary>
    /// <param name="path1"></param>
    /// <param name="path2"></param>
    /// <returns></returns>
    public static string Combine(string path1, string path2)
    {
        return PathUtil.Combine(path1, path2);
    }

    /// <summary>
    /// 写入字节到文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="bytes"></param>
    public static void WriteAllBytes(string path, byte[] bytes)
    {
        Util.WriteAllBytes(path, bytes);
    }

    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loadFinish"></param>
    public static void ShowUIImmediate(string name, params object[] args)
    {
        UIManager.instance.ShowUIImmediate(name, args);
    }

    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="name"></param>
    /// <param name="args"></param>
    public static void ShowUI(string name, params object[] args)
    {
        UIManager.instance.ShowUI(name, args);
    }

    /// <summary>
    /// 隐藏UI
    /// </summary>
    /// <param name="name"></param>
    public static void HideUI(string name)
    {
        UIManager.instance.HideUI(name);
    }

    /// <summary>
    /// 得到指定UI的数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static UIBase GetUIBase(string name)
    {
        return UIManager.instance.GetUIBase(name);
    }

    /// <summary>
    /// 是否显示UI
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool IsShowUI(string name)
    {
        return UIManager.instance.IsShow(name);
    }

    /// <summary>
    /// 隐藏所有UI并销毁
    /// </summary>
    public static void ClearUI(bool isDestory)
    {
        UIManager.instance.HideAllUI(isDestory);
    }

    /// <summary>
    /// 移除一个Window窗口
    /// </summary>
    /// <param name="name"></param>
    public static void RemoveWindowStackData(string name)
    {
        UIManager.instance.RemoveWindowStackData(name);
    }

    /// <summary>
    /// 隐藏Logo页面
    /// </summary>
    public static void HideDisplayLogo()
    {
        UIManager.instance.m_DisplayLogo.SetActive(false);
    }

    /// <summary>
    /// 刷新所有文本
    /// </summary>
    /// <param name="callback"></param>
    public static void RefreshAllText(System.Action callback = null)
    {
        UIManager.instance.RefreshAllText(callback);
    }

    /// <summary>
    /// 退出
    /// </summary>
    public static void Quit()
    {
        Application.Quit();
    }


    public static void ReStartGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

        //记录主线程
        SynchronizationContext sc = SynchronizationContext.Current;
        var appThread = new Thread(() =>
        {
            Thread.Sleep(500);

            //传递到主线程调用isPlaying
            sc.Post((object o) =>
            {
                UnityEditor.EditorApplication.isPlaying = true;
            }, null);
        });
        appThread.Start();


#elif UNITY_ANDROID
        RestartAndroid();
#elif UNITY_IPHONE
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.Abort);
#else
        throw new System.NotImplementedException();
#endif
    }

    private static void RestartAndroid()
    {
        if (Application.isEditor)
            return;

        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            const int kIntent_FLAG_ACTIVITY_CLEAR_TASK = 0x00008000;
            const int kIntent_FLAG_ACTIVITY_NEW_TASK = 0x10000000;

            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var pm = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            var intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);

            intent.Call<AndroidJavaObject>("setFlags", kIntent_FLAG_ACTIVITY_NEW_TASK | kIntent_FLAG_ACTIVITY_CLEAR_TASK);
            currentActivity.Call("startActivity", intent);
            currentActivity.Call("finish");
            var process = new AndroidJavaClass("android.os.Process");
            int pid = process.CallStatic<int>("myPid");
            process.CallStatic("killProcess", pid);
        }
    }

    public static void SendMail(string email, string title)
    {
        string subject = MyEscapeURL(title);
        string body = MyEscapeURL("邮件预设内容");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    private static string MyEscapeURL(string url)
    {
        //%20是空格在url中的编码，这个方法将url中非法的字符转换成%20格式
        return WWW.EscapeURL(url).Replace("+", "%20");
    }


    /// <summary>
    /// 打开网页
    /// </summary>
    /// <param name="url"></param>
    public static void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    /// <summary>
    /// 是否是空对象
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNull(Object obj)
    {
        return null == obj;
    }

    /// <summary>
    /// 是否不是空对象
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsNotNull(Object obj)
    {
        return null != obj;
    }

    /// <summary>
    /// 实例化一个对象
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <param name="worldPositionStays"></param>
    /// <returns></returns>
    public static GameObject Instantiate(GameObject go, string name, Transform parent, bool worldPositionStays)
    {
        GameObject clone = null;
        if (null == go)
        {
            clone = new GameObject();
        }
        else
        {
            clone = GameObject.Instantiate(go);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            clone.name = name;
        }
        if (null != parent)
        {
            clone.transform.SetParent(parent, worldPositionStays);
        }
        return clone;
    }

    /// <summary>
    /// 设置父类使用默认数据 
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    /// <param name="worldPositionStays"></param>
    public static void EntityIdentity(GameObject go, string name, Transform parent, bool worldPositionStays)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            go.name = name;
        }
        go.transform.SetParent(parent, worldPositionStays);
        if (!worldPositionStays)
        {
            go.transform.localPosition = Vector3.zero;
        }
        go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        go.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 立即销毁
    /// </summary>
    /// <param name="go"></param>
    public static void DestroyImmediate(Object obj)
    {
        if (null != obj)
        {
            GameObject.DestroyImmediate(obj);
        }
    }

    /// <summary>
    /// 不销毁
    /// </summary>
    /// <param name="go"></param>
    public static void DontDestroyOnLoad(GameObject go)
    {
        if (null != go)
        {
            GameObject.DontDestroyOnLoad(go);
        }
    }

    /// <summary>
    /// 有坑
    /// </summary>
    public static void AtlasRequested()
    {
        //SpriteAtlasManager.atlasRequested += (string assetBundleName, System.Action<SpriteAtlas> action) =>
        //{
        //    assetBundleName = string.Format("res/ui/texture/{0}.spriteatlas", assetBundleName.ToLower());
        //    var asyncAsset = AssetManager.instance.LoadAsset(assetBundleName, null, false, App.abMode);
        //    action(asyncAsset.mainAsset as SpriteAtlas);
        //};

        //SpriteAtlasManager.atlasRegistered += (SpriteAtlas spriteAtlas) =>
        //{
        //    Sprite sprite = spriteAtlas.GetSprite("SpriteName");
        //};
    }

    /// <summary>
    /// 杀死指定DoTween
    /// </summary>
    /// <param name="doTweenId"></param>
    /// <param name="complete"></param>
    public static void DoTweenKill(string doTweenId, bool complete = false)
    {
        DOTween.Kill(doTweenId, complete);
    }

    /// <summary>
    /// 移动到指定Y值,局部坐标
    /// </summary>
    /// <param name="target"></param>
    /// <param name="endValue"></param>
    /// <param name="delay"></param>
    /// <param name="duration"></param>
    /// <param name="complete"></param>
    /// <param name="doTweenId"></param>
    public static void DOLocalMoveY(Transform target, float endValue, float delay, float duration, System.Action complete, string doTweenId = "")
    {
        Tweener tweener = target.DOLocalMoveY(endValue, duration);
        tweener.SetDelay(delay);
        if (null != complete)
        {
            tweener.onComplete = () => { complete(); };
        }
        if (string.IsNullOrWhiteSpace(doTweenId))
        {
            tweener.SetId(doTweenId);
        }
    }

    /// <summary>
    /// DoTween浮点
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="delay"></param>
    /// <param name="duration"></param>
    /// <param name="callBack"></param>
    /// <param name="complete"></param>
    public static void ToFloat(float start, float end, float delay, float duration, System.Action<float> callBack, System.Action complete)
    {
        DOTween.To(() => start, (value) => {
            if (callBack != null)
            {
                callBack(value);
            }
        }, end, duration).SetDelay(delay).onComplete = () => {
            if (complete != null)
            {
                complete();
            }
        };
    }

    /// <summary>
    /// 添加触发事件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="eventTriggerType"></param>
    /// <param name="e"></param>
    public static void AddEventTrigger(GameObject go, EventTriggerType eventTriggerType, UnityAction<BaseEventData> e)
    {
        EventTrigger eventTrigger = go.GetComponent<EventTrigger>();
        if (null == eventTrigger)
        {
            eventTrigger = go.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventTriggerType;
        entry.callback.AddListener(e);
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// 移除触发事件
    /// </summary>
    /// <param name=go"></param>
    public static void RemoveEventTrigger(GameObject go, EventTriggerType eventTriggerType)
    {
        EventTrigger eventTrigger = go.GetComponent<EventTrigger>();
        if (null != eventTrigger)
        {
            for (int i = eventTrigger.triggers.Count - 1; i >= 0; --i)
            {
                if (eventTrigger.triggers[i].eventID == eventTriggerType)
                {
                    eventTrigger.triggers.RemoveAt(i);
                }
            }
        }
    }

    /// <summary>
    /// 移除所有触发事件
    /// </summary>
    /// <param name="go"></param>
    public static void RemoveAllEventTrigger(GameObject go)
    {
        EventTrigger eventTrigger = go.GetComponent<EventTrigger>();
        if (null != eventTrigger)
        {
            eventTrigger.triggers.Clear();
        }
    }

    /// <summary>
    /// 改变层级
    /// </summary>
    /// <param name="target"></param>
    /// <param name="layerName"></param>
    public static void ChangeLayer(GameObject target, string layerName)
    {
        if (null != target)
        {
            int layer = LayerMask.NameToLayer(layerName); 
            target.layer = layer;
            for (int i = 0; i < target.transform.childCount; ++i)
            {
                ChangeLayer(target.transform.GetChild(i).gameObject, layerName);
            }
        }
    }

    /// <summary>
    /// 获取某个Canvas下鼠标点击位置的坐标
    /// </summary>
    /// <param name="rect">Canvas的RectTransform</param>
    /// <param name="camera">照射该Canvas的摄像机</param>
    /// <returns></returns>
    public static Vector2 ScreenPointToLocalPointInRectangle(RectTransform rect, Camera camera, Vector3 targetPos)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, targetPos, camera, out Vector2 data))
            return data;

        return Vector2.zero;
    }

    /// <summary>
    /// 文本格式化
    /// </summary>
    /// <param name="str"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string TextFormat(string str, params object[] args)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return str;
        }
        return string.Format(str, args);
    }


    /// <summary>
    /// 检测是否点击到UI上
    /// </summary>
    /// <returns></returns>
    public static bool CheckGuiRaycastObjects()
    {
        // PointerEventData eventData = new PointerEventData(Main.Instance.eventSystem);

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        // Main.Instance.graphicRaycaster.Raycast(eventData, list);
        EventSystem.current.RaycastAll(eventData, list);
        //Debug.Log(list.Count);
        return list.Count > 0;
    }

}

/// <summary>
/// 用于网络相关
/// </summary>
public partial class LuaHelper
{
    /// <summary>
    /// Http GET请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="complete"></param>
    /// <param name="timeout">超时时间</param>
    /// <param name="interval"></param>
    /// <param name="requestCount"></param>
    public static void WWWGetText(string url, System.Action<string, object> complete, int timeout = 6, float interval = 0f, int requestCount = 1)
    {
        Lua.instance.StartCoroutine(GetRequest(url, complete, timeout, interval, requestCount));
    }

    /// <summary>
    /// Http GET请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="complete"></param>
    /// <param name="timeout">超时时间</param>
    /// <param name="interval"></param>
    /// <param name="requestCount"></param>
    public static void WWWGetByte(string url, System.Action<string, object> complete, int timeout = 6, float interval = 0f, int requestCount = 1)
    {
        Lua.instance.StartCoroutine(GetRequest(url, complete, timeout, interval, requestCount, false));
    }

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="complete"></param>
    /// <param name="timeout">超时时间</param>
    /// <param name="interval">再次请求的时间间隔(单位·秒)</param>
    /// <param name="requestCount">再请求次数</param>
    /// <returns></returns>
    private static IEnumerator GetRequest(string url, System.Action<string, object> complete, int timeout = 6, float interval = 0f, int requestCount = 1, bool bText = true)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = timeout;
        request.SendWebRequest();
        while (request.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }
        if (--requestCount <= 0 || string.IsNullOrEmpty(request.error))
        {
            if (bText)
            {
                complete.Invoke(request.error, request.downloadHandler.text);
            }
            else
            {
                complete.Invoke(request.error, request.downloadHandler.data);
            }
            requestCount = 0;
        }
        request.Dispose();
        request = null;

        if (requestCount > 0)
        {
            yield return new WaitForSeconds(interval);
            if (bText)
            {
                WWWGetText(url, complete, timeout, interval, requestCount);
            }
            else
            {
                WWWGetByte(url, complete, timeout, interval, requestCount);
            }
        }
    }

    /// <summary>
    /// Http POST请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="form"></param>
    /// <param name="complete"></param>
    /// <param name="timeout">超时时间</param>
    /// <param name="interval"></param>
    /// <param name="requestCount"></param>
    public static void WWWPostText(string url, WWWForm form, System.Action<string, object> complete, int timeout = 6, float interval = 0f, int requestCount = 1)
    {
        Lua.instance.StartCoroutine(PostRequest(url, form, complete, timeout, interval, requestCount));
    }

    /// <summary>
    /// 得到一个Post Form
    /// </summary>
    /// <returns></returns>
    public static WWWForm GetWWWForm()
    {
        return new WWWForm();
    }

    /// <summary>
    /// Http POST请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="form"></param>
    /// <param name="complete"></param>
    /// <param name="timeout">超时时间</param>
    /// <param name="interval"></param>
    /// <param name="requestCount"></param>
    public static void WWWPostByte(string url, WWWForm form, System.Action<string, object> complete, int timeout = 6, float interval = 0f, int requestCount = 1)
    {
        Lua.instance.StartCoroutine(PostRequest(url, form, complete, timeout, interval, requestCount, false));
    }

    /// <summary>
    /// POST请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="form"></param>
    /// <param name="complete"></param>
    /// <param name="timeout">超时时间</param>
    /// <param name="interval">再次请求的时间间隔(单位·秒)</param>
    /// <param name="requestCount">再请求次数</param>
    /// <returns></returns>
    private static IEnumerator PostRequest(string url, WWWForm form, System.Action<string, object> complete, int timeout = 6, float interval = 0f, int requestCount = 1, bool bText = true)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.timeout = timeout;
        request.SendWebRequest();
        while (request.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }
        if (--requestCount <= 0 || string.IsNullOrEmpty(request.error))
        {
            if (bText)
            {
                complete.Invoke(request.error, request.downloadHandler.text);
            }
            else
            {
                complete.Invoke(request.error, request.downloadHandler.data);
            }
            requestCount = 0;
        }
        request.Dispose();
        request = null;

        if (requestCount > 0)
        {
            yield return new WaitForSeconds(interval);
            if (bText)
            {
                WWWPostText(url, form, complete, timeout, interval, requestCount);
            }
            else
            {
                WWWPostByte(url, form, complete, timeout, interval, requestCount);
            }
        }
    }

    /// <summary>
    /// 得到URL精灵图
    /// </summary>
    /// <param name="url"></param>
    /// <param name="complete"></param>
    /// <param name="timeout"></param>
    /// <param name="interval"></param>
    /// <param name="requestCount"></param>
    public static void GetUrlSprite(string url, System.Action<string, Sprite> complete, int timeout = 6, float interval = 0f, int requestCount = 1)
    {
        Lua.instance.StartCoroutine(DownloadImage(url, complete, timeout, interval, requestCount));
    }

    /// <summary>
    /// 下载图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="complete"></param>
    /// <param name="timeout"></param>
    /// <param name="interval"></param>
    /// <param name="requestCount"></param>
    /// <param name="bText"></param>
    /// <returns></returns>
    private static IEnumerator DownloadImage(string url, System.Action<string, Sprite> complete, int timeout = 6, float interval = 0f, int requestCount = 1)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        request.timeout = timeout;
        request.SendWebRequest();
        while (request.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }
        if (--requestCount <= 0 || string.IsNullOrEmpty(request.error))
        {
            Sprite sprite = null;
            if (string.IsNullOrWhiteSpace(request.error))
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }

            complete.Invoke(request.error, sprite);
            requestCount = 0;
        }
        request.Dispose();
        request = null;

        if (requestCount > 0)
        {
            yield return new WaitForSeconds(interval);
            DownloadImage(url, complete, timeout, interval, requestCount);
        }
    }
}

/// <summary>
/// 用于资源的加载与卸载
/// </summary>
public partial class LuaHelper
{
    /// <summary>
    /// AB字节加载
    /// </summary>
    /// <param name="data"></param>
    /// <param name="name"></param>
    public static void AssetBundleLoadFromMemory(byte[] data, string name, System.Action<Object> action)
    {
        AssetBundle assetBundle = AssetBundle.LoadFromMemory(data);
        action.Invoke(assetBundle.LoadAsset(name));
        assetBundle.Unload(true);
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static AsyncAsset AssetBundleLoad(string path)
    {
        return AssetManager.instance.AssetBundleLoad(path);
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="complete"></param>
    /// <returns></returns>
    public static AsyncAsset AssetBundleAsyncLoad(string path, System.Action<bool, AsyncAsset> complete)
    {
        return AssetManager.instance.AssetBundleAsyncLoad(path, complete);
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="complete"></param>
    /// <param name="async"></param>
    /// <returns></returns>
    public static AsyncAsset LoadAsset(string name, System.Action<bool, AsyncAsset> complete, bool async = true)
    {
        return AssetManager.instance.LoadAsset(name, complete, async, App.abMode);
    }

    /// <summary>
    /// 同步加载文件资源
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static AsyncAsset FileAssetLoad(string path)
    {
        return AssetManager.instance.FileAssetLoad(path);
    }

    /// <summary>
    /// 异步加载文件资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="complete"></param>
    /// <returns></returns>
    public static AsyncAsset FileAssetAsyncLoad(string path, System.Action<bool, AsyncAsset> complete)
    {
        return AssetManager.instance.FileAssetAsyncLoad(path, complete);
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="unloadAllLoadedObjects"></param>
    public static void UnloadAsset(AsyncAsset asset, bool unloadAllLoadedObjects)
    {
        AssetManager.instance.UnloadAsset(asset, unloadAllLoadedObjects);
    }

    /// <summary>
    /// 卸载所有资源
    /// </summary>
    public static void UnloadAssets(bool unloadAllLoadedObjects = true)
    {
        AssetManager.instance.UnloadAsset(unloadAllLoadedObjects);
    }

    /// <summary>
    /// 池中加载，返回对象
    /// </summary>
    /// <param name="name"></param>
    /// <param name="complete"></param>
    /// <param name="async"></param>
    public static void LoadFromPool(string name, System.Action<GameObject> complete, bool async = true)
    {
        AssetPool.instance.LoadFromPool(name, complete, async);
    }

    /// <summary>
    /// 卸载资源到池
    /// </summary>
    /// <param name="go"></param>
    public static void UnloadToPool(GameObject go)
    {
        AssetPool.instance.UnloadToPool(go);
    }

    /// <summary>
    /// 池中加载，返回异步资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="complete"></param>
    /// <param name="async"></param>
    public static AsyncAsset LoadAssetFromPool(string name, System.Action<bool, AsyncAsset> complete, bool async = true)
    {
        return AssetPool.instance.LoadAssetFromPool(name, complete, async);
    }

    /// <summary>
    /// 池中加载，返回异步资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="complete"></param>
    /// <param name="async"></param>
    public static void LoadObjectFromPool(string name, System.Action<UnityEngine.Object> complete, bool async = true)
    {
        AssetPool.instance.LoadObjectFromPool(name, complete, async);
    }
}

/// <summary>
/// Shader处理
/// </summary>
public partial class LuaHelper
{

    /// <summary>
    /// 初始化Shader
    /// </summary>
    /// <param name="shaders"></param>
    public static void InitShader(Object[] shaders)
    {
        ShaderPool.instance.Init(shaders);
    }

    /// <summary>
    /// 初始化材质球
    /// </summary>
    /// <param name="materials"></param>
    public static void InitMaterial(Object[] materials)
    {
        MaterialPool.instance.Init(materials);
    }

    /// <summary>
    /// 得到一个Shader
    /// </summary>
    /// <param name="name"></param>
    public static Shader GetShader(string name)
    {
        return ShaderPool.instance.GetShader(name);
    }

    /// <summary>
    /// 设置图形置灰
    /// </summary>
    /// <param name="graphic"></param>
    /// <param name="enable"></param>
    /// <param name="power"></param>
    public static void SetGray(UnityEngine.UI.Graphic graphic, bool enable, float power = 1f)
    {
        if (!enable && graphic.material.name != "UI/UIEffect")
        {
            return;
        }
        UIEffectIns.SetGrayEffect(graphic, enable, power);
    }

    /// <summary>
    /// 为True设置置灰效果
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="power"></param>
    public static void SetGrayInChildren(GameObject go, bool enable, float power = 1f)
    {
        UnityEngine.UI.Graphic[] graphics = go.GetComponentsInChildren<UnityEngine.UI.Graphic>(true);
        for (int i = 0; i < graphics.Length; ++i)
        {
            if (!enable && (graphics[i].material.name != "UI/UIEffect"))
            {
                continue;
            }
            UIEffectIns.SetGrayEffect(graphics[i], enable, power);
        }
    }

    /// <summary>
    /// Spine是否开启描边
    /// </summary>
    /// <param name="go"></param>
    /// <param name="outline"></param>
    public static void SetSpineOutline(GameObject go, bool outline)
    {
        Spine.Unity.SkeletonRenderer renderer = go.GetComponentInChildren<Spine.Unity.SkeletonRenderer>(true);
        UIEffectIns.SetSpineOutline(renderer, outline, Color.red, 16);
    }
}

/// <summary>
/// 屏幕位置转换
/// </summary>
public partial class LuaHelper
{
    /// <summary>
    /// 得到UI大小
    /// </summary>
    /// <returns></returns>
    public static Vector2 GetCanvasSize()
    {
        return UIManager.instance.uiRoot.sizeDelta;
    }

    public static Camera uiCamera => UIManager.instance.uiCamera;

    public static RectTransform uiRoot => UIManager.instance.uiRoot;

    public static Vector3 ScreenPointToWorldPoint(RectTransform rect, Vector2 point)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, point, UIManager.instance.uiCamera, out pos);
        return pos;
    }

    public static Vector3 ScreenPointToWorldPointByCamera(RectTransform rect, Vector2 point, Camera camera)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, point, camera, out pos);
        return pos;
    }

    public static Vector2 ScreenPointToUIPoint(RectTransform rect, Vector2 point)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, point, UIManager.instance.uiCamera, out pos);
        return pos;
    }

    public static Vector2 WorldToScreenPoint(Vector3 pos)
    {
        return RectTransformUtility.WorldToScreenPoint(UIManager.instance.uiCamera, pos);
    }

    /// <summary>
    /// 将3d场景中的位置转换对应屏幕上的ui位置
    /// </summary>
    /// <param name="camera3D">3d场景中的摄像机</param>
    /// <param name="worldPos3D">3d场景中的世界位置</param>
    /// <param name="uiRectTransform">ui</param>
    /// <returns></returns>
    public static Vector2 WorldPosToUiLocalPointIn3DScene(Camera camera3D, Vector3 worldPos3D, RectTransform uiRectTransform)
    {
        Vector3 screenPos = camera3D.WorldToScreenPoint(worldPos3D);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiRectTransform, screenPos, UIManager.instance.uiCamera, out localPos);
        return localPos;
    }

    public static bool RectangleContainsScreenPoint(RectTransform rect, Vector2 screenPoint, Camera camera = null)
    {
        if (null == camera)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint);
        }
        return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPoint, camera);
    }
}

/// <summary>
/// 设置粒子，Spine，UI层级
/// </summary>
public partial class LuaHelper
{
    /// <summary>
    /// 设置粒子层级，一般动态加载使用
    /// </summary>
    /// <param name="go"></param>
    /// <param name="sortingOrder"></param>
    public static void SetParticleSortingOrder(GameObject go, int sortingOrder)
    {
        if (null != go)
        {
            Renderer[] renderers = go.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < renderers.Length; ++i)
            {
                renderers[i].sortingOrder = sortingOrder;
            }
        }
    }

    /// <summary>
    /// 设置Spine层级，一般动态加载使用
    /// </summary>
    /// <param name="go"></param>
    /// <param name="sortingOrder"></param>
    public static void SetSpineSortingOrder(GameObject go, int sortingOrder)
    {
        SetParticleSortingOrder(go, sortingOrder);
    }

    /// <summary>
    /// 设置UI层级
    /// </summary>
    /// <param name="go"></param>
    /// <param name="sortingOrder"></param>
    public static void SetUISortingOrder(GameObject go, int sortingOrder)
    {
        if (null != go)
        {
            Canvas[] renderers = go.GetComponentsInChildren<Canvas>(true);
            for (int i = 0; i < renderers.Length; ++i)
            {
                renderers[i].overrideSorting = true;
                renderers[i].sortingOrder = sortingOrder;
            }
        }
    }
}