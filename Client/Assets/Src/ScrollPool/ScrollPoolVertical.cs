using UnityEngine;

public class ScrollPoolVertical : ScrollPool
{

    public float spacing;

    public override void SetListPivot()
    {
        RectTransform rectList = scrollList.GetComponent<RectTransform>();
        rectList.pivot = new Vector2(rectList.pivot.x, 1);
    }

    public override int GetMaxPoolSize()
    {
        float viewHeight = scrollRect.GetComponent<RectTransform>().rect.height;
        float sizeY = viewHeight % (spacing + cardSize.y);
        int maxSize = (int)(viewHeight / (spacing + cardSize.y));
        if (sizeY > spacing)
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
        float listHeight = padding.top + cardSize.y + (num - 1) * (spacing + cardSize.y) + padding.bottom;
        rectList.sizeDelta = new Vector2(rectList.sizeDelta.x, num > 0 ? listHeight : 0);
    }

    public override int GetStartIndex()
    {
        float posY = scrollList.GetComponent<RectTransform>().anchoredPosition.y;
        float disY;
        int startIndex;
        if (posY < 0)
        {
            posY = 0;
        }
        disY = Mathf.Abs(posY);
        if (disY >= cardSize.y + padding.top)
        {
            startIndex = (int)Mathf.Floor((disY - padding.top - cardSize.y) / (spacing + cardSize.y)) + 2;
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
        float posY = (index - 1) * (cardSize.y + spacing) + padding.top;
        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(padding.left, -posY);
    }

    public override Vector2 GetTargetPos(int index)
    {
        float posX = scrollList.GetComponent<RectTransform>().anchoredPosition.x;
        float posY;

        float listHeight = scrollList.GetComponent<RectTransform>().rect.height;
        float scrollHeight = scrollRect.GetComponent<RectTransform>().rect.height;

        if (listHeight <= scrollHeight)
        {
            posY = 0;
        }
        else
        {
            if (index <= 1)
            {
                posY = 0;
            }
            else
            {
                posY = Mathf.Clamp((index - 1) * (cardSize.y + spacing) + padding.top + locationOffset, 0, listHeight - scrollHeight);
            }
        }

        return new Vector2(posX, posY);
    }
}
