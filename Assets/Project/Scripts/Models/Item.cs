using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class Item : Interactable
{
    public const float interactionDistance = 2f;

    Sprite _itemIcon;
    public Sprite itemIcon { get => _itemIcon; }
    GameObject _itemModel;
    public GameObject itemModel { get => _itemModel; }
    bool _isConsomable = true;
    public bool isConsomable { get => _isConsomable; }
    bool _isStackable = true;
    public bool isStackable { get => _isStackable; }
    int _maxStackableSize = 10;
    public int maxStackableSize { get => _maxStackableSize; }

    #region delegate
    public delegate void ItemHasBeenPickedUP();
    public ItemHasBeenPickedUP itemHasBeenPickedUP;
    #endregion

    public Item()
    {
        setDistanceToInteraction(interactionDistance);
    }

    public Item(string name, Sprite itemIcon, GameObject itemModel, bool isConsomable, bool isStackable, int maxStackableSize)
    {
        this.name = name;
        this._itemIcon = itemIcon;
        this._itemModel = itemModel;
        this._isConsomable = isConsomable;
        this._isStackable = isStackable;
        this._maxStackableSize = maxStackableSize;
        setDistanceToInteraction(interactionDistance);
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
