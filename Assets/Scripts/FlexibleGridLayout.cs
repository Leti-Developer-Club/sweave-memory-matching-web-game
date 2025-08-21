using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FlexibleGridLayoutGroup : LayoutGroup
{
    [Header("Grid Settings")]
    public int rows = 2;
    public int cols = 4;
    public Vector2 spacing = new Vector2(5, 5);

    [HideInInspector]
    public Vector2 cellSize;

    public void SetGridSize(int r, int c)
    {
        rows = r;
        cols = c;
        SetDirty();
    }

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float totalSpacingX = spacing.x * (cols - 1) + padding.left + padding.right;
        float totalSpacingY = spacing.y * (rows - 1) + padding.top + padding.bottom;

        float cellWidth = (parentWidth - totalSpacingX) / cols;
        float cellHeight = (parentHeight - totalSpacingY) / rows;

        cellSize = new Vector2(cellWidth, cellHeight);
    }

    public override void CalculateLayoutInputVertical()
    {
        // Required by LayoutGroup, not used
    }

    public override void SetLayoutHorizontal()
    {
        ArrangeChildren();
    }

    public override void SetLayoutVertical()
    {
        ArrangeChildren();
    }

    private void ArrangeChildren()
    {
        if (rectChildren.Count == 0)
            return;

        float totalWidth = cols * cellSize.x + (cols - 1) * spacing.x;
        float totalHeight = rows * cellSize.y + (rows - 1) * spacing.y;

        // Calculate starting position (bottom-left corner?)
        float startX = gameObject.transform.localPosition.x;
        float startY = gameObject.transform.localPosition.y;


        for (int i = 0; i < rectChildren.Count; i++)
        {
            int row = i / cols;
            int col = i % cols;


            float xPos;
            float yPos;
            // Calculate proper grid positions
            xPos = startX + col * (cellSize.x + spacing.x);
            yPos = startY + row * (cellSize.y + spacing.y);

            SetChildAlongAxis(rectChildren[i], 0, xPos, cellSize.x);
            SetChildAlongAxis(rectChildren[i], 1, yPos, cellSize.y);

            // Also try MemoryCard component for UI-based cards
            MemoryCard MemoryCardScript = rectChildren[i].GetComponent<MemoryCard>();
            if (MemoryCardScript != null)
            {
                MemoryCardScript.UpdateSpriteSize(cellSize);
            }
            else
            {
                Debug.Log("No MemoryCard component found");
            }
        }
    }
}
