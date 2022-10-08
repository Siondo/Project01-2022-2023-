using UnityEngine;

namespace Framework
{
    namespace UnityAsset
    {
        public sealed class EditorAsset : UnityAsyncAsset
        {
            #region Variable
            /// <summary>
            /// 资源
            /// </summary>
            private Object m_mainAsset = null;
            #endregion

            #region Property
            /// <summary>
            /// 是否完成
            /// </summary>
            public override bool isDone
            {
                get { return null != m_mainAsset; }
            }

            /// <summary>
            /// 进度
            /// </summary>
            /// <value>The progress.</value>
            public override float progress
            {
                get
                {
                    m_progress = null != m_mainAsset ? 1F : 0f;
                    return m_progress;
                }
            }

            /// <summary>
            /// 错误
            /// </summary>
            public override string error
            {
                get
                {
                    return null != m_mainAsset ? string.Empty : "unknown error";
                }
            }

            /// <summary>
            /// 字节
            /// </summary>
            public override byte[] bytes => (m_mainAsset as TextAsset).bytes;

            /// <summary>
            /// 文本
            /// </summary>
            public override string text => (m_mainAsset as TextAsset).text;
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public EditorAsset(string url)
                : base(url)
            { }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            public override void AsyncLoad()
            {
                base.AsyncLoad();
#if UNITY_EDITOR
                if (url.EndsWith(".lua") && !App.abLua)
                {
                    string fielPath = PathUtil.dataPath.Replace("Assets", url);
                    string txt = System.IO.File.ReadAllText(fielPath);
                    m_mainAsset = new TextAsset(txt);
                }
                else
                {
                    m_mainAsset = UnityEditor.AssetDatabase.LoadAssetAtPath(m_url, typeof(Object));
                }
#endif
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override AssetBundleRequest LoadAssetAsync(string name)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// 得到资源
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public override Object GetAsset(string name)
            {
                return m_mainAsset;
            }

            /// <summary>
            /// 卸载
            /// </summary>
            /// <param name="unloadAllLoadedObjects"></param>
            public override void Unload(bool unloadAllLoadedObjects = true)
            {
                base.Unload(unloadAllLoadedObjects);
                m_mainAsset = null;
            }
            #endregion
        }
    }
}