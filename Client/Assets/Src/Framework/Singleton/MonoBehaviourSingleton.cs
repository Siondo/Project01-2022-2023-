using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace Singleton
    {
        /// <summary>
        /// 单例
        /// </summary>
        public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour, new()
        {
            /// <summary>
            /// 单例对象
            /// </summary>
            private static T g_instance = null;



            /// <summary>
            /// 是否有实例
            /// </summary>
            public static bool hasInstance => null != g_instance;

            #region Function
            /// <summary>
            /// 得到单例
            /// </summary>
            /// <value>The instance.</value>
            public static T instance
            {
                get
                {
                    if (null == g_instance)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        DontDestroyOnLoad(go);
                        g_instance = go.AddComponent<T>();
                    }
                    return g_instance;
                }
            }

            /// <summary>
            /// 加载脚本实例时调用 Awake
            /// </summary>
            protected virtual void Awake()
            {
                if (null == g_instance)
                {
                    GameObject go = gameObject;
                    go.name = typeof(T).Name;
                    DontDestroyOnLoad(go);
                    g_instance = go.GetComponent<T>();
                }
                Create();
            }

            protected virtual void Start() { }

            /// <summary>
            /// 创建数据
            /// </summary>
            protected virtual void Create() { Debug.Log(string.Format("单例 {0} 实例化", typeof(T).Name)); }

            /// <summary>
            /// 清理数据
            /// </summary>
            public virtual void Clear() { }
            #endregion
        }
    }
}