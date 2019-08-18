using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerBehavior : BeingBehavior
{
    public float cameraRotation = 30f;
    public InteractableObject interactionTarget;

    // TEMPORARY
    public bool autoAttack = true;

    void Update()
    {
        LeftClick();
        RightClick();
        interactWithCurrentInteractionTarget();
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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == (int)LayersIndex.Ground)
            {
                moveTo(hit.point);
                interactionTarget = null;
            }
               
        }
        if (Input.GetButtonDown("Fire1"))
        {
            moveToInteractibleTarget();
        }

       
    }

    void RightClick()
    {
        if (Input.GetButton("Fire2"))
        {
            moveToInteractibleTarget();
        }

        for (int i = 0; i < SkillBarUI.instance.getSkillSlotNumber(); i++)
        {
            SkillSlotUI skillSlot = SkillBarUI.instance.getSkillAtIndex(i);

            if (Input.GetButton(skillSlot.inputName))
            {
                if (skillSlot.skill != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit mousehit;
                    if (Physics.Raycast(ray, out mousehit))
                    {
                        EnemyBehavior enemyBehavior = null;
                        // Check if there was an enemy under the mouse
                        if (mousehit.transform.CompareTag(Tags.Enemy.ToString()))
                            enemyBehavior = mousehit.transform.GetComponent<EnemyBehavior>();

                        attack(skillSlot.skill, mousehit.point, enemyBehavior.being);
                    }

                }
            }
        }
    }

    void moveToInteractibleTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<InteractableObject>() != null)
        {
            InteractableObject interactableObject = hit.transform.GetComponent<InteractableObject>();
            interactionTarget = interactableObject;
        }
    }

    void interactWithCurrentInteractionTarget()
    {
        if (interactionTarget != null)
        {
            float interactionDistance = interactionTarget.interactable.getInteractionDistance();
            if (interactionTarget.interactable.Interactibletype == InteractableObjectType.Enemy)
                interactionDistance = being.attackRange;

            if (Vector3.Distance(transform.position, interactionTarget.transform.position) <= interactionDistance)
            {
                if (interactionTarget.interactable.interact(this) && !autoAttack)
                    interactionTarget = null;
                stopMoving();
            }else
            {
                moveTo(interactionTarget.transform.position, interactionDistance);
            }
        }
    }
   
}
