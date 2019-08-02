using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PlayerController : MovementController
{
    public float cameraRotation = 30f;

    [Header("Misc")]
    public Camera camera;
    public Player player;

    void Update()
    {
        setMoveSpeed(player.movementSpeedPercentage);
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
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.layer == (int)LayersIndex.Ground)
                moveTo(hit.point);
        }

        //transform.parent.transform.position = transform.position;
    }

    void attack()
    {
        if (Input.GetButton("Fire2"))
        {

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit mousehit;
            Debug.DrawRay(transform.position, Vector3.forward * player.attackRange, Color.green);
            if (Physics.Raycast(ray, out mousehit))
            {
                if(Vector3.Distance(transform.position, mousehit.point) > player.attackRange)
                {
                    moveTo(mousehit.point, player.attackRange);
                }
                else
                {
                    lookAtStraight(mousehit.point);
                    Ray ray2 = new Ray(transform.position,this.transform.forward);
                    RaycastHit hit;
                    
                    if (Physics.Raycast(ray2, out hit, player.attackRange))
                    {
                        print(hit.transform.name);
                        if (hit.transform.CompareTag("Enemy"))
                        {
                            player.skills[0].effect(hit.transform.gameObject.GetComponent<EnemyController>().enemy, player.aspd);
                            print(hit.transform.gameObject.GetComponent<EnemyController>().enemy.currentLife);
                        }
                    }
                }
            }


            
        }
    }

   
}
