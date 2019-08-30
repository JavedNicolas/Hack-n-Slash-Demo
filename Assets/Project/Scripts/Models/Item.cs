using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class Item : Interactable
{
    public const float interactionDistance = 6f;

    // Display
    [SerializeField] Sprite _itemIcon;
    public Sprite itemIcon { get => _itemIcon; }

    [SerializeField] GameObject _itemModel;
    public GameObject itemModel { get => _itemModel; }

    // attributs
    [SerializeField] bool _isConsomable = true;
    public bool isConsomable { get => _isConsomable; }

    [SerializeField] bool _isStackable = true;
    public bool isStackable { get => _isStackable; }

    [SerializeField] int _maxStackableSize = 10;
    public int maxStackableSize { get => _maxStackableSize; }

    [SerializeField] TargetType _targetType;
    public TargetType targetType { get => _targetType; }

    [SerializeField] List<EffectAndValue> _effects;
    public List<EffectAndValue> effects { get => _effects; }

    // Recycle
    [SerializeField] bool _canBeRecycle = false;
    public bool canBeRecycle { get => _canBeRecycle; }

    #region delegate
    public delegate void ItemHasBeenPickedUP();
    public ItemHasBeenPickedUP itemHasBeenPickedUP;
    #endregion

    public Item()
    {
        setDistanceToInteraction(interactionDistance);
    }

    public Item(Item item)
    {
        this._databaseID = item.databaseID;
        this.name = item.name;
        this._itemIcon = item.itemIcon;
        this._itemModel = item.itemModel;
        this._isConsomable = item.isConsomable;
        this._isStackable = item.isStackable;
        this._maxStackableSize = item.maxStackableSize;
        this._targetType = item.targetType;
        this._effects = item.effects;
        this._interactibleType = item.interactibleType;
        this._canBeRecycle = item._canBeRecycle;
        setDistanceToInteraction(interactionDistance);
    }

    public Item(string name, Sprite itemIcon, GameObject itemModel, bool isConsomable, bool isStackable, int maxStackableSize, bool canBeRecycled,
        TargetType targetType, List<EffectAndValue> effects)
    {
        this.name = name;
        this._itemIcon = itemIcon;
        this._itemModel = itemModel;
        this._isConsomable = isConsomable;
        this._isStackable = isStackable;
        this._maxStackableSize = maxStackableSize;
        this._targetType = targetType;
        this._effects = effects;
        this._canBeRecycle = canBeRecycled;
        setDistanceToInteraction(interactionDistance);
    }

    /// <summary>
    /// Use the item
    /// </summary>
    /// <param name="sender">Sender using the item</param>
    /// <param name="target">Target recyving the effect if the item is targeted</param>
    /// <returns>True if the item has been used, false if not</returns>
    public virtual bool use(BeingBehavior sender, GameObject target = null)
    {
        if ((targetType != TargetType.None && target != null) || targetType == TargetType.None)
        {
            // check if the the effects can be use
            for (int i = 0; i < _effects.Count; i++)
            {
                if (!effects[i].effect.canBeUsed(sender, target, effects[i].value))
                    return false;
            }

            for (int i = 0; i < _effects.Count; i++)
                effects[i].effect.use(sender, target, effects[i].value);
        }
        return true;
    }

    public override bool interact(BeingBehavior sender, GameObject objectToInteractWith)
    {
        ItemObject itemObject = objectToInteractWith.GetComponent<ItemObject>();
        if (itemObject != null)
        {
            return itemObject.loot.item.pickUP((Player)sender.being, itemObject.loot.quantity);
        }

        return false;
    }

    /// <summary>
    /// pick up the item
    /// </summary>
    /// <param name="sender">The sender where the target inventory is</param>
    /// <returns>return true if the item has been picked</returns>
    public virtual bool pickUP(Player player, int quantity)
    {
        bool success = player.inventory.addToInventory(this, quantity);
        if(success)
            itemHasBeenPickedUP();

        return success;
    }


}
