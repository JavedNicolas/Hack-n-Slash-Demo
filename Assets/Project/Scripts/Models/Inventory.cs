using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public int numberOfSlot = 10;
    List<InventorySlot> _slots = new List<InventorySlot>();
    public List<InventorySlot> slots {  get { return _slots; } }

    public Inventory()
    {
        for(int i = 0; i < numberOfSlot; i++)
        {
            _slots.Add(new InventorySlot(i));
        }
    }

    /// <summary>
    /// Add item to the inventory 
    /// </summary>
    /// <param name="itemToAdd">The item to Add</param>
    /// <returns>return true if added to the inventory, return false if the inventory is full</returns>
    public bool addToInventory(Item itemToAdd, int quantityToAdd = 1)
    {
        // find if there is currently a non full stack of this item
        int firstAvailableStackForThisItem = _slots.FindIndex(x => (x.item != null && x.item.databaseID == itemToAdd.databaseID && x.item.isStackable && x.quantity + quantityToAdd < itemToAdd.maxStackableSize));
        bool slotAvailable = false;

        if (firstAvailableStackForThisItem != -1)
            slotAvailable = true;
        else
            for(int i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].item == null)
                {
                    slotAvailable = true;
                    firstAvailableStackForThisItem = i;
                    break;
                }
            }

        if (slotAvailable)
        {
            InventorySlot slot = _slots[firstAvailableStackForThisItem];
            slot.AddItem(itemToAdd, quantityToAdd);
            _slots[firstAvailableStackForThisItem] = slot;
            inventoryChanged();
        }

        return slotAvailable;
    }

    /// <summary>
    /// Update a slot changed from the UI
    /// </summary>
    /// <param name="slotIndex">the slot index to update</param>
    /// <param name="slot">The slot to put instead of the current one</param>
    public void updateSlots(List<InventorySlot> updatedSlots)
    {
        for(int i = 0; i < updatedSlots.Count; i++)
        {
            _slots[updatedSlots[i].index] = updatedSlots[i];
        }
        
        inventoryChanged();
    }

    public void inventoryChanged()
    {
        GameUI.instance.updateInventoryUI(this);
    }
}
