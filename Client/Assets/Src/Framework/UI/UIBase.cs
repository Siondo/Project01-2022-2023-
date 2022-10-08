using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using XLua;

namespace Framework
{
    using Event;

    namespace UI
    {
        /// <summary>
        /// UI层级
        /// </summary>
        public enum UILayer
        {
            /// <summary>
            /// 背景层(比如主界面)
            /// </summary>
            Background = 0,

            /// <summary>
            /// 默认层(其他功能界面)
            /// </summary>
            Default = 1,

            /// <summary>
            /// 弹出窗口层(比如提示框)
            /// </summary>
            Popup = 2,

            /// <summary>
            /// 最上层
            /// </summary>
            Top = 3,
        }

        /// <summary>
        /// UI界面类型
        /// </summary>
        public enum UIType
        {
            /// <summary>
            /// 打开时会关闭上一个Window界面 点关闭或返回可返回上一个Window
            /// </summary>
            Window = 0,

            /// <summary>
            /// 直接在指定层打开 不会关闭其他界面
            /// </summary>
            Dialog = 1,
        }

        /// <summary>
        /// 销毁方式
        /// </summary>
        public enum UIDestroyMode
        {
            /// <summary>
            /// 常驻 退出游戏时才销毁
            /// </summary>
            Never = 0,

            /// <summary>
            /// 切换场景时销毁
            /// </summary>
            SceneChange = 1,

            /// <summary>
            /// 界面关闭时销毁
            /// </summary>
            Hide = 2,
        }

        /// <summary>
        /// 显示或隐藏方式
        /// </summary>
        public enum UIShowOrHideMode
        {
            /// <summary>
            /// 通知设置Active来显示或隐藏对象
            /// </summary>
            Active = 0,

            /// <summary>
            /// 通过改变Laye来显示或隐藏对象
            /// </summary>
            Layer = 1,
        }

        /// <summary>
        /// UI显示的状态
        /// </summary>
        public enum UIState
        {
            /// <summary>
            /// UI显示
            /// </summary>
            Show = 0,

            /// <summary>
            /// UI隐藏
            /// </summary>
            Hide = 1,
        }

        /// <summary>
        /// UIBase
        /// </summary>
        public class UIBase : UIToLua
        {
            #region Variable
            /// <summary>
            /// UI的层级
            /// </summary>
            [Tooltip("UI的层级")]
            [SerializeField] protected UILayer m_uiLayer = UILayer.Default;

            /// <summary>
            /// UI的类型
            /// </summary>
            [Tooltip("UI的类型")]
            [SerializeField] protected UIType m_uiType = UIType.Window;

            /// <summary>
            /// 界面销毁方式
            /// </summary>
            [Tooltip("界面销毁方式")]
            [SerializeField] protected UIDestroyMode m_destroyMode = UIDestroyMode.SceneChange;

            /// <summary>
            /// 卸载方式
            /// </summary>
            [Tooltip("显示或隐藏方式")]
            [SerializeField] protected UIShowOrHideMode m_showOrHideMode = UIShowOrHideMode.Layer;

            /// <summary>
            /// 显示状态
            /// </summary>
            protected UIState m_uiState = UIState.Hide;

            /// <summary>
            /// 层级顺序
            /// </summary>
            [SerializeField] protected int m_sortOrder = 0;

            /// <summary>
            /// Canvas设置层级后跟显示相关
            /// </summary>
            protected Canvas m_canvas = null;

            /// <summary>
            /// GraphicRaycaster图像射线碰撞相关
            /// </summary>
            protected GraphicRaycaster m_graphicRaycaster = null;

            /// <summary>
            /// Lua表
            /// </summary>
            private LuaTable m_table = null;

            /// <summary>
            /// 显示
            /// </summary>
            private LuaFunction m_onShow = null;

            /// <summary>
            /// 更新
            /// </summary>
            private LuaFunction m_onUpdate = null;

            /// <summary>
            /// 隐藏
            /// </summary>
            private LuaFunction m_onHide = null;

            /// <summary>
            /// 卸载
            /// </summary>
            private LuaFunction m_onUnload = null;

            /// <summary>
            /// 粒子特效Spine使用层级设置
            /// </summary>
            private List<UISortingOrder> m_sorting = new List<UISortingOrder>();
            #endregion

            #region Property
            /// <summary>
            /// UI层级
            /// </summary>
            public virtual UILayer uiLayer
            {
                get { return m_uiLayer; }
            }

            /// <summary>
            /// UI类型
            /// </summary>
            public virtual UIType uiType
            {
                get { return m_uiType; }
            }

            /// <summary>
            /// 界面销毁方式
            /// </summary>
            public virtual UIDestroyMode destroyMode
            {
                get { return m_destroyMode; }
            }

            /// <summary>
            /// 显示或隐藏方式
            /// </summary>
            public UIShowOrHideMode showOrHideMode
            {
                get { return m_showOrHideMode; }
            }

