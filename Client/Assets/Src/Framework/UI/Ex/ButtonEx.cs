using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Framework
{
    public class ButtonEx : Button
    {
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_onClick = new ButtonClickedEvent();

        /// <summary>
        /// 按钮点击的间隔时间
        /// </summary>
        [SerializeField]
        private float m_clickIntervalTime = 0.2f;

        /// <summary>
        /// 可以点击状态(正常)
        /// </summary>
        [SerializeField]
        private GameObject m_normal;

        /// <summary>
        /// 不可点击状态(禁止)
        /// </summary>
        [SerializeField]
        private GameObject m_disabled;

        /// <summary>
        /// 是否开启点击动画
        /// </summary>
        [SerializeField]
        private bool m_isButtonScale;

        /// <summary>
        /// 按下缩放大小
        /// </summary>
        [SerializeField]
        private Vector3 m_clickDownScale = new Vector3(0.98f, 0.98f, 0.98f);

        /// <summary>
        /// 静止状态大小
        /// </summary>
        [SerializeField]
        private Vector3 m_normalScale = Vector3.one;

        /// <summary>
        /// 上一次点击时间
        /// </summary>
        private float m_lastClickTime = 0F;

        /// <summary>
        /// 新写点击事件
        /// </summary>
        public new ButtonClickedEvent onClick
        {
            get
            {
                return m_onClick;
            }
            set
            {
                m_onClick = value;
            }
        }

        /// <summary>
        /// 重写interactable
        /// </summary>
        public new bool interactable
        {
            get
            {
                return base.interactable;
            }
            set
            {
                base.interactable = value;
                if (m_normal != null)
                {
                    m_normal.SetActive(value);
                }
                if (m_disabled != null)
                {
                    m_disabled.SetActive(!value);
                }
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        protected override void Awake()
        {
            base.onClick.AddListener(OnClick);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            interactable = base.interactable;
        }

        /// <summary>
        /// 按钮点击
        /// </summary>
        private void OnClick()
        {
            float realtimeSinceStartup = Time.realtimeSinceStartup;
            if (realtimeSinceStartup >= m_lastClickTime + m_clickIntervalTime)
            {
                m_lastClickTime = realtimeSinceStartup;

                onClick.Invoke();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (m_isButtonScale && base.interactable)
                transform.localScale = m_clickDownScale;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (m_isButtonScale && base.interactable)
                transform.localScale = m_normalScale;
        }

        /// <summary>
        /// 设置图形置灰
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="enable"></param>
        /// <param name="power"></param>
        public void SetGray(bool enable, bool interactable = false, float power = 1f)
        {
            if (!enable && targetGraphic.material.name != "UI/UIEffect")
            {
                return;
            }
            UIEffectIns.SetGrayEffect(targetGraphic, enable, power);
            this.enabled = interactable;
        }

        /// <summary>
        /// 为True设置置灰效果
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="power"></param>
        public void SetGrayInChildren(bool enable, bool interactable = false, float power = 1f)
        {
            Graphic[] graphics = gameObject.GetComponentsInChildren<Graphic>(true);
            for (int i = 0; i < graphics.Length; ++i)
            {
                if (!enable && (graphics[i].material.name != "UI/UIEffect"))
                {
                    continue;
                }
                UIEffectIns.SetGrayEffect(graphics[i], enable, power);
            }
            this.enabled = interactable;
        }
    }
}