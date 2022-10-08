using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using XLua;

namespace Framework
{
    namespace UI
    {
        [System.Serializable]
        public class ReferenceComponent
        {
            /// <summary>
            /// 使用的组件列表
            /// </summary>
            private readonly static Dictionary<string, string> g_CGroup = new Dictionary<string, string>() {
                {typeof(GameObject).Name, "Go" },
                {typeof(Transform).Name, "Tf" },
                {typeof(RectTransform).Name, "RectTf" },
                {typeof(ImageEx).Name, "ImgEx" },
                {typeof(SpriteRendererEx).Name, "SRdrEx" },
                {typeof(TextEx).Name, "TxtEx" },
                {typeof(ButtonEx).Name, "BtnEx" },
                {typeof(SliderEx).Name, "SldEx" },
                {typeof(ToggleEx).Name, "ToggleEx" },
                {typeof(InputFieldEx).Name, "InputEx" },
                {typeof(ScrollRectEx).Name, "ScrollEx" },
                {typeof(UIToLua).Name, "Script" },
                {typeof(UIReference).Name, "Ref" },
                {typeof(UIPenetrateEvent).Name, "PenEvent" },
                {typeof(GestureProcessorMono).Name, "Ges" },
            };

            [SerializeField]
            private GameObject m_target = null;


            [SerializeField]
            private List<Object> m_list = new List<Object>();

            /// <summary>
            /// 组件对象表
            /// </summary>
            public List<Object> list => m_list;

            /// <summary>
            /// 是否包含组件名
            /// </summary>
            /// <param name="componentName"></param>
            /// <returns></returns>
            public static bool Contains(string componentName)
            {
                return g_CGroup.ContainsKey(componentName);
            }

            /// <summary>
            /// 得到名字
            /// </summary>
            /// <param name="componentName"></param>
            /// <returns></returns>
            public static string GetName(Object @object)
            {
                return @object.name.Substring(1, @object.name.Length - 1) + g_CGroup[@object.GetType().Name];
            }

            /// <summary>
            /// 目标对象
            /// </summary>
            public GameObject target => m_target;

            /// <summary>
            /// 设置目标
            /// </summary>
            /// <param name="obj"></param>
            public void SetTarget(GameObject obj)
            {
                m_target = obj;
            }

            /// <summary>
            /// 设置
            /// </summary>
            /// <param name="obj"></param>
            public bool ContainsObj(Object obj)
            {
                return m_list.Contains(obj);
            }

            /// <summary>
            /// 添加对象
            /// </summary>
            /// <param name="obj"></param>
            public void AddObj(Object obj)
            {
                m_list.Add(obj);
            }

            /// <summary>
            /// 移除对象
            /// </summary>
            /// <param name="obj"></param>
            public void RemoveObj(Object obj)
            {
                m_list.Remove(obj);
            }
        }

        [System.Serializable]
        public class ReferenceParam
        {
            /// <summary>
            /// 定义参数类型
            /// </summary>
            public enum ParamType
            {
                Number = 0,
                String = 1,
                Boolean = 2,
                Color = 3,
            }

            [SerializeField]
            private GameObject m_target = null;

            /// <summary>
            /// 参数名字
            /// </summary>
            [SerializeField]
            private string m_name = string.Empty;

            /// <summary>
            /// 参数名字
            /// </summary>
            [SerializeField]
            private ParamType m_type = ParamType.Number;

            [SerializeField] private double m_double = 0;
            [SerializeField] private string m_string = string.Empty;
            [SerializeField] private bool m_bool = false;
            [SerializeField] private Color m_color = Color.white;

            public double d { get { return m_double; } set { m_double = value; } }
            public string s { get { return m_string; } set { m_string = value; } }
            public bool b { get { return m_bool; } set { m_bool = value; } }
            public Color c { get { return m_color; } set { m_color = value; } }

            /// <summary>
            /// 得到名字
            /// </summary>
            /// <returns></returns>
            public string paramName
            {
                get { return m_name; }
                set { m_name = value; }
            }

            /// <summary>
            /// 数据类型
            /// </summary>
            public ParamType paramType
            {
                get { return m_type; }
                set { m_type = value; }
            }
        }

        /// <summary>
        /// UIBase
        /// </summary>
        [System.Serializable]
        public class UIReference : MonoBehaviour
        {
            /// <summary>
            /// 需要操作的GameObject列表
            /// </summary>
            [Header("需要操作的GameObject列表")]
            [SerializeField] private List<ReferenceComponent> m_data = new List<ReferenceComponent>();

            /// <summary>
            /// 需要操作的GameObject列表
            /// </summary>
            [Header("需要操作的Param列表")]
            [SerializeField] private List<ReferenceParam> m_paramData = new List<ReferenceParam>();

            /// <summary>
            /// UI引用表
            /// </summary>
            private LuaTable m_uiRef = null;

            /// <summary>
            /// 列表
            /// </summary>
            public List<ReferenceComponent> data => m_data;

            /// <summary>
            /// 参数数据
            /// </summary>
            public List<ReferenceParam> paramData => m_paramData;

            /// <summary>
            /// 得到引用Table
            /// </summary>
            /// <returns></returns>
            public LuaTable GetReferenceTable()
            {
                if (m_uiRef == null)
                {
                    m_uiRef = Lua.instance.NewTable();
                    m_uiRef.SetInPath("gameObject", gameObject);
                    m_uiRef.SetInPath("transform", transform);
                    m_uiRef.SetInPath("rectTransform", transform as RectTransform);
                    UIToLua uIToLua = null;
                    foreach (var data in m_data)
                    {
                        for (int i = 0; i < data.list.Count; ++i)
                        {
                            if (null == data.list[i])
                            {
                                continue;
                            }
                            uIToLua = data.list[i] as UIToLua;
                            if (null == uIToLua)
                            {
                                m_uiRef.SetInPath(ReferenceComponent.GetName(data.list[i]), data.list[i]);
                            }
                            else
                            {
                                m_uiRef.SetInPath(ReferenceComponent.GetName(data.list[i]), uIToLua.GetScriptTable());
                            }
                        }
                    }

                    LuaTable paramTable = Lua.instance.NewTable();
                    foreach (var data in m_paramData)
                    {
                        if (string.IsNullOrEmpty(data.paramName))
                        {
                            continue;
                        }
                        if (data.paramType == ReferenceParam.ParamType.Number)
                        {
                            paramTable.SetInPath(data.paramName, data.d);
                        }
                        else if (data.paramType == ReferenceParam.ParamType.String)
                        {
                            paramTable.SetInPath(data.paramName, data.s);
                        }
                        else if (data.paramType == ReferenceParam.ParamType.Boolean)
                        {
                            paramTable.SetInPath(data.paramName, data.b);
                        }
                        else if (data.paramType == ReferenceParam.ParamType.Color)
                        {
                            paramTable.SetInPath(data.paramName, data.c);
                        }
                    }
                    m_uiRef.SetInPath<LuaTable>("Const", paramTable);
                }
                return m_uiRef;
            }

            /// <summary>
            /// 启动
            /// </summary>
            protected virtual void Awake() { }

            /// <summary>
            /// 开始
            /// </summary>
            protected virtual void Start() { }

            /// <summary>
            /// 销毁
            /// </summary>
            protected virtual void OnDestroy() { }
        }
    }
}