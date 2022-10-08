using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GestureProcessorMono : MonoBehaviour
{
    [SerializeField]
    private float m_deltaScale = 0.01f;
    protected GestureProcessor m_gestureProcessor = new GestureProcessor();
    protected Vector2 m_touchPos = Vector2.zero;
    protected float m_simulateDis = 0;

    private class SingleTouch : UnityEvent<Vector2> { }
    private SingleTouch m_SingleTouchBegin = new SingleTouch();
    private SingleTouch m_SingleTouchMoving = new SingleTouch();
    private SingleTouch m_SingleTouchEnd = new SingleTouch();
    private class DoubleTouch : UnityEvent<Vector2, float> { }
    private DoubleTouch m_DoubleTouchBegin = new DoubleTouch();
    private DoubleTouch m_DoubleTouchMoving = new DoubleTouch();
    private DoubleTouch m_DoubleTouchEnd = new DoubleTouch();

    private UnityEvent m_Update = new UnityEvent();
    private UnityEvent m_LateUpdate = new UnityEvent();

    /// <summary>
    /// 是否单点开始
    /// </summary>
    private bool m_isSingleTouchBegin = false;

    #region Properties
    public bool UIObstacle
    {
        get
        {
            return m_gestureProcessor.UIObstacle;
        }
        set
        {
            m_gestureProcessor.UIObstacle = value;
        }
    }

    public string allowTag
    {
        get; set;
    }
    public string forbiddenTag
    {
        get; set;
    }
    #endregion

    private void Awake()
    {
        m_gestureProcessor.onSingleTouchBegin += OnSingleTouchBegin;
        m_gestureProcessor.onSingleMoving += OnSingleTouchMoving;
        m_gestureProcessor.onSingleTouchEnd += OnSingleTouchEnd;
        m_gestureProcessor.onDoubleTouchBegin += OnDoubleTouchBegin;
        m_gestureProcessor.onDoubleMoving += OnDoubleTouchMoving;
        m_gestureProcessor.onDoubleTouchEnd += OnDoubleTouchEnd;
        m_gestureProcessor.isObstacleUI = IsObstacleUI;
    }

    private void Update()
    {
        if (m_gestureProcessor != null)
        {
            m_gestureProcessor.Update();
        }
        m_Update.Invoke();
    }

    private void LateUpdate()
    {
        m_LateUpdate.Invoke();
    }

    public void AddSingleTouchBeginListener(UnityAction<Vector2> call)
    {
        m_SingleTouchBegin.RemoveAllListeners();
        m_SingleTouchBegin.AddListener(call);
    }

    public void AddSingleMovingListener(UnityAction<Vector2> call)
    {
        m_SingleTouchMoving.RemoveAllListeners();
        m_SingleTouchMoving.AddListener(call);
    }

    public void AddSingleTouchEndListener(UnityAction<Vector2> call)
    {
        m_SingleTouchEnd.RemoveAllListeners();
        m_SingleTouchEnd.AddListener(call);
    }

    public void AddDoubleTouchBeginListener(UnityAction<Vector2, float> call)
    {
        m_DoubleTouchBegin.RemoveAllListeners();
        m_DoubleTouchBegin.AddListener(call);
    }

    public void AddDoubleMovingListener(UnityAction<Vector2, float> call)
    {
        m_DoubleTouchMoving.RemoveAllListeners();
        m_DoubleTouchMoving.AddListener(call);
    }

    public void AddDoubleTouchEndListener(UnityAction<Vector2, float> call)
    {
        m_DoubleTouchEnd.RemoveAllListeners();
        m_DoubleTouchEnd.AddListener(call);
    }

    public void AddUpdateListener(UnityAction call)
    {
        m_Update.RemoveAllListeners();
        m_Update.AddListener(call);
    }

    public void AddLateUpdateListener(UnityAction call)
    {
        m_LateUpdate.RemoveAllListeners();
        m_LateUpdate.AddListener(call);
    }

    protected void OnSingleTouchBegin(Vector2 mousePosition)
    {
        m_isSingleTouchBegin = true;
        m_SingleTouchBegin.Invoke(mousePosition);
    }

    protected void OnSingleTouchMoving(Vector2 mousePosition)
    {
        m_SingleTouchMoving.Invoke(mousePosition);
    }

    protected void OnSingleTouchEnd(Vector2 mousePosition)
    {
        if (m_isSingleTouchBegin)
        {
            m_SingleTouchEnd.Invoke(mousePosition);
            m_isSingleTouchBegin = false;
        }
    }

    protected void OnDoubleTouchBegin(Vector2 mousePosition, float dis)
    {
#if UNITY_EDITOR
        m_simulateDis = 0.5f;
        dis = m_simulateDis;
#endif
        m_DoubleTouchBegin.Invoke(mousePosition, dis);
    }

    protected void OnDoubleTouchMoving(Vector2 mousePosition, float dis)
    {
#if UNITY_EDITOR
        m_simulateDis += Input.mouseScrollDelta.y * m_deltaScale;
        m_simulateDis = Mathf.Clamp(m_simulateDis, 0, 1);
        dis = m_simulateDis;
#endif
        m_DoubleTouchMoving.Invoke(mousePosition, dis);
    }

    protected void OnDoubleTouchEnd(Vector2 mousePosition, float dis)
    {
        m_DoubleTouchEnd.Invoke(mousePosition, dis);
    }

    protected bool IsObstacleUI()
    {
        if (!string.IsNullOrEmpty(allowTag))
        {
            if (null != EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.CompareTag(allowTag))
            {
                return false;
            }

            return true;
        }
        if (!string.IsNullOrEmpty(forbiddenTag))
        {
            if (null != EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.CompareTag(forbiddenTag))
            {
                return true;
            }
            return false;
        }

        return UIObstacle;
    }

    public GameObject currentSelectedGameObject
    {
        get
        {
            return EventSystem.current.currentSelectedGameObject;
        }
    }
}                                                                                        
