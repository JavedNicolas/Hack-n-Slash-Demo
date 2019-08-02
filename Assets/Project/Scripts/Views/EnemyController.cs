using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MovementController
{
    [Header("Player Dectection")]
    public float dectetionRange = 10f;
    public float attackRange = 2f;

    List<PlayerController> players = new List<PlayerController>();
    public Enemy enemy;

    int closestPlayerIndex = 0;
    Vector3 closestPlayerPosition;
    float closestPlayerDistance;

    void Start()
    {
        enemy.skills.Add(new BasicAttack(10));
        
        players = FindObjectsOfType<PlayerController>().ToList();
        InvokeRepeating("checkForPlayerInRange", 0.0f, 0.1f);
    }

    private void Update()
    {
        setMoveSpeed(enemy.movementSpeedPercentage);
        checkIfPLayerIsInAttackRange();
    }

    /// <summary>
    /// Check if the closesest Player is in the detection Range
    /// </summary>
    void checkForPlayerInRange()
    {
        if (players.Count > 0)
        {
            closestPlayerIndex = 0;
            closestPlayerPosition = players[0].transform.position;
            closestPlayerDistance = Vector3.Distance(transform.position, closestPlayerPosition);

            for (int i = 1; i < players.Count; i++)
            {
                float distance2 = Vector3.Distance(transform.position, players[i].transform.position);

                if (closestPlayerDistance < distance2)
                {
                    closestPlayerPosition = players[i].transform.position;
                    closestPlayerDistance = distance2;
                    closestPlayerIndex = i;
                }   
            }

            if (closestPlayerDistance < dectetionRange && closestPlayerDistance > attackRange)
            {
                moveTo(closestPlayerPosition, attackRange);
            }
        }
    }

    void checkIfPLayerIsInAttackRange()
    {
        if (closestPlayerDistance < attackRange)
        {
            transform.LookAt(closestPlayerPosition);
            attack(players[closestPlayerIndex]);
        }
    }

    void attack(PlayerController player)
    {
        enemy.skills[0].effect(player.player, enemy.aspd);
        print(player.player.currentLife);
    }
}
