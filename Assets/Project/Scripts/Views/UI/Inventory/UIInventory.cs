using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class UIInventory : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject fullInventory;
    [SerializeField] GameObject smallInventory;
    [SerializeField] GameObject inventorySlotPrefab;

    [Header("Settings")]
    [SerializeField] float _numberOfSlotPerLine = 7;

    private int _numberOfSlot;
    private Inventory _inventory;

    private void Update()
    {
        if(_inventory != null)
        {
            updateInventoryUI();
        }
    }

    public void loadInventory(Inventory inventory)
    {
        this._inventory = inventory;
        _numberOfSlot = _inventory.numberOfSlot;
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
    public void updateInventoryUI()
    {
        updateSlotsContent(fullInventory);
        updateSlotsContent(smallInventory);
    }

    void updateSlotsContent(GameObject inventoryToFill)
    {
        List<UIInventorySlot> itemSlots = inventoryToFill.GetComponentsInChildren<UIInventorySlot>().ToList();
        for (int i = 0; i < _inventory.slots.Count; i++)
        {
            itemSlots[i].initSlot(_inventory.slots[i], this);
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
        float totalSpacing = spacing * (_numberOfSlotPerLine - 1);
        float cellWidth = (invWith - totalSpacing) / (_numberOfSlotPerLine +1);
        Vector2 cellSize = new Vector2(cellWidth, cellWidth);

        inventoryToFill.GetComponentInChildren<GridLayoutGroup>().cellSize = cellSize;
    }

    /// <summary>
    /// Add new slot to the inventory
    /// </summary>
    /// <param name="inventoryToFill">Inventory to update (full or small)</param>
    void addInventorySlots(GameObject inventoryToFill)
    {
        int numberOfSlotToAdd = this._numberOfSlot - inventoryToFill.GetComponentsInChildren<UIInventorySlot>().Count();

        for(int i = 0; i < numberOfSlotToAdd; i++)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab);
            newSlot.transform.SetParent(inventoryToFill.GetComponentInChildren<GridLayoutGroup>().transform);
            newSlot.transform.localScale = new Vector3(1, 1, 1);
            if (newSlot.GetComponentInChildren<UIInventorySlot>())
            {
                GameManager.instance.leftClickDelegate += newSlot.GetComponentInChildren<UIInventorySlot>().leftClick;
                GameManager.instance.rightClickDelegate += newSlot.GetComponentInChildren<UIInventorySlot>().rightCLick;
            }
        }
    }

    /// <summary>
    /// update a slot in the player inventory
    /// </summary>
    /// <param name="slots"></param>
    public void updateInventorySlots(List<InventorySlot> slots)
    {
        _inventory.updateSlots(slots);
    }

}
