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

    public Item(string name, Sprite itemIcon, GameObject itemModel, bool isConsomable, bool isStackable, int maxStackableSize, EffectAndValues effects)
    {
        this.name = name;
        this._itemIcon = itemIcon;
        this._itemModel = itemModel;
        this._isConsomable = isConsomable;
        this._isStackable = isStackable;
        this._maxStackableSize = maxStackableSize;
        this._effects = effects;
        setDistanceToInteraction(interactionDistance);
    }

    public virtual void use(BeingBehavior sender, GameObject target = null)
    {
        for (int i = 0; i < _effects.effects.Count; i++)
            effects.effects[i].effect(sender, target, effects.effectValues[i]);
            
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
