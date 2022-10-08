using UnityEngine;

public enum Axis
{
    Horizontal = 1,
    Vertical = 2,
}

public class ScrollPoolGrid : ScrollPool
{
    public Vector2 spacing;
    public Axis startAxis = Axis.Horizontal;

    private int column;
    private int row;

    public override void SetListPivot()
    {
        RectTransform rectList = scrollList.GetComponent<RectTransform>();
        if (startAxis == Axis.Horizontal)
        {
            rectList.pivot = new Vector2(rectList.pivot.x, 1);
        }
        else if (startAxis == Axis.Vertical)
        {
            rectList.pivot = new Vector2(0, rectList.pivot.y);
        }
    }

    public override int GetMaxPoolSize()
    {
        int poolSize = 0;

        float viewWidth = scrollRect.GetComponent<RectTransform>().rect.width;
        float viewHeight = scrollRect.GetComponent<RectTransform>().rect.height;

        if (startAxis == Axis.Horizontal)
        {
            column = Mathf.FloorToInt((viewWidth - padding.left - padding.right - cardSize.x) / (cardSize.x + spacing.x)) + 1;
            float remaindHeight = viewHeight % (spacing.y + cardSize.y);
            int maxViewRow = (int)(viewHeight / (spacing.y + cardSize.y));
            if (remaindHeight > spacing.y)
            {
                maxViewRow += 2;
            }
            else
            {
                maxViewRow += 1;
            }
            poolSize = maxViewRow * column;
        }
        else if (startAxis == Axis.Vertical)
        {
            row = Mathf.FloorToInt((viewHeight - padding.top - padding.bottom - cardSize.y) / (cardSize.y + spacing.y)) + 1;
            float remaindWidth = viewWidth % (spacing.x + cardSize.x);
            int maxViewColumn = (int)(viewWidth / (spacing.x + cardSize.x));
            if (remaindWidth > spacing.x)
            {
                maxViewColumn += 2;
            }
            else
            {
                maxViewColumn += 1;
            }
            poolSize = maxViewColumn * row;
        }

        return poolSize;
    }

    public override void InitListSize(int num)
    {
        RectTransform rectList = scrollList.GetComponent<RectTransform>();
        if (startAxis == Axis.Horizontal)
        {
            row = Mathf.CeilToInt((float)num / column);
            float height = padding.top + row * cardSize.y + (row - 1) * spacing.y + padding.bottom;
            rectList.sizeDelta = new Vector2(rectList.sizeDelta.x, num > 0 ? height : 0);
        }
        else if (startAxis == Axis.Vertical)
        {
            column = Mathf.CeilToInt((float)num / row);
            float width = padding.left + column * cardSize.x + (column - 1) * spacing.x + padding.right;
            rectList.sizeDelta = new Vector2(num > 0 ? width : 0, rectList.sizeDelta.y);
        }
    }

    public override int GetStartIndex()
    {
        int startIndex = 1;
        if (startAxis == Axis.Horizontal)
        {
            float posY = scrollList.GetComponent<RectTransform>().anchoredPosition.y;
            float disY;
            if (posY < 0)
            {
                posY = 0;
            }
            disY = Mathf.Abs(posY);
            if (disY >= cardSize.y + padding.top)
            {
                int target_row = Mathf.FloorToInt((disY - padding.top - cardSize.y) / (cardSize.y + spacing.y)) + 1;
                startIndex = target_row * column + 1;
            }
            else
            {
                startIndex = 1;
            }
        }
        else if (startAxis == Axis.Vertical)
        {
            float posX = scrollList.GetComponent<RectTransform>().anchoredPosition.x;
            float disX;
            if (posX > 0)
            {
                posX = 0;
            }
            disX = Mathf.Abs(posX);
            if (disX >= cardSize.x + padding.left)
            {
                int target_column = Mathf.FloorToInt((disX - padding.left - cardSize.x) / (cardSize.x + spacing.x)) + 1;
                startIndex = target_column * row + 1;
            }
            else
            {
                startIndex = 1;
            }
        }
        return startIndex;
    }

    public override void InitCardPosAndName(int index, GameObject card)
    {
        int targetRow = 1;
        int targetColumn = 1;

        if (startAxis == Axis.Horizontal)
        {
            targetRow = Mathf.CeilToInt((float)index / column);
            targetColumn = index - (targetRow - 1) * column;
            card.name = string.Format("obj[{0}][{1}]", targetRow, targetColumn);
        }
        else if (startAxis == Axis.Vertical)
        {
            targetColumn = Mathf.CeilToInt((float)index / row);
            targetRow = index - (targetColumn - 1) * row;
            card.name = string.Format("obj[{0}][{1}]", targetColumn, targetRow);
        }

        float posX = padding.left + (targetColumn - 1) * (spacing.x + cardSize.x);
        float posY = padding.top + (targetRow - 1) * (spacing.y + cardSize.y);
        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, -posY);
    }

    public override Vector2 GetTargetPos(int index)
    {
        float posX = 0;
        float posY = 0;

        if (startAxis == Axis.Horizontal)
        {
            float listHeight = scrollList.GetComponent<RectTransform>().rect.height;
            float scrollHeight = scrollRect.GetComponent<RectTransform>().rect.height;

            posX = scrollList.GetComponent<RectTransform>().anchoredPosition.x;

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
                    int targetRow = Mathf.CeilToInt((float)index / column);
                    posY = Mathf.Clamp(padding.top + (targetRow - 1) * (spacing.y + cardSize.y) + locationOffset, 0, listHeight - scrollHeight);
                }
            }
        }
        else if (startAxis == Axis.Vertical)
        {
            float listWidth = scrollList.GetComponent<RectTransform>().rect.width;
            float scrollWidth = scrollRect.GetComponent<RectTransform>().rect.width;

            posY = scrollList.GetComponent<RectTransform>().anchoredPosition.y;

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
                    int targetColumn = Mathf.CeilToInt((float)index / row);
                    posX = -(Mathf.Clamp(padding.left + (targetColumn - 1) * (spacing.x + cardSize.x) + locationOffset, 0, listWidth - scrollWidth));
                }
            }
        }

        return new Vector2(posX, posY);
    }

}
