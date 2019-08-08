using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public Sprite itemIcon;
    public GameObject itemModel;

    public string itemName;

    #region delegate
    public delegate void ItemHasBeenPickedUP();
    public ItemHasBeenPickedUP itemHasBeenPickedUP;
    #endregion

    public virtual void effect(Being sender = null, Item target = null)
    {
    }

    public virtual bool pickUP(Player sender)
    {
        bool success = sender.inventory.addToInventory(this);
        if(success)
            itemHasBeenPickedUP();

        return success;
    }
}