            /// <summary>
            /// 当前UI状态
            /// </summary>
            public UIState uiState
            {
                get { return m_uiState; }
            }

            /// <summary>
            /// 排序顺序
            /// </summary>
            public int sortingOrder
            {
                get
                {
                    return m_canvas.sortingOrder;
                }
            }
            #endregion

            #region Function
            /// <summary>
            /// 启动
            /// </summary>
            protected override void Awake()
            {
#if UNITY_EDITOR
                var c = GetComponent<Canvas>();
                if (null == c && !Lua.hasInstance)
                {
                    gameObject.SetActive(false);
                    return;
                }
#endif
                gameObject.SetActive(true);

                base.Awake();
            }

            /// <summary>
            /// 开始
            /// </summary>
            protected override void Start()
            {
                base.Start();
                if (null == m_table)
                {
                    enabled = false;
                }
            }

            /// <summary>
            /// 销毁
            /// </summary>
            protected override void OnDestroy()
            {
                base.OnDestroy();
            }


            /// <summary>
            /// 添加组件(不存在则添加)
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            private T AddComponent<T>() where T : Component
            {
                T t = gameObject.GetComponent<T>();
                if (null == t)
                {
                    t = gameObject.AddComponent<T>();
                }
                return t;
            }

            /// <summary>
            /// 得到参数
            /// </summary>
            /// <param name="args"></param>
            /// <returns></returns>
            private object[] GetArgs(params object[] args)
            {
                object[] array = new object[args.Length + 1];
                array[0] = m_table;
                Array.Copy(args, 0, array, 1, args.Length);
                return array;
            }

            /// <summary>
            /// 只加载一次
            /// </summary>
            public virtual void OnLoad()
            {
                m_canvas = AddComponent<Canvas>();
                m_graphicRaycaster = AddComponent<GraphicRaycaster>();

                m_table = GetScriptTable();

                LuaFunction onLoad = m_table.GetInPath<LuaFunction>("OnLoad");
                onLoad.Call(GetArgs());
                onLoad = null;

                m_onShow = m_table.GetInPath<LuaFunction>("OnShow");
                m_onUpdate = m_table.GetInPath<LuaFunction>("OnUpdate");
                m_onHide = m_table.GetInPath<LuaFunction>("OnHide");
                m_onUnload = m_table.GetInPath<LuaFunction>("OnUnload");
                m_table.GetInPath<LuaFunction>("OnAwake").Call(GetArgs());
            }

            /// <summary>
            /// 显示UI
            /// </summary>
            public virtual void OnShow(params object[] args)
            {
                m_uiState = UIState.Show;
                if (m_showOrHideMode == UIShowOrHideMode.Layer)
                {
                    gameObject.layer = (int)Layers.UI;
                }
                else
                {
                    gameObject.SetActive(true);
                }

                m_onShow.Call(GetArgs(args));
            }

            /// <summary>
            /// 更新UI
            /// </summary>
            /// <param name="args"></param>
            public virtual void OnUpdate(params object[] args)
            {
                m_onUpdate.Call(GetArgs(args));
            }

            /// <summary>
            /// 隐藏UI
            /// </summary>
            public virtual void OnHide()
            {
                m_uiState = UIState.Hide;
                if (m_showOrHideMode == UIShowOrHideMode.Layer)
                {
                    gameObject.layer = (int)Layers.HIDE;
                }
                else
                {
                    gameObject.SetActive(false);
                }

                m_onHide.Call(GetArgs());
            }

            /// <summary>
            /// 卸载销毁
            /// </summary>
            public virtual void OnUnload()
            {
                m_onUnload.Call(GetArgs());

                m_onShow = null;
                m_onUpdate = null;
                m_onHide = null;
                m_onUnload = null;
            }

            /// <summary>
            /// 设置UI顺序
            /// </summary>
            /// <param name="sortingOrder"></param>
            public void SetSortingOrder(int sortingOrder)
            {
                m_canvas.overrideSorting = true;
                if (m_sortOrder > 0)
                {
                    sortingOrder = m_sortOrder;
                }
                m_canvas.sortingOrder = sortingOrder;

                OnSortingOrderChange(sortingOrder);
            }


            /// <summary>
            /// 排序顺序改变
            /// </summary>
            /// <param name="newOrder"></param>
            public void OnSortingOrderChange(int newOrder)
            {
                for (int i = 0; i < m_sorting.Count; ++i)
                {
                    m_sorting[i].SetSortingOrder(newOrder);
                }
            }

            /// <summary>
            /// 添加一个排序顺序
            /// </summary>
            /// <param name="order"></param>
            public void AddSortingOrder(UISortingOrder order)
            {
                if (!m_sorting.Contains(order))
                {
                    m_sorting.Add(order);
                }
            }
            #endregion
        }
    }
}