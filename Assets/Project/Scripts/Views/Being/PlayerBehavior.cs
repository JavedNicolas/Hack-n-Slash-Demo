using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == (int)LayersIndex.Ground)
                moveTo(hit.point);
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
                if(Vector3.Distance(transform.position, mousehit.point) > being.attackRange)
                {
                    moveTo(mousehit.point, being.attackRange);
                }
                else
                {
                    if (mousehit.transform.CompareTag("Enemy"))
                    {
                        attack(being.skills[0], new Vector3(0, 0, 0), mousehit.transform.GetComponent<EnemyBehavior>());
                    }
                }
            }
        }
        if (Input.GetKey(KeyCode.A))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mousehit;
            if (Physics.Raycast(ray, out mousehit))
            {
                if(being.skills[1].skillType == SkillType.Projectile)
                {
                    
                    attack(being.skills[1], mousehit.point);
                }
                
            }
        }
    }

   
}
