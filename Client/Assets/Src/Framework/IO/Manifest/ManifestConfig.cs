﻿using System.Collections.Generic;

namespace Framework
{
    namespace IO
    {
        /// <summary>
        /// 配置
        /// </summary>
        public class ManifestConfig
        {
            #region Variable
            /// <summary>
            /// 内部版本
            /// </summary>
            private string m_version = string.Empty;

            /// <summary>
            /// 打包资源版本
            /// </summary>
            private string m_assetVersion = string.Empty;

            /// <summary>
            /// 数据
            /// </summary>
            private Dictionary<string, Manifest> m_data = new Dictionary<string, Manifest>(2 << 12);

            /// <summary>
            /// 空表
            /// </summary>
            private static List<string> g_empty = new List<string>();
            #endregion

            #region Property
            /// <summary>
            /// 版本
            /// </summary>
            public string version
            {
                get { return m_version; }
                set { m_version = value; }
            }

            /// <summary>
            /// 得到版本
            /// </summary>
            public string assetVersion
            {
                get { return m_assetVersion; }
                set { m_assetVersion = value; }
            }

            /// <summary>
            /// 得到数据
            /// </summary>
            /// <value>The data.</value>
            public Dictionary<string, Manifest> data
            {
                get { return m_data; }
                set { m_data = value; }
            }
            #endregion

            #region Function
            /// <summary>
            /// 添加
            /// </summary>
            /// <param name="t">T.</param>
            public void Add(Manifest manifest)
            {
                if (m_data.ContainsKey(manifest.name))
                {
                    m_data[manifest.name] = manifest;
                }
                else
                {
                    m_data.Add(manifest.name, manifest);
                }
            }

            /// <summary>
            /// 移除
            /// </summary>
            /// <param name="name"></param>
            public void Remove(string name)
            {
                if (m_data.ContainsKey(name))
                {
                    m_data.Remove(name);
                }
            }

            /// <summary>
            /// 得到一份清单
            /// </summary>
            /// <param name="name">Name.</param>
            public Manifest Get(string name)
            {
                return m_data.ContainsKey(name) ? m_data[name] : null;
            }

            /// <summary>
            /// 得到依赖资源表
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public List<string> GetDependencies(string name)
            {
                Manifest manifest = Get(name);
                return manifest != null ? manifest.dependencies : g_empty;
            }

            /// <summary>
            /// 是否包含值
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool Contains(string name)
            {
                return m_data.ContainsKey(name);
            }
            #endregion
        }
    }
}