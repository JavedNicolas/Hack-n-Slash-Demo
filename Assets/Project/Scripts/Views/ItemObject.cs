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
    public void setLoot(Loot loot)
    {
        this._loot = loot;
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
