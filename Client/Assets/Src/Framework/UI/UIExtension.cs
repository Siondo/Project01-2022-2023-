using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public static class UIEffectIns
    {
        public static Material GetMaterial(string shaderPath)
        {
            Shader shader = Pool.ShaderPool.instance.GetShader(shaderPath);
            if (shader == null)
            {
                Debug.LogErrorFormat("Not find shader:{0}", shaderPath);
                return null;
            }
            Material mat = new Material(shader);
            mat.hideFlags = HideFlags.DontSave;
            return mat;
        }

        public static Material GetMaterial(string shaderPath, Graphic graphic)
        {
            Shader shader = Pool.ShaderPool.instance.GetShader(shaderPath);
            if (shader == null)
            {
                Debug.LogErrorFormat("Not find shader:{0}", shaderPath);
                return null;
            }
            if (shader != graphic.material.shader)
            {
                Material mat = new Material(shader);
                mat.hideFlags = HideFlags.DontSave;
                mat.CopyPropertiesFromMaterial(graphic.material);
                graphic.material = mat;
            }
            return graphic.material;
        }

        public static void SetGrayEffect(this Graphic graphic, bool enabled, float power = 1f)
        {
            Material material = GetMaterial("UI/UIEffect", graphic);
            if (material == null)
            {
                return;
            }
            if (enabled)
            {
                material.EnableKeyword("TONE_GRAY");
                material.SetFloat("_TonePower", power);
            }
            else
            {
                material.DisableKeyword("TONE_GRAY");
            }
        }

        public static void SetPixelEffect(this Graphic graphic, bool enabled, float power = 1f)
        {
            Material material = GetMaterial("UI/UIEffect", graphic);
            if (material == null)
            {
                return;
            }
            if (enabled)
            {
                material.DisableKeyword("EFFECT_BLUR");
                material.EnableKeyword("EFFECT_PIXEL");
                material.SetFloat("_PixelPower", power);
            }
            else
            {
                material.DisableKeyword("EFFECT_PIXEL");
            }
        }

        public static void SetBlurEffect(this Graphic graphic, bool enabled, float power = 1f)
        {
            Material material = GetMaterial("UI/UIEffect", graphic);
            if (material == null)
            {
                return;
            }
            if (enabled)
            {
                material.DisableKeyword("EFFECT_PIXEL");
                material.EnableKeyword("EFFECT_BLUR");
                material.SetFloat("_PixelPower", power);
            }
            else
            {
                material.DisableKeyword("EFFECT_BLUR");
            }
        }

        /// <summary>
        /// 设置Spine描边
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="outline"></param>
        /// <param name="color"></param>
        /// <param name="outlineWidth"></param>
        public static void SetSpineOutline(Spine.Unity.SkeletonRenderer renderer, bool outline, Color color, float outlineWidth = 6)
        {
            if (null == renderer)
            {
                return;
            }

            if (outline)
            {
                Material originalMaterial = renderer.skeletonDataAsset.atlasAssets[0].PrimaryMaterial;
                if (!originalMaterial.shader.name.Contains("Outline"))
                {
                    Material newMaterial = GetMaterial("Spine/Outline/Skeleton");
                    if (null != newMaterial)
                    {
                        newMaterial.CopyPropertiesFromMaterial(originalMaterial);
                        renderer.CustomMaterialOverride[originalMaterial] = newMaterial;
                    }
                }

                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                mpb.SetColor("_OutlineColor", color);
                mpb.SetFloat("_OutlineWidth", outlineWidth);
                mpb.SetFloat("_ThresholdEnd", 0.25f);
                renderer.GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
            }
            else
            {
                Material originalMaterial = renderer.skeletonDataAsset.atlasAssets[0].PrimaryMaterial;
                if (renderer.CustomMaterialOverride.ContainsKey(originalMaterial))
                {
                    Material overrideMaterial = renderer.CustomMaterialOverride[originalMaterial];
                    if (overrideMaterial.shader.name.Contains("Outline"))
                    {
                        Material newMaterial = GetMaterial("Spine/Skeleton");
                        if (null != newMaterial)
                        {
                            newMaterial.CopyPropertiesFromMaterial(originalMaterial);
                            renderer.CustomMaterialOverride[originalMaterial] = newMaterial;
                        }
                    }
                }
            }
        }
    }
}
