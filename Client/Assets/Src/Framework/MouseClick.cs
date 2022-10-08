using System.Collections;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_canvasRoot;

    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private float m_delay = 2f;

    [SerializeField]
    private string m_clickName;

    [SerializeField]
    private bool m_openDrag = true;

    [SerializeField]
    private string m_dragName;

    [SerializeField]
    private float m_dragStartDistance = 12f;

    [SerializeField]
    private float m_dragDeltaDistance = 6f;

    private Canvas m_canvas;
    private GestureProcessor m_gestureProcessor;
    public static MouseClick Instance;

    private bool m_move = false;
    private Vector2 m_startPos = Vector2.zero;
    private Vector2 m_movePos = Vector2.zero;
    private GameObject m_dragGo = null;

    public void Awake()
    {
        Instance = this;
        transform.localScale = m_canvasRoot.localScale;
        m_canvas = gameObject.GetComponentInChildren<Canvas>();
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        m_gestureProcessor = new GestureProcessor();
        m_gestureProcessor.onSingleTouchBegin += SingleTouchBegin;
        m_gestureProcessor.onSingleMoving += SingleMoving;
        m_gestureProcessor.onSingleTouchEnd += SingleTouchEnd;
        m_gestureProcessor.isObstacleUI = IsObstacleUI;
    }

    public void OnDestroy()
    {
        if (null != m_gestureProcessor)
        {
            m_gestureProcessor.onSingleTouchBegin -= SingleTouchBegin;
            m_gestureProcessor.onSingleMoving -= SingleMoving;
            m_gestureProcessor.onSingleTouchEnd -= SingleTouchEnd;
        }
    }

    private void Update()
    {
        if (m_gestureProcessor != null)
        {
            m_gestureProcessor.Update();
        }
    }


    protected bool IsObstacleUI()
    {
        return false;
    }

    private void SingleTouchBegin(Vector2 pos)
    {
        m_move = false;
        m_startPos = pos;

        if (null != m_dragGo)
        {
            GameObject go = m_dragGo;
            m_dragGo = null;

            go.SetActive(false);
            StartCoroutine(UnloadToPool(go, 2));
        }
    }

    private void SingleMoving(Vector2 pos)
    {
        if (!m_openDrag)
        {
            return;
        }
        if (m_move)
        {
            if (Vector2.Distance(m_movePos, pos) >= m_dragDeltaDistance)
            {
                m_movePos = pos;
                Vector2 localPos;
                if (TryGetLocalPoint(pos, out localPos) && null != m_dragGo)
                {
                    m_dragGo.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
                }
            }
        }
        else if (Vector2.Distance(m_startPos, pos) >= m_dragStartDistance)
        {
            m_move = true;

            Vector2 localPos;
            if (TryGetLocalPoint(pos, out localPos))
            {
                CreateMoveEffect(localPos);
            }
        }
    }

    private void SingleTouchEnd(Vector2 pos)
    {
        if (m_move)
        {
            m_move = false;
            if (null != m_dragGo)
            {
                GameObject go = m_dragGo;
                m_dragGo = null;

                go.SetActive(false);
                StartCoroutine(UnloadToPool(go, 2));
            }
        }
        else
        {
            Vector2 localPos;
            if (TryGetLocalPoint(pos, out localPos))
            {
                CreateClickEffect(localPos);
            }
        }
    }

    private bool TryGetLocalPoint(Vector2 pos, out Vector2 localPos)
    {
        Vector3 mousePos = pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_canvasRoot, mousePos, m_camera, out localPos);
        if (Mathf.Abs(localPos.x) < m_canvasRoot.rect.width / 2 && Mathf.Abs(localPos.y) < m_canvasRoot.rect.height / 2)
        {
            return true;
        }
        return false;
    }

    private void CreateClickEffect(Vector2 localPos)
    {
        LuaHelper.LoadFromPool(m_clickName, (go)=> {
            LuaHelper.ChangeLayer(go, Constant.Layers.UI);
            LuaHelper.EntityIdentity(go, string.Empty, transform, true);
            go.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
            go.SetActive(true);
            SetSortingOrder(go, 0);
            StartCoroutine(UnloadToPool(go, 2));
        }, true);
    }

    private void CreateMoveEffect(Vector2 localPos)
    {
        LuaHelper.LoadFromPool(m_dragName, (go) => {
            if (null != m_dragGo)
            {
                LuaHelper.UnloadToPool(m_dragGo);
                m_dragGo = null;
            }
            if (false == m_move)
            {
                LuaHelper.UnloadToPool(go);
                return;
            }
            m_dragGo = go;
            LuaHelper.ChangeLayer(go, Constant.Layers.UI);
            LuaHelper.EntityIdentity(go, string.Empty, transform, true);
            go.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);
            go.SetActive(true);
            SetSortingOrder(go, 1);
        }, true);
    }

    private IEnumerator UnloadToPool(GameObject go, float time = 2)
    {
        yield return new WaitForSeconds(time);
        LuaHelper.UnloadToPool(go);
    }

    public void SetSortingOrder(GameObject go, int order)
    {
        Renderer[] renders = go.GetComponentsInChildren<Renderer>();
        if (renders != null)
        {
            for (int i = 0; i < renders.Length; ++i)
            {
                if (renders[i] == null)
                {
                    continue;
                }
                renders[i].sortingOrder = m_canvas.sortingOrder + order;
            }
        }
    }
}
