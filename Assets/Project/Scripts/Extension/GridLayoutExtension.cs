using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Orientation
{
    Horizontal, Vertical
}

public static class GRidLayoutExtension
{
    /// <summary>
    /// Set the size of each cell to fit the parent
    /// </summary>
    public static void setCellSize(this GridLayoutGroup gridLayout, Orientation orientationToSizeOn, int numberOfElement, RectTransform gridLayoutRectTransform)
    {
        if(gridLayoutRectTransform != null)
        {
            float gridLayoutSizeWithoutPadding = 0;

            if (orientationToSizeOn == Orientation.Horizontal)
            {
                float gridLayoutSize = gridLayoutRectTransform.rect.width;
                gridLayoutSizeWithoutPadding = gridLayoutSize - gridLayout.padding.left - gridLayout.padding.right - (gridLayout.spacing.x * (numberOfElement - 1));
            }  
            else if (orientationToSizeOn == Orientation.Vertical)
            {
                float gridLayoutSize = gridLayoutRectTransform.rect.height;
                gridLayoutSizeWithoutPadding = gridLayoutSize - gridLayout.padding.top - gridLayout.padding.bottom - (gridLayout.spacing.y * (numberOfElement - 1));
            }
                

            float cellSize = gridLayoutSizeWithoutPadding / numberOfElement;

            gridLayout.cellSize = new Vector2(cellSize, cellSize);
        }
    }
}
