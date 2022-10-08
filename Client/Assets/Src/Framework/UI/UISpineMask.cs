using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Framework
{
    namespace UI
    {
        //注意 只有这个UI在要被裁剪的粒子上层时 才在UI上挂这个脚本
        public class UISpineMask : MonoBehaviour
        {
            private static Material m_material;

            private static int g_stencilRef = Shader.PropertyToID("_StencilRef");

            [SerializeField]
            private float m_stencilRef = 1;

            private void Start()
            {
                if (m_material == null)
                {
                    var shader = LuaHelper.GetShader("Spine/Skeleton");
                    if (shader == null)
                    {
                        Debug.LogErrorFormat("UISpineMask 没有shader:Spine/Skeleton");
                        return;
                    }
                    m_material = new Material(shader);
                    if (m_material.HasProperty(g_stencilRef))
                    {
                        m_material.SetFloat(g_stencilRef, m_stencilRef);
                    }
                    m_material.hideFlags = HideFlags.DontSave;

                    //m_material.shader = shader;
                }

                var renders = GetComponentsInChildren<MeshRenderer>(true);
                foreach (var render in renders)
                {
                    //m_material.mainTexture = render.material.mainTexture;
                    //render.material = m_material;
                    if (render.material.HasProperty(g_stencilRef))
                    {
                        render.material.SetFloat(g_stencilRef, m_stencilRef);
                    }
                    render.material.hideFlags = HideFlags.DontSave;
                    render.material.renderQueue = 3000;
                }
            }
        }
    }
}
