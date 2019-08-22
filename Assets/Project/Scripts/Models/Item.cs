using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Item : Interactable
{
    public int databaseID;
    public Sprite itemIcon;
    public GameObject itemModel;
    public bool isConsomable = true;
    public bool isStackable = true;
    public int maxStackableSize = 10;

    #region delegate
    public delegate void ItemHasBeenPickedUP();
    public ItemHasBeenPickedUP itemHasBeenPickedUP;
    #endregion

    public Item()
    {
        setDistanceToInteraction(2f);
    }

    public virtual void use(Being sender = null, Item target = null)
    {
    }

    public override bool interact(BeingBehavior sender, GameObject objectToInteractWith)
    {
        return pickUP((Player)sender.being);
    }

    /// <summary>
    /// pick up the item
    /// </summary>
    /// <param name="sender">The sender where the target inventory is</param>
    /// <returns>return true if the item has been picked</returns>
    public virtual bool pickUP(Player player)
    {
        bool success = player.inventory.addToInventory(this);
        itemHasBeenPickedUP();

        return success;
    }


}
