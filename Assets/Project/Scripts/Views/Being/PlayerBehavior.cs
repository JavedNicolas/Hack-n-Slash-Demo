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

    [Header("Raycast Masks")]
    [SerializeField] LayerMask movementMask;
    [SerializeField] LayerMask abilitySpawnMask;

    [Header("Clicks")]
    float dropItemMaxDistance = 20f;

    [Header("Animation")]
    [SerializeField] ParticleSystem[] levelUPParticules;

    // camera
    [SerializeField] Camera _playerCamera;
    public Camera playerCamera { get => _playerCamera; }

    public new PlayerAbilityManager abilityManager { get => (PlayerAbilityManager)_abilityManager;}

    // UIs
    UITopBar _topBarUI;
    UIInventory _inventoryUI;
    UISkillBar _skillBarUI;
    UILife _lifeUI;
    UIMana _manaUI;

    //ability Buffer
    Ability abilityBuffer;

    // override the being
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
        being.hasLeveledUP = launchlevelUpAnimation;
        _interactOnce = !autoAttack;
        _abilityManager = GetComponent<PlayerAbilityManager>();
        getUIElements();
        initPlayerUI();

        if(_playerCamera == null)
        {
            _playerCamera = Camera.main;
        }

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
            
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, movementMask))
        {
            if (abilityBuffer != null)
                abilityBuffer = null;
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

        if (abilityBuffer != null)
            abilityBuffer = null;

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
                    Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit mouseHit;
                    // cast a ray on the mouse position from the camera
                    if (Physics.Raycast(ray, out mouseHit))
                    {
                        Vector3 targetedPosition = mouseHit.point;

                        // get the position of the ground at the targeted position
                        RaycastHit abilitySpawnHit;
                        if(Physics.Raycast(ray, out abilitySpawnHit, Mathf.Infinity, abilitySpawnMask))
                            targetedPosition.y = abilitySpawnHit.point.y;

                        BeingBehavior targetedBehavior = mouseHit.transform.GetComponent<BeingBehavior>();
                        IEnumerator coroutine = useAbility(targetedBehavior, targetedPosition, skillSlot.ability);
                        StartCoroutine(coroutine);
                    }
                }
        }
    }

    IEnumerator useAbility(BeingBehavior target, Vector3 targetedPosition, Ability ability)
    {
        if(!abilityManager.isInRange(ability, targetedPosition))
        {
            abilityBuffer = ability;
            moveTo(targetedPosition);
            yield return new WaitUntil(() => abilityManager.isInRange(ability, targetedPosition) || abilityBuffer == null);
        }

        if (abilityBuffer == null)
            yield return null;

        stopMoving();

        if (abilityManager.tryToPerformAbility(target, targetedPosition, ability))
        {
            abilityUsed(targetedPosition, target, ability);
            abilityBuffer = null;
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
        being.addAllExperience(value);
    }

    protected override void die()
    {
        
    }

}
