﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InventorySlot 
{
    private int _index;
    public int index { get { return _index; } }
    public Item item;
    public int quantity;

    public InventorySlot(int index)
    {
        this._index = index;
        this.item = null;
        this.quantity = -1;
    }

    public void emptySlot()
    {
        item = null;
        quantity = -1;
    }

    public void updateIndex(int index)
    {
        this._index = index;
    }

    /// <summary>
    /// Copy the slot into the current one 
    /// </summary>
    /// <param name="slot">The slot to copy</param>
    public void copySlot(InventorySlot slot)
    {
        item = slot.item;
        quantity = slot.quantity;
    }


    /// <summary>
    /// Compare the current slot to an other slot
    /// </summary>
    /// <param name="slot">slot to compare</param>
    /// <returns>false if different and true if not</returns>
    public bool isEqualTo(InventorySlot slot)
    {
        if (slot.item != item)
            return false;
        if (slot.quantity != quantity)
            return false;
        if (slot.index != index)
            return false;

        return true;
    }

    /// <summary>
    /// Remove the item from the slot, if the item is stackable then remove quantity from it except if the quantity is zero
    /// </summary>
    public void removeItem()
    {
        if (item.isStackable)
        {
            quantity--;
            if (quantity <= 0)
            {
                item = null;
                quantity = -1;
            }
        }else
        {
            item = null;
        }
    }


    /// <summary>
    /// Add an item to the slot, except if the item is stackable then add quantity
    /// </summary>
    /// <param name="item">The item to add</param>
    public void AddItem(Item item)
    {
        if(this.item != null && this.item.isConsomable)
        {
            this.quantity ++;
        }
        else
        {
            this.item = item;
            if (item.isStackable)
                this.quantity = 1;
        }
    }
}
