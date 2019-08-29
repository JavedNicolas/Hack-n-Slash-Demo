﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : BaseUI, IPointerEnterHandler
{
    [Header("Targeting and moving object")]
    [SerializeField] Vector3 movingOffset = new Vector3(0, -15, 0);
    [SerializeField] Vector3 itemScaleWhileHandling = new Vector3(0.3f, 0.3f, 0.3f);

    // The ui slot
    private InventorySlot _inventorySlot;
    public InventorySlot inventorySlot { get => _inventorySlot; }

    // Components
    Image itemImage;
    TextMeshProUGUI itemQuantityText;

    // Object handling
    // targeted Object
    static bool targeting = false;
    static InventorySlotUI targetingSourceSlot;
    static GameObject targetingCursorItem;

    // Moving object
    static bool moving = false;
    static InventorySlotUI movingSourceSlot;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        itemQuantityText = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Deselect slot on hovering
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Update()
    {
        displayTargetingObject();
        displayObjectMoving();
    }

    /// <summary>
    /// Update the slot with his content
    /// </summary>
    public void initSlot(InventorySlot updatedslot)
    {
        if (updatedslot.item != null)
        {
            Color color = itemImage.color;
            color.a = 1;
            itemImage.color = color;
            itemImage.sprite = updatedslot.item.itemIcon;
            if (updatedslot.item.isStackable)
                itemQuantityText.text = updatedslot.quantity.ToString();
        }
        else
        {
            _inventorySlot.emptySlot();
            itemQuantityText.text = "";
            itemImage.sprite = null;
            Color color = itemImage.color;
            color.a = 0;
            itemImage.color = color;
        }

        _inventorySlot = updatedslot;
    }

    #region moving
    void displayObjectMoving()
    {
        if(moving && movingSourceSlot == this)
        {
            itemImage.transform.localScale = itemScaleWhileHandling;
            itemImage.transform.position = Input.mousePosition + movingOffset;

            
        }
    }

    void tryMovingTarget()
    {
        if (!hasItem() && this != movingSourceSlot)
        {
            // set this inventory slot
            _inventorySlot.copySlot(movingSourceSlot.inventorySlot);
            movingSourceSlot._inventorySlot.emptySlot();

            // clean the old inventory slot
            updateInventory(new List<InventorySlot>() { _inventorySlot, movingSourceSlot.inventorySlot });
            stopMovingObject();
        }
        else if (hasItem() && this != movingSourceSlot)
        {
            InventorySlot bufferSlot = this.inventorySlot;
            _inventorySlot.copySlot(movingSourceSlot.inventorySlot);
            movingSourceSlot._inventorySlot.copySlot(bufferSlot);

            updateInventory(new List<InventorySlot>() { _inventorySlot, movingSourceSlot.inventorySlot });
            stopMovingObject();
        }
        else if (this == movingSourceSlot)
        {
            stopMovingObject();
        }
    }

    void stopMovingObject()
    {
        moving = false;
        movingSourceSlot.GetComponent<RectTransform>().localPosition = Vector3.zero; 
        movingSourceSlot.itemImage.transform.localScale = Vector3.one;
        movingSourceSlot = null;
    }
    #endregion

    #region targeting
    void displayTargetingObject()
    {
        if (targeting && targetingSourceSlot == this)
        {
            if (targetingCursorItem == null)
            {
                targetingCursorItem = new GameObject();
                targetingCursorItem.transform.parent = transform.parent;
                targetingCursorItem.AddComponent<Image>();
                targetingCursorItem.GetComponent<Image>().sprite = itemImage.sprite;
                targetingCursorItem.transform.localScale = itemScaleWhileHandling;
            }
                
            targetingCursorItem.transform.position = Input.mousePosition + movingOffset;
            if (Input.GetButtonDown(InputConstant.leftMouseButtonName))
                stopTargeting();
        }
    }

    void tryUsingTargetItem()
    {
        if (hasItem() && _inventorySlot.item.databaseID != targetingSourceSlot.inventorySlot.item.databaseID)
        {
            if(targetingSourceSlot.inventorySlot.item.use(GameManager.instance.GetPlayerBehavior(), transform.gameObject))
            {
                targetingSourceSlot.removeItem();
                updateInventory(targetingSourceSlot._inventorySlot);
            }
            stopTargeting();
        }else
        {
            stopTargeting();
        }
    }

    void stopTargeting()
    {
        targeting = false;
        Destroy(targetingCursorItem);
    }
    #endregion

    /// <summary>
    /// Use the item when clicked
    /// </summary>
    void useItem(GameObject target = null)
    {
        if(_inventorySlot.item.use(GameManager.instance.GetPlayerBehavior()))
            if (_inventorySlot.item.isConsomable)
                if(_inventorySlot.item.targetType == TargetType.None)
                {
                    _inventorySlot.removeItem();
                    updateInventory(_inventorySlot);
                }
    }

    public void removeItem()
    {
        _inventorySlot.removeItem();
    }


    #region clicks
    protected override void leftClickOnUI()
    {
        if (!moving && hasItem() && !targeting)
        {
            moving = true;
            movingSourceSlot = this;
        }
        else if(moving)
        {
            tryMovingTarget();
        }
    }

    public void leftClick(bool overInterface)
    {
        if (!overInterface)
        {
            if (moving && movingSourceSlot == this)
            {
                if (dropItem()){
                    _inventorySlot.emptySlot();
                    updateInventory(_inventorySlot);
                }
                stopMovingObject();
            }else if (targeting && targetingSourceSlot == this)
            {
                stopTargeting();
            }
        }
    }

    bool dropItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit) && hit.transform.CompareTag(Tags.Ground.ToString()))
        {
            GameObject itemObject = Instantiate(GameManager.instance.itemObjet);
            Loot loot = new Loot(_inventorySlot.item, 0, _inventorySlot.quantity);
            itemObject.GetComponent<ItemObject>().setLoot(loot);
            itemObject.transform.position = hit.point;
            return true;
        }
        return false;
    }

    protected override void rightClickOnUI()
    {
        if (!targeting && _inventorySlot.item != null && !moving)
        {
            if(_inventorySlot.item.isConsomable && _inventorySlot.item.targetType != TargetType.None)
            {
                targeting = true;
                targetingSourceSlot = this;
            }
            else if (_inventorySlot.item.isConsomable && _inventorySlot.item.targetType == TargetType.None)
            {
                useItem();
            }
                
        }
        if (targeting && targetingSourceSlot != this)
        {
            tryUsingTargetItem();
        }
    }

    public void rightCLick(bool overInterface)
    {
        if (!overInterface)
        {
            if (moving && movingSourceSlot == this)
                stopMovingObject();
            else if (targeting && targetingSourceSlot == this)
                stopTargeting();
        }
        else
        {
            if (moving && movingSourceSlot == this)
                stopMovingObject();
        }
    }

    #endregion

    public void updateInventory(List<InventorySlot> slots)
    {
        GameUI.instance.updateInventorySlots(slots);
    }

    public void updateInventory(InventorySlot slot)
    {
        GameUI.instance.updateInventorySlots(new List<InventorySlot>() { slot });
    }

    //getter
    public bool hasItem() { return _inventorySlot.item != null ? true : false; }
    public InventorySlot getInventorySlot() { return _inventorySlot; }
}
