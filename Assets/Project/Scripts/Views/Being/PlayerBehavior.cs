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

    // TEMPORARY
    public bool autoAttack = true;


    private void Start()
    {
        interactOnce = !autoAttack;
    }

    void Update()
    {
        LeftClick();
        RightClick();
    }

    /// <summary>
    /// Start the movements function if the player use the left mouse click
    /// </summary>
    void LeftClick()
    {
        if (Input.GetButton("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (moveToInteractibleTarget())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, movementMask))
            {
                moveTo(hit.point);
                interactionTarget = null;
            }
        }
    }

    void RightClick()
    {
        if (Input.GetButton("Fire2"))
        {
            moveToInteractibleTarget(true);
        }

        for (int i = 0; i < SkillBarUI.instance.getSkillSlotNumber(); i++)
        {
            SkillSlotUI skillSlot = SkillBarUI.instance.getSkillAtIndex(i);

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
   
}
