using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject fullInventory;
    [SerializeField] GameObject smallInventory;
    [SerializeField] GameObject inventorySlotPrefab;

    [Header("Settings")]
    [SerializeField] float numberOfSlotPerLine = 7;
    [SerializeField] float numberOfLines = 5;

    private void Awake()
    {
        setInventorySizes(fullInventory);
        setInventorySizes(smallInventory);
        setInventorySlots(fullInventory);
        setInventorySlots(smallInventory);
    }

    public void showInventory(bool small)
    {
        if (small)
            smallInventory.SetActive(!smallInventory.activeSelf);
        else
            fullInventory.SetActive(!fullInventory.activeSelf);
    }

    void setInventorySizes(GameObject inventoryToFill)
    {
        float invWith = inventoryToFill.GetComponent<RectTransform>().rect.width;
        float spacing = inventoryToFill.GetComponentInChildren<GridLayoutGroup>().spacing.x;
        float totalSpacing = spacing * (numberOfSlotPerLine - 1);
        float cellWidth = (invWith - totalSpacing) / (numberOfSlotPerLine +1);
        Vector2 cellSize = new Vector2(cellWidth, cellWidth);

        inventoryToFill.GetComponentInChildren<GridLayoutGroup>().cellSize = cellSize;
    }

    void setInventorySlots(GameObject inventoryToFill)
    {
        for(int i = 0; i < numberOfSlotPerLine * numberOfLines; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab);
            newSlot.transform.SetParent(inventoryToFill.GetComponentInChildren<GridLayoutGroup>().transform);
        }
    }
}
