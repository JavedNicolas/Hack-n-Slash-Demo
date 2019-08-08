using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemObject : MonoBehaviour
{
    public Item item = new LifePotion();

    // Private var
    TextMeshProUGUI itemNameLabel;
    MeshRenderer renderer;

    private void Awake()
    {
        itemNameLabel = GetComponentInChildren<TextMeshProUGUI>(); ;
        renderer = GetComponent<MeshRenderer>();
        item.itemHasBeenPickedUP = deleteItem;
    }

    private void Start()
    {
        print(item.itemName);
        if (itemNameLabel != null && item != null)
            itemNameLabel.text = item.itemName;
    }

    void deleteItem()
    {
        item.itemHasBeenPickedUP = null;
        Destroy(this.gameObject);
    }
}
