using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Framework
{
    public class ScrollRectEx : ScrollRect
    {
        /// <summary>
        /// 滚动的方向
        /// </summary>
        [Serializable]
        enum Direction
        {
            Horizontal,
            Vertical,
        }

        /// <summary>
        /// 两边各增加2个，保证滚动画面流畅性
        /// </summary>
        const int COUNT = 2;

        /// <summary>
        /// 计算滚动偏移
        /// </summary>
        const float OFFSET = 1;

        /// <summary>
        /// 删除动效播放多少帧
        /// </summary>
        const int FRAME = 10;

        [SerializeField]
        /// <summary>
        /// 布局方向
        /// </summary>
        private Direction m_direction = Direction.Vertical;

        [SerializeField]
        /// <summary>
        /// 布局间隔
        /// </summary>
        private Vector2 m_spacing = Vector2.zero;

        [SerializeField]
        /// <summary>
        /// 列数或排数
        /// </summary>
        private int m_columnOrRow = 1;

        [SerializeField]
        /// <summary>
        /// Item组大小
        /// </summary>
        private Vector2 m_groupSize = Vector2.one;

        [SerializeField]
        /// <summary>
        /// 更新Item
        /// </summary>
        private Action<GameObject, int> m_onItemUpdate = null;

        [SerializeField]
        /// <summary>
        /// 如果开启居中显示
        /// </summary>
        private bool m_centerShow = false;

        [SerializeField]
        /// <summary>
        /// 如果开启居中显示,计算居中显示的最小容器大小
        /// </summary>
        private Vector2 m_contentRect = Vector2.zero;

        /// <summary>
        /// Items对象
        /// </summary>
        private List<GameObject> m_items = new List<GameObject>();

        /// <summary>
        /// Item大小
        /// </summary>
        private Vector2 m_itemSize = Vector2.one;

        /// <summary>
        /// Item组 数量
        /// </summary>
        private int m_groupCount = 0;

        /// <summary>
        /// 初始化完成
        /// </summary>
        private bool m_initComplete = false;

        /// <summary>
        /// 数据最大个数
        /// </summary>
        private int m_count = 0;

        /// <summary>
        /// 是否开启滚动值改变监听
        /// </summary>
        private bool m_onValueChangedEnable = false;

        /// <summary>
        /// 删除动效播放到第几帧
        /// </summary>
        private int m_deleteFrame = 0;

        /// <summary>
        /// 删除播放的Item对象表
        /// </summary>
        private Dictionary<RectTransform, Vector2> m_deleteDict = new Dictionary<RectTransform, Vector2>();

        /// <summary>
        /// 缓存横向
        /// </summary>
        private bool m_horizontal = false;

        /// <summary>
        /// 缓存纵向
        /// </summary>
        private bool m_vertical = false;

        /// <summary>
        /// 缓存V2
        /// </summary>
        private Vector2 m_temp = Vector2.zero;

        /// <summary>
        /// 缓存索引
        /// </summary>
        private int m_tempIndex = 0;

        /// <summary>
        /// Item组 数量
        /// </summary>
        public int GroupCnt
        {
            get
            {
                return m_groupCount;
            }
        }

        /// <summary>
        /// 数据最大个数
        /// </summary>
        public int Count
        {
            get
            {
                return m_count;
            }
        }

        /// <summary>
        /// Item更新事件
        /// </summary>
        public Action<GameObject, int> onItemUpdate
        {
            get
            {
                return m_onItemUpdate;
            }
            set
            {
                m_onItemUpdate = value;
            }
        }

        protected override void Awake()
        {
            Transform templete = content.GetChild(0);
            if (null != templete)
            {
                templete.gameObject.SetActive(false);
            }

            if (m_centerShow)
            {
                m_contentRect = viewport.rect.size;
            }
        }

        /// <summary>
        /// 得到指定索引的Item相对于Content开启界限的相对锚点位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector2 GetLocation(int index)
        {
            GetPosition(index);
            return content.anchoredPosition + m_temp;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="count"></param>
        public void Init(int count)
        {
            DeleteGoOn();
            m_onValueChangedEnable = false;
            InitOnce();

            m_count = count;
            // 设定Content中心点、描点、位置
            SetContentAnchor();
            if (m_initComplete)
            {
                for (int i = 0; i < m_items.Count; ++i)
                {
                    RectTransform item = m_items[i].transform as RectTransform;
                    item.anchoredPosition = GetPosition(i);
                    item.name = i.ToString();
                    if (i < Count)
                    {
                        item.gameObject.SetActive(true);
                        onItemUpdate?.Invoke(item.gameObject, i);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                }

                SetContentSize();
            }
            m_onValueChangedEnable = true;
        }

        /// <summary>
        /// 更新到指定索引(使指定索引显示在第一条位置),如果超出Content容器需要纠正位置
        /// </summary>
        /// <param name="count"></param>
        /// <param name="index"></param>
        public void UpdateTo(int count, int index)
        {
            UpdateTo(count, Vector2.zero, index);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="count"></param>
        public void UpdateTo(int count)
        {
            UpdateTo(count, Vector2.zero, -1);
        }

        /// <summary>
        /// 更新到指定索引(是指定索引显示在指定位置),如果超出Content容器需要纠正位置
        /// </summary>
        /// <param name="count"></param>
        /// <param name="position"></param>
        /// <param name="index"></param>
        public void UpdateTo(int count, Vector2 position, int index)
        {
            DeleteGoOn();
            m_onValueChangedEnable = false;
            InitOnce();

            m_count = count;
            if (m_initComplete)
            {
                SetContentSize();

                if (m_direction == Direction.Horizontal)
                {
                    m_temp.x = index < 0 ? content.anchoredPosition.x : (position.x - GetPosition(index).x);
                    m_temp.y = content.anchoredPosition.y;
                    if (Mathf.Abs(m_temp.x) + viewport.rect.width > content.rect.width)
                    {
                        m_temp.x = -(content.rect.width - viewport.rect.width);
                    }
                    content.anchoredPosition = m_temp;

                    index = Mathf.FloorToInt(Mathf.Abs((-(m_spacing.x + m_itemSize.x) * COUNT - content.anchoredPosition.x)) / (m_spacing.x + m_itemSize.x));
                }
                else
                {
                    m_temp.y = index < 0 ? content.anchoredPosition.y : (position.y - GetPosition(index).y);
                    m_temp.x = content.anchoredPosition.x;
                    if (m_temp.y + viewport.rect.height > content.rect.height)
                    {
                        m_temp.y = content.rect.height - viewport.rect.height;
                    }
                    content.anchoredPosition = m_temp;

                    index = Mathf.FloorToInt(Mathf.Abs(((m_spacing.y + m_itemSize.y) * COUNT - content.anchoredPosition.y)) / (m_spacing.y + m_itemSize.y));
                }

                for (int i = 0; i < m_items.Count; ++i)
                {
                    m_tempIndex = index * m_columnOrRow + i;
                    RectTransform item = m_items[i].transform as RectTransform;
                    item.anchoredPosition = GetPosition(m_tempIndex);
                    item.name = m_tempIndex.ToString();
                    if (m_tempIndex < Count)
                    {
                        item.gameObject.SetActive(true);
                        onItemUpdate?.Invoke(item.gameObject, m_tempIndex);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                }
            }
            m_onValueChangedEnable = true;
        }

        /// <summary>
        /// 更新到指定索引(是指定索引显示在指定位置),如果超出Content容器需要纠正位置
        /// </summary>
        /// <param name="count"></param>
        /// <param name="position"></param>
        /// <param name="index"></param>
        public void UpdateAll()
        {
            DeleteGoOn();
            m_onValueChangedEnable = false;
            InitOnce();

            if (m_initComplete)
            {
                for (int i = 0; i < m_items.Count; ++i)
                {
                    RectTransform item = m_items[i].transform as RectTransform;
                    if (i < Count)
                    {
                        item.gameObject.SetActive(true);
                        onItemUpdate?.Invoke(item.gameObject, int.Parse(item.name));
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                }
            }
            m_onValueChangedEnable = true;
        }

        /// <summary>
        /// 更新指定索引位置
        /// </summary>
        /// <param name="index"></param>
        public void UpdateOne(int index)
        {
            DeleteGoOn();
            m_onValueChangedEnable = false;
            InitOnce();

            if (m_initComplete)
            {
                for (int i = 0; i < m_items.Count; ++i)
                {
                    RectTransform item = m_items[i].transform as RectTransform;
                    if (i < Count && int.Parse(item.name) == index && item.gameObject.activeSelf)
                    {
                        onItemUpdate?.Invoke(item.gameObject, int.Parse(item.name));
                        break;
                    }
                }
            }
            m_onValueChangedEnable = true;
        }

        /// <summary>
        /// 删除某一个Item
        /// </summary>
        /// <param name="index"></param>
        public void Delete(int index)
        {
            StopCoroutine("DeleteCor");
            StartCoroutine("DeleteCor", index);
        }

        /// <summary>
        /// 协同删除
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private IEnumerator DeleteCor(int index)
        {
            if (index >= 0 && index < Count)
            {
                m_count -= 1;
                // 找到目标Item索引
                int targetIndex = index;
                RectTransform item = m_items[0].transform as RectTransform;
                Vector2 offset = Vector2.zero;
                if (m_direction == Direction.Horizontal)
                {
                    int startIndex = Mathf.FloorToInt(item.anchoredPosition.x / (m_spacing.x + m_itemSize.x)) * m_columnOrRow;
                    targetIndex = index - startIndex;

                    Vector2 oldEnd = GetPosition(Count);
                    Vector2 newEnd = GetPosition(Count - 1);
                    if (Mathf.Abs(oldEnd.x - newEnd.x) > 1 && (newEnd.x + content.anchoredPosition.x) < viewport.rect.width)
                    {
                        offset.x = -(newEnd.x + m_itemSize.x - viewport.rect.width) - content.anchoredPosition.x;
                    }
                }
                else
                {
                    int startIndex = Mathf.FloorToInt(Mathf.Abs(item.anchoredPosition.y) / (m_spacing.y + m_itemSize.y)) * m_columnOrRow;
                    targetIndex = index - startIndex;

                    Vector2 oldEnd = GetPosition(Count);
                    Vector2 newEnd = GetPosition(Count - 1);
                    if (Mathf.Abs(oldEnd.y - newEnd.y) > 1 && (newEnd.y + content.anchoredPosition.y) > -viewport.rect.height)
                    {
                        offset.y = (-newEnd.y + m_itemSize.y - viewport.rect.height) - content.anchoredPosition.y;
                    }
                }

                if (targetIndex >= 0 && targetIndex < m_items.Count)
                {
                    // 先将目标Item隐藏
                    var target = m_items[targetIndex].transform as RectTransform;
                    target.gameObject.SetActive(false);

                    // 清空
                    m_deleteDict.Clear();
                    // 添加要移动的对象
                    RectTransform preItem;
                    for (int i = m_items.Count - 1; i > targetIndex; --i)
                    {
                        item = m_items[i].transform as RectTransform;
                        preItem = m_items[i - 1].transform as RectTransform;

                        item.name = preItem.name;
                        m_deleteDict.Add(item, (preItem.anchoredPosition - item.anchoredPosition) / FRAME);

                        m_tempIndex = int.Parse(item.name);
                        if (m_tempIndex < Count)
                        {
                            item.gameObject.SetActive(true);
                            onItemUpdate?.Invoke(item.gameObject, m_tempIndex);
                        }
                        else
                        {
                            item.gameObject.SetActive(false);
                        }
                    }
                    if (offset.sqrMagnitude > 1)
                    {
                        m_deleteDict.Add(content, offset / FRAME);
                    }
                    // 将目标对象移动到最后
                    m_items.RemoveAt(targetIndex);
                    item = m_items[m_items.Count - 1].transform as RectTransform;
                    m_tempIndex = int.Parse(item.name) + 1;
                    m_items.Add(target.gameObject);
                    target.name = m_tempIndex.ToString();
                    target.anchoredPosition = GetPosition(m_tempIndex);
                    target.SetAsLastSibling();
                    target.SetSiblingIndex(item.GetSiblingIndex() + 1);
                    if (m_tempIndex < Count)
                    {
                        target.gameObject.SetActive(true);
                        onItemUpdate?.Invoke(item.gameObject, m_tempIndex);
                    }
                    else
                    {
                        target.gameObject.SetActive(false);
                    }
                    // 执行移动动作
                    StopMovement();
                    m_horizontal = horizontal;
                    m_vertical = vertical;
                    horizontal = false;
                    vertical = false;
                    for (m_deleteFrame = 0; m_deleteFrame < FRAME; ++m_deleteFrame)
                    {
                        yield return new WaitForEndOfFrame();
                        foreach (var kvp in m_deleteDict)
                        {
                            kvp.Key.anchoredPosition = kvp.Key.anchoredPosition + kvp.Value;
                        }
                    }
                    SetContentSize();
                    m_deleteDict.Clear();
                    horizontal = m_horizontal;
                    vertical = m_vertical;
                }
            }
        }

        /// <summary>
        /// 继续播放删除效果
        /// </summary>
        private void DeleteGoOn()
        {
            if (m_deleteDict.Count > 0)
            {
                StopCoroutine("DeleteCor");
                for (++m_deleteFrame; m_deleteFrame < FRAME; ++m_deleteFrame)
                {
                    foreach (var kvp in m_deleteDict)
                    {
                        kvp.Key.anchoredPosition = kvp.Key.anchoredPosition + kvp.Value;
                    }
                }
                SetContentSize();
                m_deleteDict.Clear();
                horizontal = m_horizontal;
                vertical = m_vertical;
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            DeleteGoOn();
        }

        /// <summary>
        /// 初始化一次
        /// </summary>
        private void InitOnce()
        {
            if (!m_initComplete)
            {
                onValueChanged.RemoveListener(OnValueChanged);
                onValueChanged.AddListener(OnValueChanged);
                // 初始化模板大小
                m_initComplete = InitTemplateItemSize();
                // 设置组数
                SetGroupCount();
                // 设定Content中心点、描点、位置
                SetContentAnchor();
                // 初始化模板
                InitTemplate();
            }
        }

        /// <summary>
        /// 从后面取出放入前面
        /// </summary>
        /// <param name="item"></param>
        private void BackToFront(RectTransform item)
        {
            if (int.TryParse(item.name, out m_tempIndex) && m_tempIndex > 0)
            {
                for (int i = 0; i < m_columnOrRow; ++i)
                {
                    --m_tempIndex;
                    RectTransform next = m_items[m_items.Count - 1].transform as RectTransform;
                    m_items.RemoveAt(m_items.Count - 1);
                    m_items.Insert(0, next.gameObject);

                    next.anchoredPosition = GetPosition(m_tempIndex);
                    next.name = m_tempIndex.ToString();
                    next.gameObject.SetActive(true);
                    onItemUpdate?.Invoke(next.gameObject, m_tempIndex);
                }
            }
        }

        /// <summary>
        /// 从前面取出放入后面
        /// </summary>
        /// <param name="item"></param>
        private void FrontToBack(RectTransform item)
        {
            if (int.TryParse(item.name, out m_tempIndex) && m_tempIndex < Count - 1)
            {
                for (int i = 0; i < m_columnOrRow; ++i)
                {
                    ++m_tempIndex;
                    RectTransform next = m_items[0].transform as RectTransform;
                    m_items.RemoveAt(0);
                    m_items.Add(next.gameObject);

                    next.anchoredPosition = GetPosition(m_tempIndex);
                    next.name = m_tempIndex.ToString();
                    if (m_tempIndex < Count)
                    {
                        next.gameObject.SetActive(true);
                        onItemUpdate?.Invoke(next.gameObject, m_tempIndex);
                    }
                    else
                    {
                        next.gameObject.SetActive(false);
                    }
                }
            }
        }

        /// <summary>
        /// 是否满足条件后面的填充到前面
        /// </summary>
        /// <param name="firstItem"></param>
        /// <returns></returns>
        private bool IsBackToFront(ref RectTransform firstItem)
        {
            if (m_direction == Direction.Horizontal)
            {
                return firstItem.anchoredPosition.x + content.anchoredPosition.x > -(m_itemSize.x + m_spacing.x) * (COUNT - OFFSET);
            }
            else
            {
                return firstItem.anchoredPosition.y + content.anchoredPosition.y < (m_itemSize.y + m_spacing.y) * (COUNT - OFFSET);
            }
        }

        /// <summary>
        /// 是否满足条件前面的填充到后面
        /// </summary>
        /// <param name="lastItem"></param>
        /// <returns></returns>
        private bool IsFrontToBack(ref RectTransform lastItem)
        {
            if (m_direction == Direction.Horizontal)
            {
                return lastItem.anchoredPosition.x + content.anchoredPosition.x + m_itemSize.x < viewport.rect.width + (m_itemSize.x + m_spacing.x) * (COUNT - OFFSET);
            }
            else
            {
                return lastItem.anchoredPosition.y + content.anchoredPosition.y - m_itemSize.y > -viewport.rect.height - (m_itemSize.y + m_spacing.y) * (COUNT - OFFSET);
            }
        }

        /// <summary>
        /// 滚动拖动
        /// </summary>
        /// <param name="delta"></param>
        private void OnValueChanged(Vector2 delta)
        {
            if (m_onValueChangedEnable)
            {
                RectTransform firstItem = m_items[0].transform as RectTransform;
                RectTransform lastItem = m_items[m_items.Count - 1].transform as RectTransform;
                if (IsBackToFront(ref firstItem))
                {
                    BackToFront(firstItem);
                }
                else if (IsFrontToBack(ref lastItem))
                {
                    FrontToBack(lastItem);
                }
            }
        }

        /// <summary>
        /// 得到索引位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Vector2 GetPosition(int index)
        {
            int i = Mathf.FloorToInt(index / m_columnOrRow);
            int mul = index % m_columnOrRow;
            if (m_direction == Direction.Horizontal)
            {
                m_temp.x = i * m_itemSize.x + i * m_spacing.x;
                m_temp.y = (content.rect.height - m_groupSize.y) * -0.5f - mul * m_itemSize.y - mul * m_spacing.y;
            }
            else
            {
                m_temp.x = (content.rect.width - m_groupSize.x) * 0.5f + mul * m_itemSize.x + mul * m_spacing.x;
                m_temp.y = -i * m_itemSize.y + -i * m_spacing.y;
            }
            return m_temp;
        }

        /// <summary>
        /// 初始化模板
        /// </summary>
        private void InitTemplate()
        {
            if (content.childCount > 0)
            {
                RectTransform templete = content.GetChild(0) as RectTransform;
                // 设定模板对象中心点、描点
                templete.pivot = new Vector2(0f, 1f);
                templete.anchorMin = Vector2.up;
                templete.anchorMax = Vector2.up;

                // 实例化模板
                int count = GroupCnt * m_columnOrRow;
                m_items.Add(templete.gameObject);
                for (int i = 1; i < count; ++i)
                {
                    GameObject item = Instantiate<GameObject>(templete.gameObject, content);
                    m_items.Add(item);
                }
            }
        }

        /// <summary>
        /// 设置Content中心点、描点、位置
        /// </summary>
        public void SetContentAnchor()
        {
            if (m_direction == Direction.Vertical)
            {
                content.pivot = new Vector2(0.5f, 1f);
                content.anchorMin = Vector2.up;
                content.anchorMax = Vector2.one;
            }
            else
            {
                content.pivot = new Vector2(0f, 0.5f);
                content.anchorMin = Vector2.zero;
                content.anchorMax = Vector2.up;
            }
            content.anchoredPosition = Vector3.zero;
            content.offsetMin = Vector2.zero;
            content.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// 设置组数
        /// </summary>
        private void SetGroupCount()
        {
            if (m_direction == Direction.Horizontal)
            {
                m_groupCount = Mathf.CeilToInt((viewport.rect.width + m_spacing.x) / (m_itemSize.x + m_spacing.x));

                m_groupSize.x = m_itemSize.x;
                m_groupSize.y = Mathf.CeilToInt((m_columnOrRow - 1) * m_spacing.y + m_itemSize.y * m_columnOrRow);
            }
            else
            {
                m_groupCount = Mathf.CeilToInt((viewport.rect.height + m_spacing.y) / (m_itemSize.y + m_spacing.y));

                m_groupSize.x = Mathf.CeilToInt((m_columnOrRow - 1) * m_spacing.x + m_itemSize.x * m_columnOrRow);
                m_groupSize.y = m_itemSize.y;
            }
            m_groupCount += COUNT * 2;
        }


        /// <summary>
        /// 设置模板Item大小
        /// </summary>
        /// <returns></returns>
        private bool InitTemplateItemSize()
        {
            if (content.childCount > 0)
            {
                RectTransform templete = content.GetChild(0) as RectTransform;
                m_itemSize = templete.sizeDelta;
            }
            return content.childCount > 0;
        }

        /// <summary>
        /// 设置Content大小
        /// </summary>
        private void SetContentSize()
        {
            int groupCnt = Mathf.CeilToInt((float)Count / m_columnOrRow);
            if (m_direction == Direction.Horizontal)
            {
                m_temp.x = (groupCnt - 1) * m_spacing.x + m_itemSize.x * groupCnt;
                m_temp.y = content.sizeDelta.y;

                if (m_centerShow)
                {
                    var component = content.GetComponent<LayoutGroup>();
                    if (null != component)
                    {
                        component.enabled = m_temp.x < m_contentRect.x;
                        if (component.enabled)
                        {
                            m_temp.x = m_contentRect.x;
                            m_temp.y = m_contentRect.y;
                        }
                    }
                }
            }
            else
            {
                m_temp.x = content.sizeDelta.x;
                m_temp.y = (groupCnt - 1) * m_spacing.y + m_itemSize.y * groupCnt;

                if (m_centerShow)
                {
                    var component = content.GetComponent<LayoutGroup>();
                    if (null != component)
                    {
                        component.enabled = m_temp.y < m_contentRect.y;
                        if (component.enabled)
                        {
                            m_temp.x = m_contentRect.x;
                            m_temp.y = m_contentRect.y;
                        }
                    }
                }
            }
            content.sizeDelta = m_temp;
        }

        [ContextMenu("TestInit")]
        public void TestInit()
        {
            Init(61);
        }

        [ContextMenu("TestDelete")]
        public void TestDelete()
        {
            Delete(56);
        }

        [ContextMenu("Location")]
        public void Location()
        {
            m_onValueChangedEnable = false;

            // 初始化模板大小
            m_initComplete = InitTemplateItemSize();
            // 设置组数
            SetGroupCount();
            // 设定Content中心点、描点、位置
            SetContentAnchor();

            if (m_initComplete)
            {
                for (int i = 0; i < content.childCount; ++i)
                {
                    RectTransform item = content.GetChild(i) as RectTransform;
                    item.anchoredPosition = GetPosition(i);
                }

                m_count = content.childCount;
                SetContentSize();
            }
            m_onValueChangedEnable = true;
        }
    }
}