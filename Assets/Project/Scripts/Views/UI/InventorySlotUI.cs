using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    // The ui slot
    public InventorySlot inventorySlot;
    [SerializeField] GameObject itemSlot;

    // Components
    Image itemImage;
    Button itemButton;
    TextMeshProUGUI itemQuantityText;

    public delegate void ItemUsed(int inventoryIndex, InventorySlot slot);
    public ItemUsed itemUsed;

    private void Awake()
    {
        if(itemSlot != null)
        {
            itemImage = itemSlot.GetComponent<Image>();
            itemButton = itemSlot.GetComponent<Button>();
            itemQuantityText = itemSlot.GetComponentInChildren<TextMeshProUGUI>();

            if(itemButton != null)
                itemButton.onClick.AddListener(() => useItem());
        }

    }

    public void Update()
    {
        updateInventorySlot();
    }

    /// <summary>
    /// Update the slot with his content
    /// </summary>
    void updateInventorySlot()
    {
        if (inventorySlot.item != null)
        {
            itemSlot.gameObject.SetActive(true);
            itemImage.sprite = inventorySlot.item.itemIcon;
            if (inventorySlot.item.isStackable)
                itemQuantityText.text = inventorySlot.quantity.ToString();
        }
        else
            itemSlot.gameObject.SetActive(false);
    }

    /// <summary>
    /// Use the item when clicked
    /// </summary>
    void useItem()
    {
        inventorySlot.item.use(GameManager.instance.GetPlayerBehavior());
        if (inventorySlot.item.isConsomable)
        {
            inventorySlot.removeItem();
            itemUsed(inventorySlot.index, inventorySlot);
        }
            
    }

}
