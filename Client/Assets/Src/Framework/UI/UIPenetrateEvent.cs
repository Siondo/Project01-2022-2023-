using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Framework.UI
{
    public class UIPenetrateEvent : MaskableGraphic, IPointerClickHandler, ICanvasRaycastFilter
    {
        [SerializeField]
        private List<RectTransform> m_target;

        [SerializeField]
        private UnityEvent m_onClick;

        public UnityEvent onClick
        {
            get { return m_onClick; }
            set { m_onClick = value; }
        }

        /// <summary>
        /// 监听点击
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerClickHandler);
            onClick.Invoke();
        }

        /// <summary>
        /// 把事件透下去
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventData"></param>
        /// <param name="function"></param>
        public void PassEvent<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            GameObject current = eventData.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, eventData, function);
                    break;
                }
            }
        }

        bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
        {
            if (m_target.Count == 0)
            {
                return true;
            }
            // 将目标对象范围内的事件镂空（使其穿过）
            bool contain = true;
            for (int i = 0; i < m_target.Count; ++i)
            {
                if (m_target[i] == null)
                {
                    continue;
                }
                contain = RectTransformUtility.RectangleContainsScreenPoint(m_target[i], screenPos, eventCamera);
                if (contain)
                {
                    break;
                }
            }
            return !contain;
        }
    }
}

