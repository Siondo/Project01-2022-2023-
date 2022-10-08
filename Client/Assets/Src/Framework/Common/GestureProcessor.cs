using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GestureProcessor
{
	public delegate void SingleTouch(Vector2 pos);
	public delegate void DoubleTouch(Vector2 pos, float dis);
	public event SingleTouch onSingleTouchBegin = null;
    public event SingleTouch onSingleMoving     = null;
    public event SingleTouch onSingleTouchEnd   = null;

    public event DoubleTouch onDoubleTouchBegin = null;
    public event DoubleTouch onDoubleTouchEnd   = null;
    public event DoubleTouch onDoubleMoving     = null;

	#region Properties.
	public bool UIObstacle = true;

    public delegate bool IsObstacleUI();
    public IsObstacleUI isObstacleUI
    {
        set; private get;
    }
    #endregion

    #region Member Variable.
    Vector3 mLastMousePosition = Vector3.zero;
    bool    mIsObstacleTouched = false;
    float   mSimulateDis       = 0;
    #endregion

    public void Update()
    {
        if (mIsObstacleTouched)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            { 
                mIsObstacleTouched = false; 
            }
            if (!Input.GetMouseButton(0))
            {
                mIsObstacleTouched = false;
            }
#else
            if(Input.touchCount != 0)
            {
                for (int i = 0; i < Input.touchCount; ++i)
                {
                    if (!(Input.touches[i].phase == TouchPhase.Canceled || 
                          Input.touches[i].phase == TouchPhase.Ended))
                    {
                        return;
                    }
                }  
            }
            mIsObstacleTouched = false;
#endif
        }
        else
        {
            bool    pressed    = false;
            Vector3 currentPos = Vector3.zero;
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                pressed    = true;
                currentPos = Input.mousePosition;
            }
#else
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    pressed    = true;
                    currentPos = Input.touches[0].position;
                }
            }
#endif
		    if(pressed)
			{
				if(UIObstacle)
				{
#if UNITY_EDITOR
					mIsObstacleTouched = EventSystem.current.IsPointerOverGameObject();
#else
				    mIsObstacleTouched = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
                    if (mIsObstacleTouched && null != isObstacleUI)
                    {
                        mIsObstacleTouched = isObstacleUI();
                    }
				}
			}
		}

        if (mIsObstacleTouched)
        {
            return;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            mSimulateDis = 0.5f;
            if (onDoubleTouchBegin != null)
            {
                onDoubleTouchBegin(Input.mousePosition, mSimulateDis);
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                mSimulateDis += Input.mouseScrollDelta.y / 10.0f;
                mSimulateDis  = Mathf.Clamp(mSimulateDis, 0, Mathf.Sqrt(2));
                if (onDoubleMoving != null)
                {
                    onDoubleMoving(Input.mousePosition, mSimulateDis);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (onDoubleTouchEnd != null)
            {
                onDoubleTouchEnd(Input.mousePosition, 0);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                mLastMousePosition = Input.mousePosition;
                if (onSingleTouchBegin != null)
                {
                    onSingleTouchBegin(Input.mousePosition);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (onSingleTouchEnd != null)
                {
                    onSingleTouchEnd(Input.mousePosition);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (mLastMousePosition != Input.mousePosition)
                {
                    if (onSingleMoving != null)
                    {
                        onSingleMoving(Input.mousePosition);
                    }
                    mLastMousePosition = Input.mousePosition;
                }
            }
        }
#else
        if (Input.touchCount == 2)
        {
            Touch t1, t2;
            t1 = Input.touches[0];
            t2 = Input.touches[1];

            bool t1_unpressed;
            bool t2_unpressed;

            t1_unpressed = t1.phase == TouchPhase.Ended || t1.phase == TouchPhase.Canceled;
            t2_unpressed = t2.phase == TouchPhase.Ended || t2.phase == TouchPhase.Canceled;

            Vector2 v1, v2;
            Camera  camera;

            if (t1_unpressed || t2_unpressed)
            {
                if (onDoubleTouchEnd != null)
                {
                    onDoubleTouchEnd(t1_unpressed ? t2.position : t1.position, 0);
                }
            }
            else if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                camera = Camera.main;
                v1 = new Vector2(t1.position.x / camera.pixelWidth, t1.position.y / camera.pixelHeight);
                v2 = new Vector2(t2.position.x / camera.pixelWidth, t2.position.y / camera.pixelHeight);

                if (onDoubleTouchBegin != null)
                {
                    onDoubleTouchBegin((t1.position + t2.position) / 2.0f, (v1 - v2).magnitude);
                }
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                camera = Camera.main;
                v1 = new Vector2(t1.position.x / camera.pixelWidth, t1.position.y / camera.pixelHeight);
                v2 = new Vector2(t2.position.x / camera.pixelWidth, t2.position.y / camera.pixelHeight);
                if (onDoubleMoving != null)
                {
                    onDoubleMoving((t1.position + t2.position) / 2.0f, (v1 - v2).magnitude);
                }
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch touch;
            touch = Input.touches[0];
            if (touch.phase == TouchPhase.Moved)
            {
                if (onSingleMoving != null)
                {
                    onSingleMoving(touch.position);
                }
            }
            else if (touch.phase == TouchPhase.Began)
            {
                if (onSingleTouchBegin != null)
                {
                    onSingleTouchBegin(touch.position);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (onSingleTouchEnd != null)
                {
                    onSingleTouchEnd(touch.position);
                }
            }
        }
#endif
    }
}                                                                                        
