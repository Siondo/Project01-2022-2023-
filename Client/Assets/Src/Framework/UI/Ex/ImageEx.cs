using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    using UnityAsset;
    public class ImageEx : Image
    {
        /// <summary>
        /// 是否置灰
        /// </summary>
        [SerializeField]
        private bool m_grayColor = false;

        protected override void Awake()
        {
            base.Awake();
            if (m_grayColor)
            {
                SetGray(m_grayColor);
            }
        }

        /// <summary>
        /// 设置精灵
        /// </summary>
        /// <param name="res"></param>
        /// <param name="setNativeSize"></param>
        public void SetSprite(string res, bool setNativeSize = false)
        {
            SpriteManager.instance.LoadSprite(res, (c) => {
                sprite = c;
                if (setNativeSize)
                {
                    SetNativeSize();
                }
            });
        }

        /// <summary>
        /// 为True设置置灰效果
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="power"></param>
        public void SetGray(bool enable, float power = 1f)
        {
            if (!enable && material.name != "UI/UIEffect")
            {
                return;
            }
            UIEffectIns.SetGrayEffect(this, enable, power);
        }
    }
}