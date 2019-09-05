using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        int availableSlotIndex = -1;

        // find if there is currently a non full stack of this item
        for (int i = 0; i < quantityToAdd; i++)
        {
            // If the item is stackable
            if (itemToAdd.isStackable)
            {
                // check if there is a slot with the same item and enough space for it
                availableSlotIndex = _slots.FindIndex(x => (x.item != null && x.item.databaseID == itemToAdd.databaseID  && x.quantity + quantityToAdd <= itemToAdd.maxStackableSize));

                // if there is none 
                if (availableSlotIndex == -1)
                {
                    // then check if there is a free slot if so, check if there is a slot with space for one item 
                    //if not there is no space left in the inventory for this item
                    if (_slots.Exists(x => x.item == null))
                        availableSlotIndex = _slots.FindIndex(x => (x.item != null && x.item.databaseID == itemToAdd.databaseID && x.quantity < itemToAdd.maxStackableSize));
                    else
                        return false;
                }
            }

            // if the item is not stackable or there is no 
            //slot with the same item and space in it, then we check for an empty slot
            if (availableSlotIndex == -1)
                for (int j = 0; j < _slots.Count; j++)
                {
                    if (_slots[j].item == null)
                    {
                        availableSlotIndex = j;
                        break;
                    }
                }

            additem(itemToAdd, availableSlotIndex);
            availableSlotIndex = -1;
        }

        return true;
    }

    void additem(Item itemToAdd, int slotIndex)
    {
        InventorySlot slot = _slots[slotIndex];
        slot.AddItem(itemToAdd);
        _slots[slotIndex] = slot;
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
    }
}
