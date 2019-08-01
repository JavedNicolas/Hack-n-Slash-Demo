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
    }

   
}
