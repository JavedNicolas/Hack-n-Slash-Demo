using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemObject : InteractableObject
{
    public Item item = new LifePotion();

    protected TextMeshProUGUI itemNameLabel;

    private void Start()
    {
        itemNameLabel = GetComponentInChildren<TextMeshProUGUI>();

        if (item != null)
        {
            interactable = item;
            item.itemHasBeenPickedUP += deleteItem;
        }

        if (itemNameLabel != null && item != null)
        {
            itemNameLabel.text = item.name;
        }   
    }
}
