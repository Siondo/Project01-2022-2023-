using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    using Singleton;
    using UnityAsset;
    namespace Pool
    {
        public class AssetPool : MonoBehaviourSingleton<AssetPool>
        {
            private struct PoolObj
            {
                public GameObject Obj;
                public float Time;
            }

            private class AB
            {
                private string m_name = string.Empty;
                private bool m_loadComplete = false;
                private AsyncAsset m_asyncAsset = null;
                private Action<bool, AsyncAsset> m_aaAction = null;
                private List<Action<GameObject>> m_goAction = null;
                private List<PoolObj> m_pool = new List<PoolObj>(1 << 4);
                private Action<UnityEngine.Object> m_objAction = null;
                private UnityEngine.Object m_obj = null;

                /// <summary>
                /// 资源名字
                /// </summary>
                public string name
                {
                    get
                    {
                        return m_name;
                    }
                }

                /// <summary>
                /// 加载是否完成
                /// </summary>
                public bool loadComplete
                {
                    get
                    {
                        return m_loadComplete;
                    }
                    set
                    {
                        m_loadComplete = value;
                    }
                }

                /// <summary>
                /// 异步资源
                /// </summary>
                public AsyncAsset asyncAsset
                {
                    get
                    {
                        return m_asyncAsset;
                    }
                    set
                    {
                        m_asyncAsset = value;
                    }
                }

                /// <summary>
                /// 加载完成事件通知
                /// </summary>
                public Action<bool, AsyncAsset> aaAction
                {
                    get
                    {
                        return m_aaAction;
                    }
                    set
                    {
                        m_aaAction = value;
                    }
                }

                /// <summary>
                /// 完成事件对象通知
                /// </summary>
                public List<Action<GameObject>> goAction
                {
                    get
                    {
                        return m_goAction;
                    }
                    set
                    {
                        m_goAction = value;
                    }
                }

                /// <summary>
                /// 完成事件对象通知
                /// </summary>
                public Action<UnityEngine.Object> objAction
                {
                    get
                    {
                        return m_objAction;
                    }
                    set
                    {
                        m_objAction = value;
                    }
                }

                /// <summary>
                /// 加载的对象
                /// </summary>
                public UnityEngine.Object obj
                {
                    get
                    {
                        return m_obj;
                    }
                    set
                    {
                        m_obj = value;
                    }
                }

                /// <summary>
                /// 带参构造
                /// </summary>
                /// <param name="name"></param>
                public AB(string name)
                {
                    this.m_name = name;
                    this.m_goAction = new List<Action<GameObject>>(16);
                }

                /// <summary>
                /// 取出对象
                /// </summary>
                /// <returns></returns>
                public GameObject Dequeue()
                {
                    int pos = m_pool.Count - 1;
                    if (pos >= 0)
                    {
                        PoolObj obj = m_pool[pos];
                        m_pool.RemoveAt(pos);
                        return obj.Obj;
                    }
                    else
                    {
                        return asyncAsset.Instantiate(name);
                    }
                }

                /// <summary>
                /// 返还对象
                /// </summary>
                /// <param name="item"></param>
                public void Enqueue(GameObject item)
                {
                    item.transform.SetParent(AssetPool.instance.transform);
                    for (int i = 0; i < m_pool.Count; ++i)
                    {
                        if (m_pool[i].Obj == item)
                        {
                            Debug.LogWarningFormat("Repeat unload {0}", item.name);
                            return;
                        }
                    }
                    PoolObj obj;
                    obj.Obj = item;
                    obj.Time = Time.time;
                    m_pool.Add(obj);
                }

                /// <summary>
                /// GC回收
                /// </summary>
                public void GC()
                {
                    for (int i = m_pool.Count - 1; i >= 0; --i)
                    {
                        if (Time.time > m_pool[i].Time + Const.Pool_GC_TIME)
                        {
                            asyncAsset.Destroy(m_pool[i].Obj);
                            m_pool.RemoveAt(i);
                        }
                    }
                }
            }

            /// <summary>
            /// AB资源池
            /// </summary>
            private Dictionary<string, AB> m_pool;

            /// <summary>
            /// 构造
            /// </summary>
            public AssetPool()
            {
                m_pool = new Dictionary<string, AB>();
            }

            protected override void Start() {
                gameObject.SetActive(false);
                Event.Schedule.instance.ScheduleUpdate(GC, 10, -1);
            }

            /// <summary>
            /// 池中加载，返回对象
            /// </summary>
            /// <param name="name"></param>
            /// <param name="complete"></param>
            /// <param name="async"></param>
            public void LoadFromPool(string name, Action<GameObject> complete, bool async = true)
            {
                AB ab = null;
                if (m_pool.ContainsKey(name))
                {
                    ab = m_pool[name];
                }
                else
                {
                    ab = new AB(name);
                    m_pool.Add(name, ab);
                }

                if (ab.loadComplete)
                {
                    complete?.Invoke(ab.Dequeue());
                }
                else
                {
                    ab.goAction.Add(complete);
                    if (null == ab.asyncAsset)
                    {
                        ab.asyncAsset = AssetManager.instance.LoadAsset(name, (result, asyncAsset) =>
                        {
                            ab.loadComplete = true;
                            ab.asyncAsset = asyncAsset;

                            if (result)
                            {
                                foreach (var action in ab.goAction)
                                {
                                    action?.Invoke(ab.Dequeue());
                                }
                            }
                            else
                            {
                                Debug.LogError("LoadFromPool Error: " + name);
                                foreach (var action in ab.goAction)
                                {
                                    action?.Invoke(null);
                                }
                            }
                            ab.goAction.Clear();
                        }, async, App.abMode);
                    }
                }
            }

            /// <summary>
            /// 卸载资源到池
            /// </summary>
            /// <param name="go"></param>
            public void UnloadToPool(GameObject go, string name = null)
            {
                if (null == go)
                {
                    return;
                }
                if (string.IsNullOrEmpty(name))
                {
                    name = go.name;
                }
                if (null != go && m_pool.ContainsKey(name))
                {
                    AB ab = m_pool[name];
                    go.name = name;
                    ab.Enqueue(go);
                }
            }

            /// <summary>
            /// 池中加载，返回异步资源
            /// </summary>
            /// <param name="name"></param>
            /// <param name="complete"></param>
            /// <param name="async"></param>
            public AsyncAsset LoadAssetFromPool(string name, Action<bool, AsyncAsset> complete, bool async = true)
            {
                AB ab = null;
                if (m_pool.ContainsKey(name))
                {
                    ab = m_pool[name];
                }
                else
                {
                    ab = new AB(name);
                    m_pool.Add(name, ab);
                }

                if (ab.loadComplete)
                {
                    complete?.Invoke(!string.IsNullOrWhiteSpace(ab.asyncAsset.error), ab.asyncAsset);
                }
                else
                {
                    ab.aaAction += complete;
                    if (null == ab.asyncAsset)
                    {
                        ab.asyncAsset = AssetManager.instance.LoadAsset(name, (result, asyncAsset) =>
                        {
                            ab.loadComplete = true;
                            ab.asyncAsset = asyncAsset;

                            if (result)
                            {
                                ab?.aaAction(result, ab.asyncAsset);
                            }
                            else
                            {
                                Debug.LogError("LoadAssetFromPool Error: " + name);
                                ab?.aaAction(result, null);
                            }
                        }, async, App.abMode);
                    }
                }

                return ab.asyncAsset;
            }

            /// <summary>
            /// 池中加载，返回对象
            /// </summary>
            /// <param name="name"></param>
            /// <param name="complete"></param>
            /// <param name="async"></param>
            public void LoadObjectFromPool(string name, Action<UnityEngine.Object> complete, bool async = true)
            {
                AB ab = null;
                if (m_pool.ContainsKey(name))
                {
                    ab = m_pool[name];
                }
                else
                {
                    ab = new AB(name);
                    m_pool.Add(name, ab);
                }

                if (ab.loadComplete)
                {
                    complete?.Invoke(ab.obj);
                }
                else
                {
                    ab.objAction += complete;
                    if (null == ab.asyncAsset)
                    {
                        ab.asyncAsset = AssetManager.instance.LoadAsset(name, (result, asyncAsset) =>
                        {
                            ab.loadComplete = true;
                            ab.asyncAsset = asyncAsset;
                            ab.obj = asyncAsset.mainAsset;

                            if (result)
                            {
                                ab?.objAction(ab.obj);
                            }
                            else
                            {
                                Debug.LogError("LoadObjectFromPool Error: " + name);
                                ab?.objAction(null);
                            }
                        }, async, App.abMode);
                    }
                }
            }

            /// <summary>
            /// 开启GC检测
            /// </summary>
            private void GC()
            {
                foreach (var ab in m_pool.Values)
                {
                    ab.GC();
                }
            }
        }
    }
}