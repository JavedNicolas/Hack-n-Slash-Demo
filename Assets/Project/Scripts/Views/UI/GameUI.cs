using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    // instance
    public static GameUI instance;

    [Header("Base Screen UI")]
    [SerializeField] GameObject lifeUI;

    [Header("Openable UI")]
    [SerializeField] InventoryUI inventoryUI;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.instance.getPlayer().inventory.inventoryChanged += inventoryUI.updateInventoryUI;
        inventoryUI.itemHasBeenUsed = GameManager.instance.getPlayer().inventory.updateSlot;
        inventoryUI.loadInventory();
    }

    private void Update()
    {
        showInventory();
    }

    public void displayUI(Being player)
    {
        lifeUI.GetComponent<LifeUI>().setBeing(player);
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
