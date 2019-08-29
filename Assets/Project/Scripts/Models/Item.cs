using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class Item : Interactable
{
    public const float interactionDistance = 6f;

    Sprite _itemIcon;
    [SerializeField] public Sprite itemIcon { get => _itemIcon; }
    GameObject _itemModel;
    [SerializeField] public GameObject itemModel { get => _itemModel; }
    bool _isConsomable = true;
    [SerializeField] public bool isConsomable { get => _isConsomable; }
    bool _isStackable = true;
    [SerializeField] public bool isStackable { get => _isStackable; }
    int _maxStackableSize = 10;
    [SerializeField] public int maxStackableSize { get => _maxStackableSize; }
    TargetType _targetType;
    [SerializeField] public TargetType targetType { get => _targetType; }

    [SerializeField] EffectAndValues _effects;
    public EffectAndValues effects { get => _effects; }

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
        setDistanceToInteraction(interactionDistance);
    }

    public Item(string name, Sprite itemIcon, GameObject itemModel, bool isConsomable, bool isStackable, int maxStackableSize,TargetType targetType,EffectAndValues effects)
    {
        this.name = name;
        this._itemIcon = itemIcon;
        this._itemModel = itemModel;
        this._isConsomable = isConsomable;
        this._isStackable = isStackable;
        this._maxStackableSize = maxStackableSize;
        this._targetType = targetType;
        this._effects = effects;
        setDistanceToInteraction(interactionDistance);
    }

    public virtual void use(BeingBehavior sender, GameObject target = null)
    {
        if ((targetType != TargetType.None && target != null) || targetType == TargetType.None)
        {
            for (int i = 0; i < _effects.effects.Count; i++)
                effects.effects[i].effect(sender, target, effects.effectValues[i]);
        }   
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
