using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    public class UIHollowComponent : MaskableGraphic, ICanvasRaycastFilter
    {
        [SerializeField]
        private RectTransform m_target;

        private Vector3 m_targetMin = Vector3.zero;
        private Vector3 m_targetMax = Vector3.zero;

        private bool m_canRefresh = true;
        private Transform m_cacheTrans = null;

        /// <summary>
        /// 设置镂空的目标
        /// </summary>
        public void SetTarget(RectTransform target)
        {
            m_canRefresh = true;
            m_target = target;
            RefreshView();
        }

        private void SetTarget(Vector3 targetMin, Vector3 targetMax)
        {
            if (targetMin == m_targetMin && targetMax == m_targetMax)
            {
                return;
            }
            m_targetMin = targetMin;
            m_targetMax = targetMax;
            SetAllDirty();
        }

        public void UpdateTarget()
        {
            m_canRefresh = true;
            RefreshView();
        }

        private void RefreshView()
        {
            if (!m_canRefresh)
            {
                return;
            }
            m_canRefresh = false;

            if(null == m_target)
            {
                SetTarget(Vector3.zero, Vector3.zero);
                SetAllDirty();
            }
            else
            {
                Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(m_cacheTrans, m_target);
                SetTarget(bounds.min, bounds.max);
            }
        }

        protected override void OnPopulateMesh( VertexHelper vh )
        {
            if(m_targetMin == Vector3.zero && m_targetMax == Vector3.zero)
            {
                base.OnPopulateMesh(vh);
                return;
            }
            vh.Clear();

            // 填充顶点
            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            Vector2 selfPiovt = rectTransform.pivot;
            Rect selfRect = rectTransform.rect;
            float outerLx = -selfPiovt.x * selfRect.width;
            float outerBy = -selfPiovt.y * selfRect.height;
            float outerRx = (1 - selfPiovt.x) * selfRect.width;
            float outerTy = (1 - selfPiovt.y) * selfRect.height;
            // 0 - Outer:LT
            vert.position = new Vector3(outerLx, outerTy);
            vh.AddVert( vert );
            // 1 - Outer:RT
            vert.position = new Vector3(outerRx, outerTy);
            vh.AddVert( vert );
            // 2 - Outer:RB
            vert.position = new Vector3(outerRx, outerBy);
            vh.AddVert( vert );
            // 3 - Outer:LB
            vert.position = new Vector3(outerLx, outerBy);
            vh.AddVert( vert );

            // 4 - Inner:LT
            vert.position = new Vector3(m_targetMin.x, m_targetMax.y);
            vh.AddVert( vert );
            // 5 - Inner:RT
            vert.position = new Vector3(m_targetMax.x, m_targetMax.y);
            vh.AddVert( vert );
            // 6 - Inner:RB
            vert.position = new Vector3(m_targetMax.x, m_targetMin.y);
            vh.AddVert( vert );
            // 7 - Inner:LB
            vert.position = new Vector3(m_targetMin.x, m_targetMin.y);
            vh.AddVert( vert );

            // 设定三角形
            vh.AddTriangle( 4, 0, 1 );
            vh.AddTriangle( 4, 1, 5 );
            vh.AddTriangle( 5, 1, 2 );
            vh.AddTriangle( 5, 2, 6 );
            vh.AddTriangle( 6, 2, 3 );
            vh.AddTriangle( 6, 3, 7 );
            vh.AddTriangle( 7, 3, 0 );
            vh.AddTriangle( 7, 0, 4 );
        }

        bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
        {
            if (null == m_target)
            {
                return true;
            }
            // 将目标对象范围内的事件镂空（使其穿过）
            return !RectTransformUtility.RectangleContainsScreenPoint(m_target, screenPos, eventCamera);
        }

        protected override void Awake()
        {
            base.Awake();
            m_cacheTrans = GetComponent<RectTransform>();
        }

#if UNITY_EDITOR
        void Update()
        {
            m_canRefresh = true;
            RefreshView();
        }
#endif
    }
}

