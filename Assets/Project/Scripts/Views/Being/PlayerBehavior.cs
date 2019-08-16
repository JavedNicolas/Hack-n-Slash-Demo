using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.EventSystems;

public class PlayerBehavior : BeingBehavior
{
    public float cameraRotation = 30f;

    void Update()
    {
        move();
        attack();
    }

    /// <summary>
    /// Start the movements function if the player use the left mouse click
    /// </summary>
    void move()
    {
        if (Input.GetButton("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == (int)LayersIndex.Ground)
                moveTo(hit.point);

            if(Physics.Raycast(ray, out hit) && hit.transform.CompareTag(Tags.PickableItem.ToString())){
                Item itemToPickUP = hit.transform.GetComponent<ItemObject>().item;
                if (!itemToPickUP.pickUP((Player)being))
                {
                    print("Inventory Full");
                }
                Player test = (Player)being;
            }
        }
    }

    void attack()
    {
        if (Input.GetButton("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mousehit;
            if (Physics.Raycast(ray, out mousehit))
            {
                if (Vector3.Distance(transform.position, mousehit.point) > being.attackRange)
                {
                    moveTo(mousehit.point, being.attackRange);
                }
                else
                {
                    if (mousehit.transform.CompareTag(Tags.Enemy.ToString()))
                    {
                        attack(being.skills[0], mousehit.point, mousehit.transform.GetComponent<EnemyBehavior>());
                    }
                }
            }
        }

        for (int i = 0; i < SkillBarUI.instance.getSkillSlotNumber(); i++)
        {
            SkillSlotUI skillSlot = SkillBarUI.instance.getSkillAtIndex(i);

            if (Input.GetButton(skillSlot.inputName))
            {
                if(skillSlot.skill != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit mousehit;
                    if (Physics.Raycast(ray, out mousehit))
                    {
                        EnemyBehavior enemyBehavior = null;
                        // Check if there was an enemy under the mouse
                        if (mousehit.transform.CompareTag(Tags.Enemy.ToString()))
                            enemyBehavior = mousehit.transform.GetComponent<EnemyBehavior>();

                        attack(skillSlot.skill, mousehit.point, enemyBehavior);
                    }
                        
                }
            }
        }
    }

   
}
