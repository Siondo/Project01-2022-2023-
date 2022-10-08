using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    using Event;
    using Singleton;

    namespace UnityAsset
    {
        public sealed class AssetManager : MonoBehaviourSingleton<AssetManager>
        {
            #region Variable
            /// <summary>
            /// 记录已加载完成的所有异步资源
            /// </summary>
            Dictionary<string, UnityAsyncAsset> m_complete = null;

            /// <summary>
            /// 记录正在加载中的异步资源
            /// </summary>
            Dictionary<string, UnityAsyncAsset> m_loading = null;

            /// <summary>
            /// 缓存表
            /// </summary>
            List<UnityAsyncAsset> m_temp = null;

            /// <summary>
            /// 记录异步加载完成
            /// </summary>
            List<AsyncAsset> m_asyncComplete = null;

            /// <summary>
            /// 记录异步加载中
            /// </summary>
            List<AsyncAsset> m_asyncLoading = null;

            /// <summary>
            /// 是否允许加载
            /// </summary>
            bool m_isAllowLoad = true;

            /// <summary>
            /// 最大同时加载个数
            /// </summary>
            int m_maxLoader = 1;

            /// <summary>
            /// 当前最大加载数
            /// </summary>
            int m_currentMaxLoader = 0;
            #endregion

            #region Property
            /// <summary>
            /// 得到或设置是否允许加载
            /// </summary>
            public bool isAllowload
            {
                get { return m_isAllowLoad; }
                set { m_isAllowLoad = value; }
            }

            /// <summary>
            /// 得到或设置最大同时加载数
            /// </summary>
            /// <value>The max loader.</value>
            public int maxLoader
            {
                get { return m_maxLoader; }
                set { m_maxLoader = value; }
            }

            /// <summary>
            /// 得到所有完成的资源信息
            /// </summary>
            /// <returns></returns>
            public Dictionary<string, UnityAsyncAsset> GetAllComplete()
            {
                return m_complete;
            }

            /// <summary>
            /// 得到正在加载的资源信息
            /// </summary>
            /// <returns></returns>
            public Dictionary<string, UnityAsyncAsset> GetAllLoading()
            {
                return m_loading;
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public AssetManager()
            {
                m_complete = new Dictionary<string, UnityAsyncAsset>(1 << 7);
                m_loading = new Dictionary<string, UnityAsyncAsset>(1 << 4);
                m_temp = new List<UnityAsyncAsset>(m_loading.Count);
                m_asyncComplete = new List<AsyncAsset>(1 << 5);
                m_asyncLoading = new List<AsyncAsset>(1 << 3);
            }

            /// <summary>
            /// Resources加载(无需后缀名)
            /// </summary>
            /// <param name="path">Path.</param>
            public Object ResourceLoad(string path)
            {
                return Resources.Load(path);
            }

            /// <summary>
            /// Resources加载(无需后缀名)
            /// </summary>
            /// <param name="path">Path.</param>
            /// <typeparam name="T">The 1st type parameter.</typeparam>
            public T ResourceLoad<T>(string path) where T : Object
            {
                return Resources.Load<T>(path);
            }

            /// <summary>
            /// Resources加载(无需后缀名)
            /// </summary>
            /// <param name="path">Path.</param>
            /// <param name="type">Type.</param>
            public Object ResourceLoad(string path, System.Type type)
            {
                return Resources.Load(path, type);
            }

            /// <summary>
            /// 加载依赖
            /// </summary>
            /// <param name="path"></param>
            /// <param name="asyncLoad"></param>
            private UnityAsyncAsset LoadDependent(string path, bool asyncLoad = true)
            {
                path = GetBundlePath(path);

                UnityAsyncAsset async = null;
                if (m_complete.ContainsKey(path))
                {
                    async = m_complete[path];
                }
                else
                {
                    if (m_loading.ContainsKey(path))
                    {
                        async = m_loading[path];
                    }
                    else
                    {
                        async = new BundleAsset(path);
                        var data = GetDependencies(path);
                        UnityAsyncAsset temp = null;
                        for (int i = 0; i < data.Count; ++i)
                        {
                            if (path.Equals(data[i]))
                            {
                                Debug.LogError("LoadDependent Loop: " + path);
                                continue;
                            }
                            temp = LoadDependent(data[i], asyncLoad);
                            temp.AddReference(async);
                            async.AddDependent(temp);
                        }
                        m_loading.Add(async.url, async);
                        async.AsyncLoad();
                    }
                    if (!asyncLoad)
                    {
                        while (!async.isDone) { }
                        m_loading.Remove(async.url);
                        m_complete.Add(async.url, async);
                        async.Complete();
                    }
                }
                return async;
            }

            /// <summary>
            /// AssetBundle加载
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public AsyncAsset AssetBundleLoad(string path)
            {
                UnityAsyncAsset async = LoadDependent(path, false);

                AsyncAsset asyncAsset = new AsyncAsset(path, async, null);
                while (!asyncAsset.isDone) { }
                m_asyncComplete.Add(asyncAsset);
                asyncAsset.Complete();
                return asyncAsset;
            }

            /// <summary>
            /// AssetBundle异步加载
            /// </summary>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <returns></returns>
            public AsyncAsset AssetBundleAsyncLoad(string path, System.Action<bool, AsyncAsset> complete)
            {
                UnityAsyncAsset async = LoadDependent(path, true);

                AsyncAsset asyncAsset = new AsyncAsset(path, async, complete);
                m_asyncLoading.Add(asyncAsset);
                return asyncAsset;
            }

            /// <summary>
            /// 加载FileAsset
            /// </summary>
            /// <param name="path"></param>
            /// <param name="asyncLoad"></param>
            /// <returns></returns>
            private UnityAsyncAsset LoadFileAsset(string path, bool asyncLoad = true)
            {
                path = GetBundlePath(path);

                UnityAsyncAsset async = null;
                if (m_complete.ContainsKey(path))
                {
                    async = m_complete[path];
                }
                else
                {
                    if (m_loading.ContainsKey(path))
                    {
                        async = m_loading[path];
                    }
                    else
                    {
                        async = new FileAsset(path);
                        async.AsyncLoad();
                    }
                    if (!asyncLoad)
                    {
                        while (!async.isDone) { }
                        m_complete.Add(async.url, async);
                        async.Complete();
                    }
                }
                return async;
            }

            /// <summary>
            /// FileAsset加载
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public AsyncAsset FileAssetLoad(string path)
            {
                UnityAsyncAsset async = LoadFileAsset(path, false);

                AsyncAsset asyncAsset = new AsyncAsset(path, async, null);
                asyncAsset.Complete();
                m_asyncComplete.Add(asyncAsset);
                return asyncAsset;
            }

            /// <summary>
            /// FileAsset异步加载
            /// </summary>
            /// <param name="path"></param>
            /// <param name="complete"></param>
            /// <returns></returns>
            public AsyncAsset FileAssetAsyncLoad(string path, System.Action<bool, AsyncAsset> complete)
            {
                UnityAsyncAsset async = LoadFileAsset(path, true);

                AsyncAsset asyncAsset = new AsyncAsset(path, async, complete);
                m_asyncLoading.Add(asyncAsset);
                return asyncAsset;
            }

            /// <summary>
            /// 更新
            /// </summary>
            public void Update()
            {
                if (m_isAllowLoad)
                {
                    // 正在加载中的处理
                    m_currentMaxLoader = Mathf.Min(m_maxLoader, m_loading.Count);
                    if (m_currentMaxLoader > 0)
                    {
                        m_temp.Clear();
                        m_temp.AddRange(m_loading.Values);
                        foreach (var data in m_temp)
                        {
                            switch (data.loadState)
                            {
                                case UnityAsyncAsset.LoadState.Wait:
                                    {
                                        data.AsyncLoad();
                                    }
                                    break;
                                case UnityAsyncAsset.LoadState.Loading:
                                    {
                                        if (data.isDone)
                                        {
                                            data.Complete();

                                            m_complete.Add(data.url, data);
                                            m_loading.Remove(data.url);
                                        }
                                    }
                                    break;
                            }
                            if (0 == --m_currentMaxLoader)
                            {
                                break;
                            }
                        }
                    }
                    // 记录加载处理
                    if (m_asyncLoading.Count > 0 && m_asyncLoading[0].isDone)
                    {
                        AsyncAsset asyncAsset = m_asyncLoading[0];
                        m_asyncLoading.RemoveAt(0);
                        m_asyncComplete.Add(asyncAsset);
                        asyncAsset.Complete();
                    }
                }
            }

            /// <summary>
            /// 卸载所有资源
            /// </summary>
            /// <param name="unloadAllLoadedObjects"></param>
            public void UnloadAsset(bool unloadAllLoadedObjects = true)
            {
                foreach (var asyn in m_asyncComplete)
                {
                    asyn.Unload(unloadAllLoadedObjects);
                }
                foreach (var asyn in m_asyncLoading)
                {
                    asyn.Unload(unloadAllLoadedObjects);
                }

                m_complete.Clear();
                m_loading.Clear();
                m_asyncComplete.Clear();
                m_asyncLoading.Clear();
            }

            /// <summary>
            /// 卸载指定资源
            /// </summary>
            /// <param name="asset"></param>
            /// <param name="unloadAllLoadedObjects"></param>
            public void UnloadAsset(AsyncAsset asset, bool unloadAllLoadedObjects)
            {
                if (null == asset) { return; }

                asset.Unload(unloadAllLoadedObjects);
                if (m_complete.ContainsKey(asset.url))
                {
                    m_complete.Remove(asset.url);
                }
                if (m_asyncComplete.Contains(asset))
                {
                    m_asyncComplete.Remove(asset);
                }
                else if (m_asyncLoading.Contains(asset))
                {
                    m_asyncLoading.Remove(asset);
                }
            }

            /// <summary>
            /// 得到依赖列表
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            private List<string> GetDependencies(string path)
            {
                return App.manifest.GetDependencies(path);
            }

            /// <summary>
            /// 得到捆绑资源的短路径
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            private string GetBundlePath(string path)
            {
                return App.manifestMapping.Get(path);
            }
            #endregion
        }
    }

}