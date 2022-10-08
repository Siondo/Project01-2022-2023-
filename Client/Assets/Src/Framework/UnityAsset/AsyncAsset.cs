using UnityEngine;

namespace Framework
{
    using Event;

    namespace UnityAsset
    {
        /// <summary>
        /// 异步资源包
        /// </summary>
        public class AsyncAsset
        {
            #region Variable
            /// <summary>
            /// 资源名字(原始资源路径)
            /// </summary>
            private string m_assetName = string.Empty;

            /// <summary>
            /// 异步资源
            /// </summary>
            private UnityAsyncAsset m_unityAsyncAsset = null;

            /// <summary>
            /// 完成事件
            /// </summary>
            private System.Action<bool, AsyncAsset> m_action = null;

            /// <summary>
            /// 默认是需要加载资源的
            /// </summary>
            private bool m_needLoad = true;

            /// <summary>
            /// ab资源异步加载请求
            /// </summary>
            private AssetBundleRequest m_assetBundleRequest = null;

            /// <summary>
            /// 卸载完成
            /// </summary>
            private bool m_unloadComplete = false;
            #endregion

            #region Property
            /// <summary>
            /// 资源名字
            /// </summary>
            public string assetName
            {
                get { return m_assetName; }
            }

            /// <summary>
            /// 资源Url地址
            /// </summary>
            public string url
            {
                get { return m_unityAsyncAsset.url; }
            }

            /// <summary>
            /// 是否完成
            /// </summary>
            public bool isDone
            {
                get
                {
                    if (m_needLoad && m_unityAsyncAsset.isDone)
                    {
                        m_needLoad = false;
                        if (string.IsNullOrEmpty(m_unityAsyncAsset.error))
                        {
                            m_assetBundleRequest = m_unityAsyncAsset.LoadAssetAsync(assetName);
                            if (m_assetBundleRequest != null)
                            {
                                var asset = m_assetBundleRequest.asset;
                            }
                        }
                    }

                    if (null != m_assetBundleRequest)
                    {
                        return m_assetBundleRequest.isDone;
                    }
                    else
                    {
                        return m_unityAsyncAsset.isDone;
                    }
                }
            }

            /// <summary>
            /// 加载进度
            /// </summary>
            public float progress
            {
                get
                {
                    if (null != m_assetBundleRequest)
                    {
                        return 0.5f * (m_unityAsyncAsset.progress + m_assetBundleRequest.progress);
                    }
                    else
                    {
                        return m_unityAsyncAsset.progress * (m_needLoad ? 0.5f : 1f);
                    }
                }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public string error
            {
                get
                {
                    return m_unityAsyncAsset.error;
                }
            }

            /// <summary>
            /// 得到文本
            /// </summary>
            public string text
            {
                get
                {
                    if (string.IsNullOrEmpty(m_unityAsyncAsset.text))
                    {
                        TextAsset t = mainAsset as TextAsset;
                        return t != null ? t.text : m_unityAsyncAsset.text;
                    }
                    return m_unityAsyncAsset.text;
                }
            }

            /// <summary>
            /// 得到字节
            /// </summary>
            public byte[] bytes
            {
                get 
                {
                    if (string.IsNullOrEmpty(m_unityAsyncAsset.text))
                    {
                        TextAsset t = mainAsset as TextAsset;
                        return t != null ? t.bytes : m_unityAsyncAsset.bytes;
                    }
                    return m_unityAsyncAsset.bytes; 
                }
            }

            /// <summary>
            /// 得到资源
            /// </summary>
            public Object mainAsset
            {
                get
                {
                    return m_unityAsyncAsset.GetAsset(assetName);
                }
            }

            /// <summary>
            /// 用户数据
            /// </summary>
            public object userData
            {
                get; set;
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="unityAsyncAsset"></param>
            /// <param name="action"></param>
            public AsyncAsset(string assetName, UnityAsyncAsset unityAsyncAsset, System.Action<bool, AsyncAsset> action)
            {
                m_assetName = assetName;
                m_unityAsyncAsset = unityAsyncAsset;
                m_action = action;
                m_needLoad = m_unityAsyncAsset.GetType().Name == typeof(BundleAsset).Name;
                m_assetBundleRequest = null;

                m_unityAsyncAsset.AddRefObj(this);
            }

            /// <summary>
            /// 异步加载完成
            /// </summary>
            public void Complete()
            {
                if (null != m_unityAsyncAsset)
                {
                    m_unityAsyncAsset.LoadAssetAsyncComplete(assetName, m_assetBundleRequest);
                }
                m_action?.Invoke(string.IsNullOrEmpty(error), this);
            }

            /// <summary>
            /// 卸载
            /// </summary>
            /// <param name="unloadAllLoadedObjects"></param>
            public void Unload(bool unloadAllLoadedObjects = true)
            {
                if (m_unloadComplete) { return; }

                m_unloadComplete = true;
                if (null != m_unityAsyncAsset)
                {
                    m_unityAsyncAsset.RemoveRefObj(this);
                    m_unityAsyncAsset.Unload(unloadAllLoadedObjects);
                }
            }

            /// <summary>
            /// 加载所有资源
            /// </summary>
            /// <returns></returns>
            public Object[] LoadAllAssets()
            {
                if (null != m_unityAsyncAsset)
                {
                    return m_unityAsyncAsset.LoadAllAssets();
                }
                else
                {
                    return new Object[0];
                }
            }

            /// <summary>
            /// 实例化对象
            /// </summary>
            /// <returns></returns>
            public GameObject Instantiate(string name = null)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return GameObject.Instantiate(mainAsset) as GameObject;
                }
                else
                {
                    GameObject go = GameObject.Instantiate(mainAsset) as GameObject;
                    go.name = name;
                    return go;
                }
            }

            /// <summary>
            /// 销毁对象
            /// </summary>
            /// <param name="go"></param>
            public void Destroy(GameObject go)
            {
                GameObject.Destroy(go);
            }
            #endregion
        }
    }
}