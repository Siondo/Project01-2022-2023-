using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework
{
    namespace UnityAsset
    {
        public sealed class FileAsset : UnityAsyncAsset
        {
            #region Variable
            /// <summary>
            /// UnityWebRequest
            /// </summary>
            private UnityWebRequest m_unityWebRequest = null;
            #endregion

            #region Property
            /// <summary>
            /// 是否完成
            /// </summary>
            public override bool isDone
            {
                get
                {
                    return m_unityWebRequest != null ? m_unityWebRequest.isDone : false;
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
                    return m_unityWebRequest != null ? m_unityWebRequest.downloadProgress : 0f;
                }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public override string error
            {
                get
                {
                    return m_unityWebRequest != null ? m_unityWebRequest.error : "unknown error";
                }
            }

            /// <summary>
            /// 字节
            /// </summary>
            public override byte[] bytes
            {
                get
                {
                    return m_unityWebRequest != null ? m_unityWebRequest.downloadHandler.data : base.bytes;
                }
            }

            /// <summary>
            /// 文本
            /// </summary>
            public override string text
            {
                get
                {
                    return m_unityWebRequest != null ? m_unityWebRequest.downloadHandler.text : base.text;
                }
            }
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public FileAsset(string url)
                : base(url)
            {
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            public override void AsyncLoad()
            {
                base.AsyncLoad();
                m_unityWebRequest = UnityWebRequest.Get(PathUtil.GetRealUrl(url));
                m_unityWebRequest.SendWebRequest();
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
                return null;
            }

            /// <summary>
            /// 异步加载资源完成
            /// </summary>
            public override void LoadAssetAsyncComplete(string name, AssetBundleRequest assetBundleRequest)
            {

            }

            /// <summary>
            /// 得到资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override Object GetAsset(string name)
            {
                return null;
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
                m_unityWebRequest.Dispose();
                m_unityWebRequest = null;
            }
            #endregion
        }
    }
}