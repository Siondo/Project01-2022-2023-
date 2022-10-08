using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace UI
    {
        public abstract class UISortingOrder : MonoBehaviour
        {
            [SerializeField]
            protected UIBase m_ui;

            [SerializeField]
            protected int m_offset = 1;

            protected virtual void Awake()
            {
                
            }

            protected virtual void Start()
            {
                if (null == m_ui)
                {
                    m_ui = GetComponentInParent<UIBase>();
                }
                if (m_ui != null)
                {
                    SetSortingOrder(m_ui.sortingOrder);
                }
            }


            void OnEnable()
            {
                if (m_ui != null)
                {
                    StartCoroutine(DelaySetSortingOrder());
                }
            }

            /// <summary>
            /// 延迟设置层级
            /// </summary>
            /// <returns></returns>
            private IEnumerator DelaySetSortingOrder()
            {
                yield return new WaitForEndOfFrame();
                if (m_ui != null)
                {
                    SetSortingOrder(m_ui.sortingOrder);
                }
            }

            public abstract void SetSortingOrder(int order);
        }
    }
}
