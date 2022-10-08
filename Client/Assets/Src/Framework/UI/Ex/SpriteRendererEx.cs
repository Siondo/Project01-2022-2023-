using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    using UnityAsset;

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererEx : MonoBehaviour
    {
        private SpriteRenderer m_spriteRenderer;

        private void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// 设置精灵
        /// </summary>
        /// <param name="res"></param>
        /// <param name="setNativeSize"></param>
        public void SetSprite(string res, bool setNativeSize = false)
        {
            SpriteManager.instance.LoadSprite(res, (c) => {
                m_spriteRenderer.sprite = c;
                if (setNativeSize)
                {
                    //SetNativeSize();
                }
            });
        }
    }
}