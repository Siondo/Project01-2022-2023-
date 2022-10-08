using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using XLua;

public class ScrollPool : MonoBehaviour
{
    [System.Serializable]
    public struct Padding
    {
        public int left;
        public int right;
        public int top;
        public int bottom;
    }

    //配置参数
    public GameObject scrollRect;
    public GameObject scrollList;
    public GameObject prefab;
    public Padding padding;
    public float locationOffset = 0;
    [System.NonSerialized]
    public Vector2 cardSize;

    //内部参数
    private List<GameObject> pool = new List<GameObject>();
    private Dictionary<int, GameObject> curCards = new Dictionary<int, GameObject>();
    private List<int> curView = new List<int>();

    //[SerializeField]
    private int curStartIndex = 1;
    private int userTargetIndex;
    private int dataNum;
    //private float minPosX;
    //[SerializeField]
    private int poolSize;

    private bool readyBase = false;
    private bool readyStart = false;
    private bool readyData = false;

    private Action userInitFinishCb;

    public Action<int,GameObject> updateCallBack;

    public ScrollRect Scroll { get; set; }

    void Awake()
    {
        Scroll = scrollRect.GetComponent<ScrollRect>();
        Scroll.onValueChanged.AddListener(OnScrollHandler);
    }

    void Start()
    {

    }

    void OnEnable()
    {
        if (!readyBase)
        {
            InitBase();
        }
    }

    public void SetUpdateCallBack(Action<int, GameObject> callBack)
    {
        updateCallBack = callBack;
    }

    public void OnInitFinish(System.Action callback)
    {
        userInitFinishCb = callback;
    }

    public void InitPool(int objNum)
    {
        InitPool(objNum, 0);
    }

    public void InitPool(int objNum, int targetIndex)
    {
        ReleaseAllCard();
        dataNum = objNum;
        userTargetIndex = targetIndex;

        readyData = true;
        if (!readyBase)
        {
            if (gameObject.activeInHierarchy)
            {
                InitBase();
            }
        }
        else
        {
            DoInit();
        }
    }

    public void ReloadConfig()
    {
        readyBase = false;
        Scroll.onValueChanged.RemoveListener(OnScrollHandler);
    }

    private void DoInit()
    {
        if (!CheckReady())
        {
            return;
        }

        poolSize = GetMaxPoolSize();
        InitListSize(dataNum);
        SetListAtIndex(userTargetIndex);

        curStartIndex = GetViewStartIndex();
        List<int> curViewList = GetCurViewList();
        for (int i = 0; i < curViewList.Count; i++)
        {
            InitCard(curViewList[i]);
        }

        userInitFinishCb?.Invoke();
    }

    private bool CheckReady()
    {
        return readyBase && readyStart && readyData;
    }

    public virtual void SetListPivot()
    {

    }

    public void SetListAtIndex(int index)
    {
        if (index == 0)
        {
            return;
        }
        Vector2 targetPos = GetTargetPos(index);

        scrollList.GetComponent<RectTransform>().anchoredPosition = targetPos;
    }

    public virtual Vector2 GetTargetPos(int index)
    {
        return new Vector2(0, 0);
    }

    public virtual void InitListSize(int num)
    {

    }

    GameObject GetCard()
    {
        if (pool.Count > 0)
        {
            GameObject cacheCard = pool[0];
            pool.RemoveAt(0);
            return cacheCard;
        }
        else
        {
            GameObject newCard = GameObject.Instantiate(prefab, scrollList.transform, false);
            return newCard;
        }
    }

    void ReleaseCard(int index)
    {
        if (curCards.ContainsKey(index))
        {
            AddToPool(curCards[index]);
            curCards.Remove(index);
        }
    }

    void ReleaseAllCard()
    {
        List<int> buffer = new List<int>();
        foreach (var key in curCards.Keys)
        {
            buffer.Add(key);
        }
        buffer.Sort();
        for (int i = 0; i < buffer.Count; i++)
        {
            ReleaseCard(buffer[i]);
        }
    }

    public virtual int GetStartIndex()
    {
        return 1;
    }

    int GetViewStartIndex()
    {
        int startIndex = GetStartIndex();
        if (startIndex > dataNum - poolSize + 1)
        {
            startIndex = dataNum - poolSize + 1;
        }
        else if (startIndex < 1)
        {
            startIndex = 1;
        }
        return startIndex;
    }

    List<int> GetCurViewList()
    {
        List<int> viewList = new List<int>();
        if (dataNum > poolSize)
        {
            for (int i = 0; i < poolSize; i++)
            {
                viewList.Add(curStartIndex + i);
            }
        }
        else
        {
            for (int i = 0; i < dataNum; i++)
            {
                viewList.Add(i + 1);
            }
        }
        return viewList;
    }

    public int[] GetCurView()
    {
        return GetCurViewList().ToArray();
    }

    void OnScrollHandler(Vector2 vec)
    {
        if (dataNum > poolSize)
        {
            int startIndex = GetViewStartIndex();
            if (startIndex < curStartIndex)
            {
                int delta = curStartIndex - startIndex;
                for (int i = 0; i < delta; i++)
                {
                    ReleaseCard(curStartIndex + poolSize - 1);
                    curStartIndex -= 1;
                    InitCard(curStartIndex);
                }
            }
            else if (startIndex > curStartIndex)
            {
                int delta = startIndex - curStartIndex;
                for (int i = 0; i < delta; i++)
                {
                    ReleaseCard(curStartIndex);
                    curStartIndex += 1;
                    InitCard(curStartIndex + poolSize - 1);
                }
            }
        }
    }

    void InitCard(int index)
    {
        if (!curCards.ContainsKey(index))
        {
            curCards[index] = GetCard();
            RectTransform rect = curCards[index].GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);

            rect.sizeDelta = cardSize;
        }
        InitCardPosAndName(index, curCards[index]);
        curCards[index].transform.SetSiblingIndex(index - curStartIndex);
        curCards[index].SetActive(true);
        updateCallBack(index, curCards[index]);
    }

    public virtual void InitCardPosAndName(int index, GameObject card)
    {

    }
    public virtual int GetMaxPoolSize()
    {
        return 1;
    }

    public void SetPadding(int left, int right, int top, int bottom)
    {
        padding.left = left;
        padding.right = right;
        padding.top = top;
        padding.bottom = bottom;
    }

    void InitBase()
    {
        ReInit();
        DoInit();
    }

    void AddToPool(GameObject go)
    {
        if (go == null) return;

        go.SetActive(false);
        pool.Add(go);
    }

    public void ReInit()
    {
        readyBase = true;
        SetListPivot();
        SetListAtIndex(1);

        RectTransform rect = prefab.GetComponent<RectTransform>();

        cardSize = new Vector2(rect.rect.width, rect.rect.height);

        rect.pivot = new Vector2(0, 1);

        if (!readyStart)
        {
            AddToPool(prefab);
        }
        readyStart = true;
    }
}
