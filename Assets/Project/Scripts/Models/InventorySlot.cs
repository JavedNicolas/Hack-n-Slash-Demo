using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InventorySlot 
{
    private int _index;
    public int index { get { return _index; } }
    public Item item;
    public float quantity;

    public InventorySlot(int index)
    {
        this._index = index;
        this.item = null;
        this.quantity = -1;
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
            quantity++;
        }
        else
        {
            this.item = item;
            if (item.isStackable)
                quantity = 1;
        }
    }
}
