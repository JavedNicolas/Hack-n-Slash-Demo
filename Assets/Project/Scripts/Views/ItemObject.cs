using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemObject : InteractableObject
{
    [Header("The loot")]
    [SerializeField] Loot _loot;
    public Loot loot { get => _loot; }

    // TMP
    bool randomItem = true;

    // attributs
    protected TextMeshProUGUI itemNameLabel;

    private void Start()
    {
        setObjet();
    }

    private void setObjet()
    {
        itemNameLabel = GetComponentInChildren<TextMeshProUGUI>();

        if (randomItem)
        {
            _loot.item = new Item(GameManager.instance.itemDatabase.getRandomElement());
            _loot.quantity = 1;
        }

        if (_loot.item != null)
        {
            interactable = _loot.item;
            _loot.item.itemHasBeenPickedUP = deleteItem;
        }

        if (itemNameLabel != null && _loot.item != null)
        {
            string itemNameDisplay = "";
            if (_loot.quantity > 1)
                itemNameDisplay = _loot.quantity.ToString() + " x ";
            itemNameDisplay += _loot.item.name;
            itemNameLabel.text = itemNameDisplay;
        }   
    }

    public void setLoot(Loot loot)
    {
        this._loot = loot;
        randomItem = false;
        setObjet();
    }

}
