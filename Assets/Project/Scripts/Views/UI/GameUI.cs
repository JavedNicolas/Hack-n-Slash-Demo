using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    // instance
    public static GameUI instance;

    [Header("Base Screen UI")]
    [SerializeField] GameObject lifeUI;

    [Header("Openable UI")]
    [SerializeField] UIInventory inventoryUI;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        inventoryUI.loadInventory();
        GameManager.instance.getPlayer().inventory.inventoryChanged();
    }

    private void Update()
    {
        showInventory();
    }

    public void updateInventoryUI(Inventory inventory)
    {
        inventoryUI.updateInventoryUI(inventory);
    }

    public void updateInventorySlots(List<InventorySlot> slots)
    {
        GameManager.instance.getPlayer().inventory.updateSlots(slots);
    }

    public void displayUI(Being player)
    {
        lifeUI.GetComponent<UILife>().setBeing(player);
    }

    void showInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.showInventory(false);
        }
        if (Input.GetButtonDown("SmallInventory"))
        {
            inventoryUI.showInventory(true);
        }
    }

    public void FixedUpdate()
    {

    }


}
