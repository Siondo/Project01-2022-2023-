using UnityEngine;
using System.Collections.Generic;

namespace Framework
{
    namespace UnityAsset
    {
        public abstract class UnityAsyncAsset : AsyncOperation
        {
            /// <summary>
            /// 状态
            /// </summary>
            public enum LoadState
            {
                None = 0,
                Wait = 1,
                Loading = 2,
                Complete = 3,
                FalseUnload = 4,
                TrueUnload = 5,
            }

            #region Variable
            /// <summary>
            /// url
            /// </summary>
            protected string m_url = string.Empty;

            /// <summary>
            /// 加载状态
            /// </summary>
            protected LoadState m_loadState = LoadState.None;

            /// <summary>
            /// 进度
            /// </summary>
            protected float m_progress = 0f;

            /// <summary>
            /// 依赖资源列表
            /// </summary>
            protected List<UnityAsyncAsset> m_dependent = null;

            /// <summary>
            /// 被引用的资源列表
            /// </summary>
            protected List<UnityAsyncAsset> m_reference = null;

            /// <summary>
            /// 使用的对象表
            /// </summary>
            protected List<object> m_refObj = null;
            #endregion

            #region Property
            /// <summary>
            /// 得到url
            /// </summary>
            /// <value>The URL Path.</value>
            public string url
            {
                get { return m_url; }
            }

            /// <summary>
            /// 是否完成
            /// </summary>
            /// <value><c>true</c> if is done; otherwise, <c>false</c>.</value>
            public virtual bool isDone
            {
                get { return false; }
            }

            /// <summary>
            /// 进度
            /// </summary>
            /// <value>The progress.</value>
            public virtual float progress
            {
                get { return 0f; }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public virtual string error
            {
                get { return string.Empty; }
            }

            /// <summary>
            /// 文本
            /// </summary>
            public virtual string text
            {
                get { return string.Empty; }
            }

            /// <summary>
            /// 字节
            /// </summary>
            public virtual byte[] bytes
            {
                get { return new byte[0]; }
            }


            /// <summary>
            /// 加载状态
            /// </summary>
            /// <value>The state of the load.</value>
            public LoadState loadState
            {
                get { return m_loadState; }

            }

            /// <summary>
            /// 依赖引用计数
            /// </summary>
            public int referenceCount
            {
                get { return m_reference.Count; }
            }


            /// <summary>
            /// 依赖数
            /// </summary>
            public int dependentCount
            {
                get { return m_dependent.Count; }
            }

            /// <summary>
            /// 使用引用计数
            /// </summary>
            public int refObjCount
            {
                get { return m_refObj.Count; }
            }

            /// <summary>
            /// 得到所依赖的列表
            /// </summary>
            /// <returns></returns>
            public List<UnityAsyncAsset> GetDependentList()
            {
                return m_dependent;
            }

            /// <summary>
            /// 得到所有引用列表
            /// </summary>
            /// <returns></returns>
            public List<UnityAsyncAsset> GetReferenceList()
            {
                return m_reference;
            }

            /// <summary>
            /// 得到引用对象列表
            /// </summary>
            /// <returns></returns>
            public List<object> GetRefObjList()
            {
                return m_refObj;
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public UnityAsyncAsset(string url)
            {
                m_url = url;
                m_loadState = LoadState.Wait;
                m_dependent = new List<UnityAsyncAsset>(1 << 3);
                m_reference = new List<UnityAsyncAsset>(1 << 2);
                m_refObj = new List<object>(1 << 2);
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            public virtual void AsyncLoad()
            {
                m_loadState = LoadState.Loading;
            }

            /// <summary>
            /// 异步加载完成
            /// </summary>
            public virtual void Complete()
            {
                m_loadState = LoadState.Complete;
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public abstract AssetBundleRequest LoadAssetAsync(string name);

            /// <summary>
            /// 异步加载资源完成
            /// </summary>
            public virtual void LoadAssetAsyncComplete(string name, AssetBundleRequest assetBundleRequest) { }

            /// <summary>
            /// 加载所有的资源
            /// </summary>
            /// <returns></returns>
            public virtual Object[] LoadAllAssets() { return new Object[0]; }

            /// <summary>
            /// 得到资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public abstract Object GetAsset(string name);

            /// <summary>
            /// 添加依赖
            /// </summary>
            public virtual void AddDependent(UnityAsyncAsset unityAsyncAsset)
            {
                m_dependent.Add(unityAsyncAsset);
            }

            /// <summary>
            /// 添加引用
            /// </summary>
            public virtual void AddReference(UnityAsyncAsset unityAsyncAsset)
            {
                m_reference.Add(unityAsyncAsset);
            }

            /// <summary>
            /// 移除引用
            /// </summary>
            public virtual void RemoveReference(UnityAsyncAsset unityAsyncAsset)
            {
                m_reference.Remove(unityAsyncAsset);
            }

            /// <summary>
            /// 添加到使用对象表
            /// </summary>
            /// <param name="refObj"></param>
            public virtual void AddRefObj(object refObj)
            {
                m_refObj.Add(refObj);
            }

            /// <summary>
            /// 从使用对象表移除
            /// </summary>
            public virtual void RemoveRefObj(object refObj)
            {
                m_refObj.Remove(refObj);
            }

            /// <summary>
            /// 卸载资源
            /// </summary>
            public virtual void Unload(bool unloadAllLoadedObjects = true)
            {
                m_loadState = unloadAllLoadedObjects ? LoadState.TrueUnload : LoadState.FalseUnload;
            }
            #endregion
        }
    }
}