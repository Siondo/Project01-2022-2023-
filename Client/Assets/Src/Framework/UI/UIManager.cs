using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Framework
{
    using Singleton;
    using Event;
    using UnityAsset;
    using Pool;
    namespace UI
    {
        /// <summary>
        /// UI管理器
        /// </summary>
        public sealed class UIManager : MonoBehaviourSingleton<UIManager>
        {
            /// <summary>
            /// 空对象数组
            /// </summary>
            private static readonly object[] EMPTYOBJECTARRAY = new object[0];

            /// <summary>
            /// UI资源加载状态
            /// </summary>
            public enum UIState
            {
                None = 0,
                Loading,
                Complete,
            }

            public class UIData
            {
                /// <summary>
                /// UI名字
                /// </summary>
                private string m_name = string.Empty;

                /// <summary>
                /// 是否显示
                /// </summary>
                private bool m_show = false;

                /// <summary>
                /// 参数
                /// </summary>
                private object[] m_args = EMPTYOBJECTARRAY;

                /// <summary>
                /// UI
                /// </summary>
                private UIBase m_uiBase = null;

                /// <summary>
                /// UI资源加载状态
                /// </summary>
                private UIState m_uIState = UIState.None;

                /// <summary>
                /// 资源
                /// </summary>
                private AsyncAsset m_asyncAsset = null;

                /// <summary>
                /// UI名字
                /// </summary>
                public string name
                {
                    get { return m_name; }
                    set { m_name = value; }
                }

                /// <summary>
                /// 是否显示
                /// </summary>
                public bool show
                {
                    get { return m_show; }
                    set { m_show = value; }
                }

                /// <summary>
                /// 参数
                /// </summary>
                public object[] args
                {
                    get { return m_args; }
                    set { m_args = value; }
                }

                /// <summary>
                /// UI
                /// </summary>
                public UIBase uiBase
                {
                    get { return m_uiBase; }
                    set { m_uiBase = value; }
                }

                /// <summary>
                /// UI资源加载状态
                /// </summary>
                public UIState uIState
                {
                    get { return m_uIState; }
                    set { m_uIState = value; }
                }

                /// <summary>
                /// 资源
                /// </summary>
                public AsyncAsset asyncAsset
                {
                    get { return m_asyncAsset; }
                    set { m_asyncAsset = value; }
                }

                /// <summary>
                /// 清理数据
                /// </summary>
                public void Clear()
                {
                    m_name = string.Empty;
                    m_args = EMPTYOBJECTARRAY;
                    m_show = false;
                    m_uiBase = null;
                    uIState = UIState.None;
                    m_asyncAsset = null;
                }
            }

            public class UIWindowData
            {
                /// <summary>
                /// UI名字
                /// </summary>
                private string m_name = string.Empty;

                /// <summary>
                /// UI数据
                /// </summary>
                private UIData m_data = null;

                /// <summary>
                /// 参数
                /// </summary>
                private object[] m_args = EMPTYOBJECTARRAY;

                /// <summary>
                /// UI名字
                /// </summary>
                public string name
                {
                    get { return m_name; }
                    set { m_name = value; }
                }

                /// <summary>
                /// UI数据
                /// </summary>
                public UIData data
                {
                    get { return m_data; }
                    set { m_data = value; }
                }

                /// <summary>
                /// 参数
                /// </summary>
                public object[] args
                {
                    get { return m_args; }
                    set { m_args = value; }
                }

                /// <summary>
                /// 清理数据
                /// </summary>
                public void Clear()
                {
                    m_name = string.Empty;
                    m_data = null;
                    m_args = EMPTYOBJECTARRAY;
                }
            }

            public GameObject m_DisplayLogo;
            public Font m_TargetFont;

            #region Variable
            /// <summary>
            /// 摄像机
            /// </summary>
            [SerializeField]
            private Camera m_uiCamera = null;

            /// <summary>
            /// UIRoot
            /// </summary>
            [SerializeField]
            private RectTransform m_uiRoot = null;

            /// <summary>
            /// UI背景层
            /// </summary>
            [SerializeField]
            private RectTransform m_background = null;

            /// <summary>
            /// UI默认层
            /// </summary>
            [SerializeField]
            private RectTransform m_default = null;

            /// <summary>
            /// UI弹框层
            /// </summary>
            [SerializeField]
            private RectTransform m_popup = null;

            /// <summary>
            /// UI顶层
            /// </summary>
            [SerializeField]
            private RectTransform m_top = null;

            /// <summary>
            /// UI父类
            /// </summary>
            private RectTransform[] m_uiParent = null;

            /// <summary>
            /// 窗口UI栈
            /// </summary>
            private Stack<UIWindowData> m_windowStack = null;

            /// <summary>
            /// 界面集合
            /// </summary>
            private Dictionary<string, UIData> m_data = null;

            /// <summary>
            /// 窗口数据池
            /// </summary>
            private Pool<UIWindowData> m_windowPool = null;

            /// <summary>
            /// 池数据
            /// </summary>
            private Pool<UIData> m_pool = null;

            /// <summary>
            /// 当前打开的窗口
            /// </summary>
            private UIWindowData m_uIData = null;

            /// <summary>
            /// 等待下一帧
            /// </summary>
            private WaitForEndOfFrame m_waitForEndOfFrame = new WaitForEndOfFrame();
            #endregion

            #region Property
            /// <summary>
            /// UiCamera
            /// </summary>
            public Camera uiCamera => m_uiCamera;

            /// <summary>
            /// UiRoot
            /// </summary>
            public RectTransform uiRoot => m_uiRoot;

#if UNITY_EDITOR
            /// <summary>
            /// 得到窗口栈信息
            /// </summary>
            /// <returns></returns>
            public UIWindowData[] GetAllWindow()
            {
                var array = null != m_windowStack ? m_windowStack.ToArray() : new UIWindowData[0];
                List<UIWindowData> window = new List<UIWindowData>(array);
                if (null != m_uIData)
                {
                    window.Insert(0, m_uIData);
                }
                return window.ToArray();
            }

            /// <summary>
            /// 得到UI数据
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public UIData GetUIData(string name)
            {
                return GetUI(name);
            }

            /// <summary>
            /// 得到所有的对话框
            /// </summary>
            /// <returns></returns>
            public UIData[] GetAllDialog()
            {
                List<UIData> dialog = new List<UIData>();
                if (null != m_data)
                {
                    dialog.AddRange(m_data.Values);
                }
                return dialog.ToArray();
            }
#endif
            #endregion


            #region Function
            /// <summary>
            /// 创建
            /// </summary>
            protected override void Create()
            {
                base.Create();
                m_DisplayLogo = transform.Find("DisplayLogo").gameObject;
                m_windowStack = new Stack<UIWindowData>(8);
                m_data = new Dictionary<string, UIData>(32);

                m_windowPool = PoolManager.instance.Get<Pool<UIWindowData>>();
                m_pool = PoolManager.instance.Get<Pool<UIData>>();

                m_uiParent = new RectTransform[]
                { 
                    m_background, m_default, m_popup, m_top
                };
            }

            public void onLoadTargetFont(TextEx text)
            {
                if (m_TargetFont == null)
                {
                    var fontString = string.Empty;
                    var platform = LuaHelper.GetAppPlatform();
                    if (platform.Equals("tc"))
                        fontString = "cn.otf";
                    else if (platform.Equals("en"))
                        fontString = "en.ttf";
                    LuaHelper.LoadAsset("Res/Font/" + fontString, (reslut, asset) =>
                      {
                          if (reslut)
                          {
                              m_TargetFont = asset.mainAsset as Font;
                              //UnityEditor.Undo.RecordObject(text, text.gameObject.name);
                              text.font = m_TargetFont;
                          }

                      }, false);
                }
                else
                {
                    //UnityEditor.Undo.RecordObject(text, text.gameObject.name);
                    text.font = m_TargetFont;
                }
            }

            /// <summary>
            /// 得到UI
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public UIData GetUI(string name)
            {
                return m_data.ContainsKey(name) ? m_data[name] : null;
            }

            /// <summary>
            /// 是否显示
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool IsShow(string name)
            {
                bool bShow = false;
                var ui = GetUI(name);
                if (null != ui && ui.show && null != ui.uiBase && ui.uiBase.uiState == UI.UIState.Show)
                {
                    bShow = true;
                }
                return bShow;
            }

            public UIBase GetUIBase(string name)
            {
                var ui = GetUI(name);
                return null != ui ? ui.uiBase : null;
            }

            /// <summary>
            /// 设置当前窗口信息
            /// </summary>
            /// <param name="name"></param>
            public void SetCurrentWindowInfo(params object[] args)
            {
                if (null != m_uIData)
                {
                    m_uIData.args = args;
                }
            }

            /// <summary>
            /// 设置指定窗口信息
            /// </summary>
            /// <param name="name"></param>
            /// <param name="args"></param>
            public void SetWindowInfo(string name, params object[] args)
            {
                foreach (var window in m_windowStack)
                {
                    if (window.name == name)
                    {
                        window.args = args;
                        break;
                    }
                }
            }

            /// <summary>
            /// 等待当前帧结束
            /// </summary>
            /// <param name="action"></param>
            /// <returns></returns>
            private IEnumerator WaitForEndOfFrame(System.Action action)
            {
                yield return m_waitForEndOfFrame;
                action();
            }

            /// <summary>
            /// 开启或禁用所有UI响应
            /// </summary>
            public void SetEventSystemEnable(bool enabled)
            {
                EventSystem.current.enabled = enabled;
            }

            /// <summary>
            /// 能否返回到上一个窗口
            /// </summary>
            /// <returns></returns>
            public bool CanReturnLastWindow()
            {
                return m_windowStack.Count > 1;
            }

            /// <summary>
            /// 显示UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="immediate"></param>
            /// <param name="complete"></param>
            /// <param name="args"></param>
            private void ShowUI(string name, bool immediate, System.Action complete, params object[] args)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: {0} is null or empty!", name);
                    return;
                }

                UIData data = GetUI(name);
                if (data != null)
                {
                    data.show = true;
                    data.args = args;
                    if (data.uIState == UIState.Complete)
                    {
                        ShowUI(data);
                        complete?.Invoke();
                    }
                }
                else
                {
                    data = m_pool.Get();
                    data.name = name;
                    data.show = true;
                    data.args = args;
                    data.uIState = UIState.Loading;
                    m_data.Add(data.name, data);
                    data.asyncAsset = AssetManager.instance.LoadUI(name, (bResult, target) =>
                    {
                        System.Action func = () =>
                        {
                            if (bResult)
                            {
                                GameObject go = target.Instantiate();
                                if (null == go)
                                {
                                    Debug.LogErrorFormat("UI: {0} instantiate fail!", target.assetName);
                                    return;
                                }
                                go.name = go.name.Replace("(Clone)", "");
                                UIBase t = go.GetComponent<UIBase>();
                                if (null == t)
                                {
                                    Debug.LogErrorFormat("UI: {0} 'UIBase' is not find!", go.name);
                                }
                                else
                                {
                                    go.transform.SetParent(m_uiParent[(int)t.uiLayer]);
                                    go.transform.localPosition = Vector3.zero;
                                    go.transform.localScale = Vector3.one;
                                    go.transform.localRotation = Quaternion.identity;
                                    RectTransform rectTransform = go.transform as RectTransform;
                                    if (rectTransform != null)
                                    {
                                        rectTransform.offsetMin = Vector3.one * -1;
                                        rectTransform.offsetMax = Vector3.one;
                                    }

                                    data = GetUI(data.name); //这里不能根据 界面/组件 的物体名称传入查询， 要依LUA->UICommon内的配置来查询 （go.name 更为 data.name） 2022/3/28 by siondo
                                    data.uiBase = t;
                                    data.uIState = UIState.Complete;
                                    data.uiBase.OnLoad();
                                    ShowUI(data);
                                    complete?.Invoke();
                                }
                            }
                            else
                            {
                                Debug.LogErrorFormat("UI: {0} load fail!", target.assetName);
                            }
                        };
                        if (immediate)
                        {
                            func();
                        }
                        else
                        {
                            StartCoroutine(WaitForEndOfFrame(func));
                        }

                    }, async: !immediate);
                }
            }

            /// <summary>
            /// 显示UI
            /// </summary>
            /// <param name="data"></param>
            private void ShowUI(UIData data)
            {
                //打开窗口，上一个窗口隐藏并进入堆栈，并且默认类型窗口关闭
                if (data.uiBase.uiType == UIType.Window)
                {
                    //上一个窗口隐藏并进入堆栈
                    if (m_uIData != null && data != m_uIData.data)
                    {
                        m_uIData.data.show = false;
                        HideUI(m_uIData.data);

                        m_uIData.data = null;
                        m_windowStack.Push(m_uIData);
                        m_uIData = null;
                    }
                    //设置新的当前窗口
                    m_uIData = m_windowPool.Get();
                    m_uIData.name = data.name;
                    m_uIData.data = data;
                }
                else if(data.uiBase.uiType == UIType.Dialog && data.uiBase.uiLayer == UILayer.Popup)
                {
                    if (data.name.Contains("BattleListView") || data.name.Contains("FriendListView") || data.name.Contains("MainListView")
                        || data.name.Contains("RecordListView") || data.name.Contains("ShopListView") || data.name.Contains("RankListView")) { }
                    else
                    {
                        if (!data.uiBase.transform.Find("COVER-MASK[AUTOBORN]"))
                        {
                            var mask = LuaHelper.Instantiate(new GameObject(), "COVER-MASK[AUTOBORN]", data.uiBase.transform, false);
                            mask.transform.SetAsFirstSibling();

                            var rect = mask.AddComponent<RectTransform>();
                            rect.anchorMin = Vector2.zero;
                            rect.anchorMax = Vector2.one;
                            rect.pivot = Vector2.one / 2;
                            rect.right = Vector3.zero;

                            var imgEx = mask.AddComponent<ImageEx>();
                            imgEx.color = new Color(0, 0, 0, 0.7f);

                            var btnEx = mask.AddComponent<ButtonEx>();
                            btnEx.transition = UnityEngine.UI.Selectable.Transition.None;
                            btnEx.onClick.RemoveAllListeners();
                            btnEx.onClick.AddListener(() =>
                            {
                                data.uiBase.OnHide();
                            });
                        }
                    }
                }

                //设置UI显示层级
                if (null != data.uiBase)
                {
                    //打开窗口
                    data.uiBase.transform.SetAsLastSibling();

                    int layer = (int)data.uiBase.uiLayer, cnt = 0;
                    Transform parent = m_uiParent[layer];
                    int childCount = parent.childCount;
                    UIBase ui = null;
                    for (int i = 0; i < childCount; i++)
                    {
                        ui = parent.GetChild(i).GetComponent<UIBase>();
                        if (data.uiBase == ui)
                        {
                            ui.SetSortingOrder(layer * Const.UI_START_SORTINGORDER + cnt++ * Const.UI_INTERVAL_SORTINGORDER);
                            if (data.uiBase.uiState == UI.UIState.Show)
                            {
                                data.uiBase.OnUpdate(data.args);
                            }
                            else
                            {
                                data.uiBase.OnShow(data.args);
                            }
                        }
                        else if (ui.uiState == UI.UIState.Show)
                        {
                            ui.SetSortingOrder(layer * Const.UI_START_SORTINGORDER + cnt++ * Const.UI_INTERVAL_SORTINGORDER);
                        }
                    }
                }
            }

            /// <summary>
            /// 隐藏UI
            /// </summary>
            /// <param name="data"></param>
            private void HideUI(UIData data)
            {
                if (data.uiBase.uiState != UI.UIState.Hide)
                {
                    data.uiBase.OnHide();
                    if (data.uiBase.destroyMode == UIDestroyMode.Hide)
                    {
                        DestroyUI(data);
                    }
                }
            }

            /// <summary>
            /// 销毁UI
            /// </summary>
            /// <param name="data"></param>
            private void DestroyUI(UIData data)
            {
                data.uiBase.OnUnload();
                data.asyncAsset.Destroy(data.uiBase.gameObject);
                AssetManager.instance.UnloadAsset(data.asyncAsset, true);

                m_data.Remove(data.name);
                data.Clear();
                m_pool.Release(data);
            }

            /// <summary>
            /// 显示UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="args"></param>
            public void ShowUI(string name, params object[] args)
            {
                ShowUI(name, false, null, args);
            }

            /// <summary>
            /// 立即显示UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="args"></param>
            public void ShowUIImmediate(string name, params object[] args)
            {
                ShowUI(name, true, null, args);
            }

            /// <summary>
            /// 关闭UI
            /// </summary>
            /// <param name="name"></param>
            /// <param name="param"></param>
            public void HideUI(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    Debug.LogErrorFormat("UI: {0} is null or empty!", name);
                    return;
                }

                UIData data = GetUI(name);
                if (null != data)
                {
                    //如果关闭的界面是当前窗口,从窗口栈弹出窗口，如果栈中没有，则打开主窗口
                    if (null != m_uIData && data == m_uIData.data)
                    {
                        m_uIData.Clear();
                        m_windowPool.Release(m_uIData);
                        m_uIData = null;

                        if (m_windowStack.Count > 0)
                        {
                            UIWindowData temp = m_windowStack.Pop();
                            ShowUI(temp.name, false, () =>
                            {
                                //隐藏当前UI
                                data.show = false;
                                HideUI(data);

                                temp.Clear();
                                m_windowPool.Release(temp);
                                temp = null;
                            }, temp);
                        }
                        else
                        {                        
                            //隐藏当前UI
                            data.show = false;
                            HideUI(data);
                        }
                    }
                    else
                    {
                        //隐藏当前UI
                        data.show = false;
                        HideUI(data);
                    }
                }
            }

            /// <summary>
            /// 隐藏所有显示的UI
            /// </summary>
            /// <param name="destroy">关闭后是否销毁</param>
            public void HideAllUI(bool destroy)
            {
                //隐藏所有的UI，并销毁
                List<UIData> list = new List<UIData>(m_data.Values);
                foreach (var data in list)
                {
                    if (data.uiBase.uiState == UI.UIState.Show)
                    {
                        data.uiBase.OnHide();
                    }
                    if (destroy)
                    {
                        DestroyUI(data);
                    }
                }
                //数据入池
                if (m_uIData != null)
                {
                    m_uIData.Clear();
                    m_windowPool.Release(m_uIData);
                    m_uIData = null;
                }
                while (m_windowStack.Count > 0)
                {
                    m_uIData = m_windowStack.Pop();
                    m_uIData.Clear();
                    m_windowPool.Release(m_uIData);
                }
                m_uIData = null;
                m_windowStack.Clear();
            }

            /// <summary>
            /// 移除所有窗口UI数据
            /// </summary>
            public void RemoveAllWindowStackData()
            {
                foreach (var data in m_windowStack)
                {
                    data.Clear();
                    m_windowPool.Release(data);
                }
            }

            /// <summary>
            /// 移除指定的窗口数据
            /// </summary>
            /// <param name="name"></param>
            public void RemoveWindowStackData(string name)
            {
                List<UIWindowData> list = new List<UIWindowData>(m_windowStack.ToArray());
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    if (list[i].name == name)
                    {
                        list[i].Clear();
                        m_windowPool.Release(list[i]);
                        list.RemoveAt(i);
                        break;
                    }
                }
                m_windowStack.Clear();
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    m_windowStack.Push(list[i]);
                }
            }

            /// <summary>
            /// 刷新所有的文本语言
            /// </summary>
            /// <param name="callback"></param>
            public void RefreshAllText(System.Action callback = null)
            {
                StopCoroutine("CorRefreshAllText");
                StartCoroutine("CorRefreshAllText", callback);
            }

            /// <summary>
            /// 协同刷新
            /// </summary>
            /// <param name="updateCall"></param>
            /// <returns></returns>
            private IEnumerator CorRefreshAllText(System.Action callback)
            {
                var allTxt = m_uiRoot.GetComponentsInChildren<TextEx>(true);
                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < allTxt.Length; ++i)
                {
                    allTxt[i].RefreshText();

                    if (i % 5 == 0)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }

                callback?.Invoke();
                yield return 0;
            }
            #endregion
        }
    }
}
