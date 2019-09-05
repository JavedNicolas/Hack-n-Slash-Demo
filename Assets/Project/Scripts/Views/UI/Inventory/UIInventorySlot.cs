using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIInventorySlot : BaseUI, IPopUpOnHovering
{
    [Header("Targeting and moving object")]
    [SerializeField] Vector3 _movingOffset = new Vector3(0, -15, 0);
    [SerializeField] Vector3 _itemScaleWhileHandling = new Vector3(0.3f, 0.3f, 0.3f);

    // The player
    PlayerBehavior _playerBehavior;

    // UIInventory
    UIInventory _parent;

    // The ui slot
    private InventorySlot _inventorySlot;
    public InventorySlot inventorySlot { get => _inventorySlot; }

    // Components
    Image _itemImage;
    TextMeshProUGUI _itemQuantityText;

    // Object handling
    // targeted Object
    public static bool targeting = false;
    static UIInventorySlot _targetingSourceSlot;
    static GameObject _targetingCursorItem;

    // Moving object
    public static bool moving = false;
    static UIInventorySlot _movingSourceSlot;
    Vector3 _startLocalPosition;

    private void Awake()
    {
        _itemImage = GetComponent<Image>();
        _itemQuantityText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _playerBehavior = GameManager.instance.GetPlayerBehavior();
        _startLocalPosition = GetComponent<RectTransform>().localPosition;
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
    public void initSlot(InventorySlot updatedslot, UIInventory parent)
    {
        if (updatedslot.item != null)
        {
            Color color = _itemImage.color;
            color.a = 1;
            _itemImage.color = color;
            _itemImage.sprite = updatedslot.item.itemIcon;
            _parent = parent;
            if (updatedslot.item.isStackable)
                _itemQuantityText.text = updatedslot.quantity.ToString();
        }
        else
        {
            _inventorySlot.emptySlot();
            _itemQuantityText.text = "";
            _itemImage.sprite = null;
            Color color = _itemImage.color;
            color.a = 0;
            _itemImage.color = color;
        }

        _inventorySlot = updatedslot;
    }


    #region moving
    void displayObjectMoving()
    {
        if(moving && _movingSourceSlot == this)
        {
            _itemImage.transform.localScale = _itemScaleWhileHandling;
            _itemImage.transform.position = Input.mousePosition + _movingOffset;
        }
    }

    void tryMovingTarget()
    {
        if (!hasItem() && this != _movingSourceSlot)
        {
            // set this inventory slot
            _inventorySlot.copySlot(_movingSourceSlot.inventorySlot);
            _movingSourceSlot._inventorySlot.emptySlot();

            // clean the old inventory slot
            updateInventorySlots(new List<InventorySlot>() { _inventorySlot, _movingSourceSlot.inventorySlot });
            stopMovingObject();
        }
        else if (hasItem() && this != _movingSourceSlot)
        {
            InventorySlot bufferSlot = this.inventorySlot;
            _inventorySlot.copySlot(_movingSourceSlot.inventorySlot);
            _movingSourceSlot._inventorySlot.copySlot(bufferSlot);

            updateInventorySlots(new List<InventorySlot>() { _inventorySlot, _movingSourceSlot.inventorySlot });
            stopMovingObject();
        }
        else if (this == _movingSourceSlot)
        {
            stopMovingObject();
        }
    }

    void stopMovingObject()
    {
        moving = false;
        _movingSourceSlot.GetComponent<RectTransform>().localPosition = _startLocalPosition; 
        _movingSourceSlot._itemImage.transform.localScale = Vector3.one;
        _movingSourceSlot = null;
    }
    #endregion

    #region targeting
    void displayTargetingObject()
    {
        if (targeting && _targetingSourceSlot == this)
        {
            if (_targetingCursorItem == null)
            {
                _targetingCursorItem = new GameObject();
                _targetingCursorItem.transform.parent = transform.parent;
                _targetingCursorItem.AddComponent<Image>();
                _targetingCursorItem.GetComponent<Image>().sprite = _itemImage.sprite;
                _targetingCursorItem.transform.localScale = _itemScaleWhileHandling;
            }
                
            _targetingCursorItem.transform.position = Input.mousePosition + _movingOffset;
            if (Input.GetButtonDown(InputConstant.leftMouseButtonName))
                stopTargeting();
        }
    }

    void tryUsingTargetItem()
    {
        if (hasItem() && _inventorySlot.item.databaseID != _targetingSourceSlot.inventorySlot.item.databaseID)
        {
            if(_targetingSourceSlot.inventorySlot.item.use(_playerBehavior, transform.gameObject))
            {
                _targetingSourceSlot.removeItem();
                updateInventorySlots(_targetingSourceSlot._inventorySlot);
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
        Destroy(_targetingCursorItem);
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
                _movingSourceSlot = this;
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
                if (_playerBehavior.tryTodropItem(_inventorySlot))
                {
                    _inventorySlot.emptySlot();
                    updateInventorySlots(_inventorySlot);
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
            if (targeting && _targetingSourceSlot == this)
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
                _targetingSourceSlot = this;
            }
            else if (_inventorySlot.item.isConsomable && _inventorySlot.item.targetType == TargetType.None)
            {
                useItem();
            }
                
        }
        if (targeting && _targetingSourceSlot != this)
        {
            tryUsingTargetItem();
        }
    }

    public void rightCLick(bool overInterface)
    {
        if (!overInterface)
        {
            if (targeting && _targetingSourceSlot == this)
                stopTargeting();
        }
    }

    #endregion

    /// <summary>
    /// Use the item when clicked
    /// </summary>
    void useItem(GameObject target = null)
    {
        if (_inventorySlot.item.use(_playerBehavior))
            if (_inventorySlot.item.isConsomable)
                if (_inventorySlot.item.targetType == TargetType.None)
                {
                    _inventorySlot.removeItem();
                    updateInventorySlots(_inventorySlot);
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

    public void updateInventorySlots(List<InventorySlot> slots)
    {
        _parent.updateInventorySlots(slots);
    }

    public void updateInventorySlots(InventorySlot slot)
    {
        _parent.updateInventorySlots(new List<InventorySlot>() { slot });
    }

    //getter
    public override Vector3 getDescriptionPopUpOffset()
    {
        return transform.parent.GetComponentInParent<GridLayoutGroup>().cellSize;
    }

    public bool hasItem() { return _inventorySlot.item != null ? true : false; }
    public InventorySlot getInventorySlot() { return _inventorySlot; }
}
