using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace Framework
{
    public class ToggleEx : Toggle
    {
        [SerializeField]
        private GameObject m_background;

        [SerializeField]
        private GameObject m_checkmark;

        /// <summary>
        /// 启动
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            //添加监听
            OnAddListener();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnToggleChanged(base.isOn);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            //播放声音
        }

        /// <summary>
        /// Toggle改变
        /// </summary>
        /// <param name="toggle"></param>
        public void OnToggleChanged(bool toggle)
        {
            OnUpdateToggleUI(toggle);
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        /// <param name="toggle"></param>
        private void OnUpdateToggleUI(bool toggle)
        {
            if (m_background != null)
            {
                m_background.SetActive(!toggle);
            }
            if (m_checkmark != null)
            {
                m_checkmark.SetActive(toggle);
            }
        }

#if UNITY_EDITOR
        public override void Rebuild(CanvasUpdate executing)
        {
            base.Rebuild(executing);

            if (!Application.isPlaying && executing == CanvasUpdate.PostLayout && gameObject.activeInHierarchy)
            {
                StopCoroutine("OnLateUpdateToggleUI");
                StartCoroutine("OnLateUpdateToggleUI");
            }
        }

        private IEnumerator OnLateUpdateToggleUI()
        {
            yield return new WaitForEndOfFrame();
            OnUpdateToggleUI(isOn);
        }
#endif

        public void OnAddListener()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            if (m_background != null || m_checkmark != null)
            {
                onValueChanged.RemoveListener(OnToggleChanged);
                onValueChanged.AddListener(OnToggleChanged);
            }
        }
    }
}