using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerBehavior : BeingBehavior
{
    [Header("Camera Rotation")]
    [SerializeField] float cameraRotation = 30f;

    [Header("Movement Mask")]
    [SerializeField] LayerMask movementMask;

    [Header("Clicks")]
    float dropItemMaxDistance = 20f;

    [Header("Animation")]
    [SerializeField] ParticleSystem[] levelUPParticules;

    // UIs
    UITopBar _topBarUI;
    UIInventory _inventoryUI;
    UISkillBar _skillBarUI;
    UILife _lifeUI;
    UIMana _manaUI;

    // being
    public new Player being
    {
        get { return (Player)_being; }
        set
        {
            _being = value;
            interactable = value;
        }

    }

    // TEMPORARY
    public bool autoAttack = true;

    private void Start()
    {
        being.hasLevelUp = launchlevelUpAnimation;
        _interactOnce = !autoAttack;

        getUIElements();
        initPlayerUI();

        GameManager.instance.leftClickDelegate += LeftClick;
        GameManager.instance.rightClickDelegate += RightClick;
    }

    private void Update()
    {
        skillBarKeyDown();
        displayInventory();
    }

    public void getUIElements()
    {
        _inventoryUI = GameUI.instance.inventoryUI;
        _skillBarUI = GameUI.instance.skillBar;
        _lifeUI = GameUI.instance.lifeUI;
        _manaUI = GameUI.instance.manaUI;
        _topBarUI = GameUI.instance.topBar;
    }

    public void initPlayerUI()
    {
        _lifeUI.setBeing(being);
        _manaUI.setBeing(being);
        _inventoryUI.loadInventory(being.inventory);
        _topBarUI.init(being);
    }

    /// <summary>
    /// Start the movements function if the player use the left mouse click
    /// </summary>
    void LeftClick(bool overInterface)
    {
        if (overInterface)
            return;

        if (!GameManager.instance.canLeftClick)
        {
            return;
        }
            

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, movementMask))
        {
            moveTo(hit.point);
            _interactionTarget = null;
        }
    }

    void RightClick(bool overInterface)
    {
        if (overInterface)
            return;

        if (!GameManager.instance.canRightClick)
            return;


        moveToInteractableTarget();
    }

    /// <summary>
    /// Check for key to use the skill in the skill bar
    /// </summary>
    void skillBarKeyDown()
    {
        for (int i = 0; i < _skillBarUI.getSkillSlotNumber(); i++)
        {
            UISkillSlot skillSlot = _skillBarUI.getSkillAtIndex(i);

            if (Input.GetButton(skillSlot.inputName))
                if (skillSlot.ability != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit mouseHit;
                    if (Physics.Raycast(ray, out mouseHit))
                        if (_abilityManager.tryToPerformAbility(mouseHit, skillSlot.ability))
                        {
                            BeingBehavior target = null;
                            if (skillSlot.ability.abilityAttributs.needTarget)
                                target = mouseHit.transform.GetComponent<BeingBehavior>();

                            abilityUsed(mouseHit.point, target, skillSlot.ability);
                        }

                }
        }
    }

    /// <summary>
    /// Tyro to drop an item, it may fail depending on the distance to the player
    /// </summary>
    /// <param name="slotWithTheItem">Slot containing the item</param>
    /// <returns>return true if the item can be dropped</returns>
    public bool tryTodropItem(InventorySlot slotWithTheItem)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag(Tags.Ground.ToString()))
        {
            if(Vector3.Distance(hit.point, transform.position) <= dropItemMaxDistance)
            {
                GameObject itemObject = Instantiate(GameManager.instance.itemObjetPrefab);
                Loot loot = new Loot(slotWithTheItem.item, 0, slotWithTheItem.quantity);
                itemObject.GetComponent<ItemObject>().setLoot(loot);
                itemObject.transform.position = hit.point;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Display inventory or hide it
    /// </summary>
    void displayInventory()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            _inventoryUI.showInventory(false);
        }
        if (Input.GetButtonDown("SmallInventory"))
        {
            _inventoryUI.showInventory(true);
        }
    }

    void launchlevelUpAnimation()
    {
        for(int i= 0; i < levelUPParticules.Length; i++)
        {
            levelUPParticules[i].Play();
        }
    }

    public void addExperience(float value)
    {
        being.addExperience(value);
    }


}
