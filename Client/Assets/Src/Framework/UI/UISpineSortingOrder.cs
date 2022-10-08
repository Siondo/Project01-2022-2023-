using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace UI
    {
        public class UISpineSortingOrder : UISortingOrder
        {
            private Renderer[] m_renders;

            protected override void Awake()
            {
                m_renders = GetComponentsInChildren<Renderer>();
                if (m_ui != null)
                {
                    m_ui.AddSortingOrder(this);
                }
            }

            /// <summary>
            /// 更新Renderer并排序
            /// </summary>
            public void UpdateSorting()
            {
                m_renders = GetComponentsInChildren<Renderer>();
                if (m_ui != null)
                {
                    m_ui.AddSortingOrder(this);
                    SetSortingOrder(m_ui.sortingOrder);
                }
            }

            public override void SetSortingOrder(int order)
            {
                if (m_renders == null)
                {
                    m_renders = GetComponentsInChildren<Renderer>();
                }
                if (m_renders != null)
                {
                    for (int i = 0; i < m_renders.Length; ++i)
                    {
                        if (m_renders[i] == null)
                        {
                            continue;
                        }
                        m_renders[i].sortingOrder = order + m_offset;
                    }
                }
            }
        }
    }
}
