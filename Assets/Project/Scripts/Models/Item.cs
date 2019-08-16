using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public int databaseID;
    public Sprite itemIcon;
    public GameObject itemModel;
    public bool isConsomable = true;
    public bool isStackable = true;
    public int maxStackableSize = 10;

    public string itemName;

    #region delegate
    public delegate void ItemHasBeenPickedUP();
    public ItemHasBeenPickedUP itemHasBeenPickedUP;
    #endregion

    public virtual void effect(Being sender = null, Item target = null)
    {
    }

    /// <summary>
    /// pick up the item
    /// </summary>
    /// <param name="sender">The sender where the target inventory is</param>
    /// <returns>return true if the item has been picked</returns>
    public virtual bool pickUP(Player sender)
    {
        bool success = sender.inventory.addToInventory(this);
        if(success)
            itemHasBeenPickedUP();

        return success;
    }
}
