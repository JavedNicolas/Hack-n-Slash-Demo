using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemObject : InteractableObject
{
    // attributs
    public TextMeshProUGUI itemNameLabel;

    [Header("The loot")]
    [SerializeField] Loot _loot;
    public Loot loot { get => _loot; }

    /// <summary>
    /// Set the loot in the game object
    /// </summary>
    /// <param name="loot">the loot</param>
    /// <param name="displayItem">Used for test, if false then do not set the graphical element</param>
    public void setLoot(Loot loot, bool displayItem = true)
    {
        this._loot = loot;
        if(displayItem)
            setObjet();
    }

    private void setObjet()
    {
        string itemNameDisplay = "";
        if (_loot.quantity > 1)
            itemNameDisplay = _loot.quantity.ToString() + " x ";
        itemNameDisplay += _loot.item.name;
        itemNameLabel.text = itemNameDisplay;

        _loot.item.itemHasBeenPickedUP += deleteItem;
        interactable = _loot.item;
    }

    protected override void deleteItem()
    {
        base.deleteItem();
    }
}
