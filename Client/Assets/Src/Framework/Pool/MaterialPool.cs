//using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    using Singleton;
    namespace Pool
    {
        public class MaterialPool : Singleton<MaterialPool>
        {
            /// <summary>
            /// AB资源池
            /// </summary>
            private Dictionary<string, Material> m_pool;

            /// <summary>
            /// 构造
            /// </summary>
            public MaterialPool()
            {
                m_pool = new Dictionary<string, Material>();
            }

            /// <summary>
            /// 初始化Material
            /// </summary>
            /// <param name="material"></param>
            public void Init(Object[] materials)
            {
                m_pool.Clear();
                Material material = null;
                for (int i = 0; i < materials.Length; ++i)
                {
                    material = materials[i] as Material;
                    if (null == material || m_pool.ContainsKey(material.name))
                    {
                        return;
                    }
                    m_pool.Add(material.name, material);
                    Debug.Log("Material: " + material.name);
                }
            }

            /// <summary>
            /// 得到一个Material
            /// </summary>
            /// <param name="name"></param>
            public Material GetMaterial(string name)
            {
                Material material = null;
                if (App.abMode)
                {
                    if (m_pool.ContainsKey(name))
                    {
                        Debug.Log("GetMaterial: " + name);
                        material = m_pool[name];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    AssetPool.instance.LoadObjectFromPool("res/material/" + name, (o) => {
                        material = o as Material;
                    }, false);
                }
                if (null != material)
                {
                    material = Object.Instantiate<Material>(material);
                }
                return material;
            }
        }
    }
}