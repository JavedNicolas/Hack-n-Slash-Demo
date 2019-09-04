using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIInventorySlot : BaseUI, IPopUpOnHovering
{
    [Header("Targeting and moving object")]
    [SerializeField] Vector3 movingOffset = new Vector3(0, -15, 0);
    [SerializeField] Vector3 itemScaleWhileHandling = new Vector3(0.3f, 0.3f, 0.3f);

    // The player
    PlayerBehavior playerBehavior;

    // The ui slot
    private InventorySlot _inventorySlot;
    public InventorySlot inventorySlot { get => _inventorySlot; }

    // Components
    Image itemImage;
    TextMeshProUGUI itemQuantityText;

    // Object handling
    // targeted Object
    public static bool targeting = false;
    static UIInventorySlot targetingSourceSlot;
    static GameObject targetingCursorItem;

    // Moving object
    public static bool moving = false;
    static UIInventorySlot movingSourceSlot;
    Vector3 startLocalPosition;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        itemQuantityText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        playerBehavior = GameManager.instance.GetPlayerBehavior();
        startLocalPosition = GetComponent<RectTransform>().localPosition;
    }

    // Deselect slot on hovering
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
        displayPopUp(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        displayPopUp(false);
    }

    public void Update()
    {
        displayTargetingObject();
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
        movingSourceSlot.GetComponent<RectTransform>().localPosition = startLocalPosition; 
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
            if(targetingSourceSlot.inventorySlot.item.use(playerBehavior, transform.gameObject))
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

    #region dragging
    protected override void dragging(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            GameManager.instance.lockClick(true, true);
            if (!moving && hasItem() && !targeting)
            {
                moving = true;
                movingSourceSlot = this;
            }
            displayObjectMoving();
        }
    }

    protected override void dragginEnd(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (playerBehavior.tryTodropItem(_inventorySlot))
                {
                    _inventorySlot.emptySlot();
                    updateInventory(_inventorySlot);
                }
                stopMovingObject();
                GameManager.instance.lockClick(false, true);
            }
            else
            {
                UIInventorySlot slotHovered = eventData.pointerEnter.GetComponent<UIInventorySlot>();
                if (slotHovered != null)
                    slotHovered.tryMovingTarget();
                else
                    stopMovingObject();
            }
        }
    }

    #endregion

    #region clicks
    protected override void leftClickOnUI() {}

    public void leftClick(bool overInterface)
    {
        if (!overInterface)
        {
            if (targeting && targetingSourceSlot == this)
            {
                stopTargeting();
            }
        }
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
            if (targeting && targetingSourceSlot == this)
                stopTargeting();
        }
    }

    #endregion

    /// <summary>
    /// Use the item when clicked
    /// </summary>
    void useItem(GameObject target = null)
    {
        if (_inventorySlot.item.use(playerBehavior))
            if (_inventorySlot.item.isConsomable)
                if (_inventorySlot.item.targetType == TargetType.None)
                {
                    _inventorySlot.removeItem();
                    updateInventory(_inventorySlot);
                }
    }

    /// <summary>
    /// remove an item from the slot
    /// </summary>
    public void removeItem()
    {
        _inventorySlot.removeItem();
    }

    public void displayPopUp(bool display)
    {
        if(_inventorySlot.item != null)
            GameUI.instance.displayDescription(display, _inventorySlot.item, this);
    }

    public void updateInventory(List<InventorySlot> slots)
    {
        GameUI.instance.updateInventorySlots(slots);
    }

    public void updateInventory(InventorySlot slot)
    {
        GameUI.instance.updateInventorySlots(new List<InventorySlot>() { slot });
    }

    //getter
    public override Vector3 getDescriptionPopUpOffset()
    {
        return transform.parent.GetComponentInParent<GridLayoutGroup>().cellSize;
    }

    public bool hasItem() { return _inventorySlot.item != null ? true : false; }
    public InventorySlot getInventorySlot() { return _inventorySlot; }
}
