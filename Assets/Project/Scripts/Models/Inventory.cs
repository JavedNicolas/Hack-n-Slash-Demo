using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    const int numberOfSlot = 10;
    List<InventorySlot> _slots = new List<InventorySlot>();
    public List<InventorySlot> slots {  get { return _slots; } }

    public Inventory()
    {
        for(int i = 0; i < numberOfSlot; i++)
        {
            _slots.Add(new InventorySlot());
        }
    }

    public bool addToInventory(Item itemToAdd)
    { 
        for(int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].item == null)
            {
                InventorySlot slot = _slots[i];
                slot.item = itemToAdd;
                _slots[i] = slot;
                return true;
                
            }
        }

        return false;
    }
}
