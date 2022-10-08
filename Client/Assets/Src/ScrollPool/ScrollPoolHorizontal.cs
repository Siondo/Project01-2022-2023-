using UnityEngine;

public class ScrollPoolHorizontal : ScrollPool
{
    //配置参数
    public float spacing;


    public override void SetListPivot()
    {
        RectTransform rectList = scrollList.GetComponent<RectTransform>();
        rectList.pivot = new Vector2(0, rectList.pivot.y);
    }

    public override int GetMaxPoolSize()
    {
        float viewWidth = scrollRect.GetComponent<RectTransform>().rect.width;
        float sizeX = viewWidth % (spacing + cardSize.x);
        int maxSize = (int)(viewWidth / (spacing + cardSize.x));
        if (sizeX > spacing)
        {
            maxSize += 2;
        }
        else
        {
            maxSize += 1;
        }
        return maxSize;
    }

    public override void InitListSize(int num)
    {
        RectTransform rectList = scrollList.GetComponent<RectTransform>();
        float listWidth = padding.left + cardSize.x + (num - 1) * (spacing + cardSize.x) + padding.right;
        rectList.sizeDelta = new Vector2(num > 0 ? listWidth : 0, rectList.sizeDelta.y);
    }

    public override int GetStartIndex()
    {
        float posX = scrollList.GetComponent<RectTransform>().anchoredPosition.x;
        float disX;
        int startIndex;
        if (posX > 0)
        {
            posX = 0;
        }
        disX = Mathf.Abs(posX);
        if (disX >= cardSize.x + padding.left)
        {
            startIndex = (int)Mathf.Floor((disX - padding.left - cardSize.x) / (spacing + cardSize.x)) + 2;
        }
        else
        {
            startIndex = 1;
        }
        return startIndex;
    }

    public override void InitCardPosAndName(int index, GameObject card)
    {
        card.name = string.Format("obj[{0}]", index);
        float posX = (index - 1) * (cardSize.x + spacing) + padding.left;
        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, -padding.top);
    }

    public override Vector2 GetTargetPos(int index)
    {
        float posX;
        float posY = scrollList.GetComponent<RectTransform>().anchoredPosition.y;

        float listWidth = scrollList.GetComponent<RectTransform>().rect.width;
        float scrollWidth = scrollRect.GetComponent<RectTransform>().rect.width;

        if (listWidth <= scrollWidth)
        {
            posX = 0;
        }
        else
        {
            if (index <= 1)
            {
                posX = 0;
            }
            else
            {
                posX = Mathf.Clamp((index - 1) * (cardSize.x + spacing) + padding.left + locationOffset, 0, listWidth - scrollWidth);
            }
        }

        return new Vector2(-posX, posY);
    }
}
