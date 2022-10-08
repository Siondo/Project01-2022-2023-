using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    namespace UI
    {
        public class UIChildSortingOrder : UISortingOrder
        {
            private Canvas m_canvas = null;

            protected override void Start()
            {
                base.Start();
                m_canvas = GetComponent<Canvas>();
                if (m_canvas == null)
                {
                    m_canvas = gameObject.AddComponent<Canvas>();
                }
                m_canvas.overrideSorting = true;
                if (m_ui != null)
                {
                    m_ui.AddSortingOrder(this);
                    SetSortingOrder(m_ui.sortingOrder);
                }
            }

            public override void SetSortingOrder(int order)
            {
                if (m_canvas != null)
                {
                    m_canvas.sortingOrder = order + m_offset;
                }
            }
        }
    }
}
