//using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    using Singleton;
    namespace Pool
    {
        public class ShaderPool : Singleton<ShaderPool>
        {
            /// <summary>
            /// AB资源池
            /// </summary>
            private Dictionary<string, Shader> m_pool;

            /// <summary>
            /// 构造
            /// </summary>
            public ShaderPool()
            {
                m_pool = new Dictionary<string, Shader>();
            }

            /// <summary>
            /// 初始化Shader
            /// </summary>
            /// <param name="shaders"></param>
            public void Init(Object[] shaders)
            {
                m_pool.Clear();
                Shader shader = null;
                for (int i = 0; i < shaders.Length; ++i)
                {
                    shader = shaders[i] as Shader;
                    if (null == shader || m_pool.ContainsKey(shader.name))
                    {
                        return;
                    }
                    m_pool.Add(shader.name, shader);
                    Debug.Log("Shader: " + shader.name);
                }
            }

            /// <summary>
            /// 得到一个Shader
            /// </summary>
            /// <param name="name"></param>
            public Shader GetShader(string name)
            {
                if (App.abMode)
                {
                    if (m_pool.ContainsKey(name))
                    {
                        Debug.Log("GetShader: " + name);
                        return m_pool[name];
                    }
                    else
                    {
                        return Shader.Find(name);
                    }
                }
                else
                {
                    return Shader.Find(name);
                }
            }
        }
    }
}