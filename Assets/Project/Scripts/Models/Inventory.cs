using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory
{
    // Slots count
    public int numberOfSlot = 10;

    const int _minimumSlotsCount = 5;
    public int minimumSlotsCount { get => _minimumSlotsCount; }

    const int _maximumSlotsCount = 30;
    public int maximumSlotsCoun { get => _maximumSlotsCount; }

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
    /// Incrase the number of slot in the inventory
    /// </summary>
    /// <param name="addionnalSlot">slot to add</param>
    /// <returns></returns>
    public bool updateInventorySize(int targetNumberOfSlot)
    {
        List<int> indexesToRemove = new List<int>();

        if(slots.Count > targetNumberOfSlot)
        {
            int numberOfSlotToRemove =  slots.Count - targetNumberOfSlot;
            indexesToRemove = slotsThatCanBeRemoved(numberOfSlotToRemove);
            if (indexesToRemove.Count == 0)
                return false;
        }

        while(slots.Count < targetNumberOfSlot && slots.Count < _maximumSlotsCount)
        {
            slots.Add(new InventorySlot());
            numberOfSlot = slots.Count;
        }

        while(slots.Count > targetNumberOfSlot && slots.Count > _minimumSlotsCount)
        {
            if (indexesToRemove.Count == 0)
            {
                updateSlotsIndex();
                return true;
            }
                
            slots.RemoveAt(indexesToRemove[indexesToRemove.Count - 1]);
            indexesToRemove.RemoveAt(indexesToRemove.Count - 1);
            numberOfSlot = slots.Count;
        }

        updateSlotsIndex();
        numberOfSlot = slots.Count;

        return true;
    }

    void updateSlotsIndex()
    {
        for(int i = 0; i < _slots.Count; i++)
        {
            _slots[i].updateIndex(i);
        }
    }

    /// <summary>
    /// Return the slot which can be Remove
    /// </summary>
    /// <param name="numberOfSlotToRemove"></param>
    /// <returns>return an empty list if there is not evnough slot to remove, or return the indexes that can be removed</returns>
    List<int> slotsThatCanBeRemoved(int numberOfSlotToRemove)
    {
        List<int> indexesToRemove = new List<int>();
        int numberOfIndexToReturn = slots.Count - numberOfSlotToRemove < _minimumSlotsCount ? slots.Count - _minimumSlotsCount : numberOfSlotToRemove;

        for(int i =0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
                indexesToRemove.Add(i);
        }

        if (indexesToRemove.Count < numberOfIndexToReturn)
            return new List<int>();

        return indexesToRemove;
    }


    /// <summary>
    /// Add item to the inventory 
    /// </summary>
    /// <param name="itemToAdd">The item to Add</param>
    /// <returns>return true if added to the inventory, return false if the inventory is full</returns>
    public bool addToInventory(Item itemToAdd, int quantityToAdd = 1)
    {
        int availableSlotIndex = -1;

        if (!_slots.Exists(x => x.item == null))
            return false;

        // check if there is enough space to add the item
        int numberOfEmptySlot = _slots.FindAll(x => x.item == null).Count;
        if (!itemToAdd.isStackable)
        {
            if (numberOfEmptySlot < quantityToAdd)
                return false;
        }else
        {
            // Check how many space there is for this item and compare it the quantity we want to add
            int availableQuantity = 0;
            List<InventorySlot> slotsWithThisItem = _slots.FindAll(x => x.item?.databaseID == itemToAdd.databaseID);
            for(int i = 0; i < slotsWithThisItem.Count; i++)
                availableQuantity += itemToAdd.maxStackableSize - slotsWithThisItem[i].quantity;

            availableQuantity += numberOfEmptySlot * itemToAdd.maxStackableSize;

            if (availableQuantity < quantityToAdd)
                return false;
        }

        // find if there is currently a non full stack of this item
        for (int i = 0; i < quantityToAdd; i++)
        {
            // If the item is stackable
            if (itemToAdd.isStackable)
            {
                // check if there is a slot with the same item and enough space for it
                availableSlotIndex = _slots.FindIndex(x => (x.item?.databaseID == itemToAdd.databaseID && x.quantity < itemToAdd.maxStackableSize));

                // if there is none 
                if (availableSlotIndex == -1)
                    availableSlotIndex = _slots.FindIndex(x => x.item == null);
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
