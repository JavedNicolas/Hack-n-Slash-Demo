using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerBehavior : BeingBehavior
{
    [Header("Camera Rotation")]
    public float cameraRotation = 30f;

    [Header("Movement Mask")]
    public LayerMask movementMask;

    [Header("Clicks")]
    float dropItemMaxDistance = 20f;

    // TEMPORARY
    public bool autoAttack = true;

    private void Start()
    {
        interactOnce = !autoAttack;
        GameManager.instance.leftClickDelegate += LeftClick;
        GameManager.instance.rightClickDelegate += RightClick;
    }

    private void Update()
    {
        skillBarKeyDown();
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
            interactionTarget = null;
        }
    }

    void RightClick(bool overInterface)
    {
        if (overInterface)
            return;

        if (!GameManager.instance.canRightClick)
            return;


        moveToInteractibleTarget();

    }

    void skillBarKeyDown()
    {
        for (int i = 0; i < UISkillBar.instance.getSkillSlotNumber(); i++)
        {
            UISkillSlot skillSlot = UISkillBar.instance.getSkillAtIndex(i);

            if (Input.GetButton(skillSlot.inputName))
                if (skillSlot.ability != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit mouseHit;
                    if (Physics.Raycast(ray, out mouseHit))
                        if (AbilityManager.instance.tryToPerformAbility(mouseHit, skillSlot.ability, this))
                        {
                            BeingBehavior target = null;
                            if (skillSlot.ability.abilityAttributs.needTarget)
                                target = mouseHit.transform.GetComponent<BeingBehavior>();

                            abilityUsed(mouseHit.point, target, skillSlot.ability);
                        }

                }
        }
    }

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

}
