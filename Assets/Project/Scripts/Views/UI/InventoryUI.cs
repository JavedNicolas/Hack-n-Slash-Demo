using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject fullInventory;
    [SerializeField] GameObject smallInventory;
    [SerializeField] GameObject inventorySlotPrefab;

    [Header("Settings")]
    [SerializeField] float numberOfSlotPerLine = 7;

    private int numberOfSlot;

    public void loadInventory()
    {
        numberOfSlot = GameManager.instance.getPlayerInventory().numberOfSlot;
        setInventorySizes(fullInventory);
        setInventorySizes(smallInventory);
        addInventorySlots(fullInventory);
        addInventorySlots(smallInventory);
    }

    /// <summary>
    /// Display inventory
    /// </summary>
    /// <param name="small">if true display the small inventory</param>
    public void showInventory(bool small)
    {
        if (small)
        {
            smallInventory.SetActive(!smallInventory.activeSelf);
            if(smallInventory.activeSelf)
                fullInventory.SetActive(false);
        }
        else
        {
            fullInventory.SetActive(!fullInventory.activeSelf);
            if (fullInventory.activeSelf)
                smallInventory.SetActive(false);
        }
            
    }

    /// <summary>
    /// Update the inventory display
    /// </summary>
    /// <param name="inventory">The inventory to put</param>
    public void updateInventoryUI(Inventory inventory)
    {
        updateSlotsContent(inventory, fullInventory);
        updateSlotsContent(inventory, smallInventory);
    }

    void updateSlotsContent(Inventory inventory, GameObject inventoryToFill)
    {
        List<InventorySlotUI> itemSlots = inventoryToFill.GetComponentsInChildren<InventorySlotUI>().ToList();
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            itemSlots[i].initSlot(inventory.slots[i]);
        }
    }

    /// <summary>
    /// Set the inventory slots && slots sizes
    /// </summary>
    /// <param name="inventoryToFill">Inventory to update (full or small)</param>
    void setInventorySizes(GameObject inventoryToFill)
    {
        float invWith = inventoryToFill.GetComponent<RectTransform>().rect.width;
        float spacing = inventoryToFill.GetComponentInChildren<GridLayoutGroup>().spacing.x;
        float totalSpacing = spacing * (numberOfSlotPerLine - 1);
        float cellWidth = (invWith - totalSpacing) / (numberOfSlotPerLine +1);
        Vector2 cellSize = new Vector2(cellWidth, cellWidth);

        inventoryToFill.GetComponentInChildren<GridLayoutGroup>().cellSize = cellSize;
    }

    /// <summary>
    /// Add new slot to the inventory
    /// </summary>
    /// <param name="inventoryToFill">Inventory to update (full or small)</param>
    void addInventorySlots(GameObject inventoryToFill)
    {
        int numberOfSlotToAdd = this.numberOfSlot - inventoryToFill.GetComponentsInChildren<InventorySlotUI>().Count();

        for(int i = 0; i < numberOfSlotToAdd; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab);
            newSlot.transform.SetParent(inventoryToFill.GetComponentInChildren<GridLayoutGroup>().transform);
            newSlot.transform.localScale = new Vector3(1, 1, 1);
            if (newSlot.GetComponentInChildren<InventorySlotUI>())
            {
                GameManager.instance.leftClickDelegate += newSlot.GetComponentInChildren<InventorySlotUI>().leftClick;
                GameManager.instance.rightClickDelegate += newSlot.GetComponentInChildren<InventorySlotUI>().rightCLick;
            }
        }
    }

}
