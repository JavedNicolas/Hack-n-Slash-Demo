using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemObject : InteractableObject
{
    [SerializeField] Item item;

    protected TextMeshProUGUI itemNameLabel;

    private void Start()
    {
        itemNameLabel = GetComponentInChildren<TextMeshProUGUI>();

        item = new Item(GameManager.instance.itemDatabase.getRandomElement());

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
