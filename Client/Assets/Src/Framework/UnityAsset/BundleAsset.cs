using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework
{
    using Event;
    namespace UnityAsset
    {
        public sealed class BundleAsset : UnityAsyncAsset
        {
            #region Variable
            /// <summary>
            /// ab创建请求器
            /// </summary>
            private AssetBundleCreateRequest m_assetBundleCreateRequest = null;

            /// <summary>
            /// 资源
            /// </summary>
            private Dictionary<string, Object> m_asset = null;
            #endregion

            #region Property
            /// <summary>
            /// 是否完成
            /// </summary>
            public override bool isDone
            {
                get 
                {
                    return m_assetBundleCreateRequest != null ? m_assetBundleCreateRequest.isDone : false;
                }
            }

            /// <summary>
            /// 进度
            /// </summary>
            /// <value>The progress.</value>
            public override float progress
            {
                get
                {
                    return m_assetBundleCreateRequest != null ? m_assetBundleCreateRequest.progress : 0f;
                }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public override string error
            {
                get
                {
                    return (m_assetBundleCreateRequest != null && m_assetBundleCreateRequest.assetBundle != null) ? string.Empty : "unknown error";
                }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public BundleAsset(string url)
                : base(url)
            {
                m_asset = new Dictionary<string, Object>();
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            public override void AsyncLoad()
            {
                base.AsyncLoad();
                m_assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(PathUtil.GetRealUrl(url));
                var bundle = m_assetBundleCreateRequest.assetBundle;
            }

            /// <summary>
            /// 异步加载完成
            /// </summary>
            public override void Complete()
            {
                base.Complete();
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override AssetBundleRequest LoadAssetAsync(string name)
            {
                AssetBundleRequest assetBundleRequest = null;
                if (string.IsNullOrEmpty(error))
                {
                    if (!m_asset.ContainsKey(name) && !m_assetBundleCreateRequest.assetBundle.isStreamedSceneAssetBundle)
                    {
                        name = "assets/" + name;
                        assetBundleRequest = m_assetBundleCreateRequest.assetBundle.LoadAssetAsync(name);
                    }
                }
                return assetBundleRequest;
            }

            /// <summary>
            /// 异步加载资源完成
            /// </summary>
            public override void LoadAssetAsyncComplete(string name, AssetBundleRequest assetBundleRequest)
            {
                var assetBundle = m_assetBundleCreateRequest.assetBundle;
                if (null != assetBundleRequest && !m_asset.ContainsKey(name))
                {
                    m_asset.Add(name, assetBundleRequest.asset);
                }
            }

            /// <summary>
            /// 加载所有资源
            /// </summary>
            /// <returns></returns>
            public override Object[] LoadAllAssets()
            {
                if (null != m_assetBundleCreateRequest && null != m_assetBundleCreateRequest.assetBundle)
                {
                    return m_assetBundleCreateRequest.assetBundle.LoadAllAssets();
                }
                return base.LoadAllAssets();
            }

            /// <summary>
            /// 得到资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override Object GetAsset(string name)
            {
                return m_asset.ContainsKey(name) ? m_asset[name] : null;
            }

            /// <summary>
            /// 卸载资源
            /// </summary>
            /// <param name="unloadAllLoadedObjects"></param>
            public override void Unload(bool unloadAllLoadedObjects = true)
            {
                if (LoadState.FalseUnload == loadState || LoadState.TrueUnload == loadState)
                {
                    return;
                }

                if (unloadAllLoadedObjects && (refObjCount > 0 || referenceCount > 0))
                {
                    return;
                }

                // 没有任何引用的话,记录依赖对象需要移除自己
                bool removeReference = (0 == refObjCount && 0 == referenceCount);

                // 自己卸载
                base.Unload(unloadAllLoadedObjects);
                if (null != m_assetBundleCreateRequest && null != m_assetBundleCreateRequest.assetBundle && !m_assetBundleCreateRequest.assetBundle.isStreamedSceneAssetBundle)
                {
                    m_assetBundleCreateRequest.assetBundle.Unload(unloadAllLoadedObjects);
                }

                // 通知依赖卸载
                foreach (var data in m_dependent)
                {
                    // 没有任何引用的话,通知依赖对象移除自己
                    if (removeReference)
                    {
                        data.RemoveReference(this);
                    }
                    // 依赖卸载
                    data.Unload(unloadAllLoadedObjects);
                }
            }
            #endregion
        }
    }
}