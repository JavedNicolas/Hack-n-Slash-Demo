using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBehavior : BeingBehavior
{
    [Header("Player Dectection")]
    public float dectetionRange = 10f;
    public float attackRange = 2f;

    [Header("EnemyUI")]
    public LifeUI lifeUI;

    List<PlayerBehavior> players = new List<PlayerBehavior>();

    int closestPlayerIndex = 0;
    Vector3 closestPlayerPosition;
    float closestPlayerDistance;

    LightningBall ability;

    void Start()
    {
        being.skills.Add(GameManager.instance.getAbilityOfType(typeof(LightningBall)));
        players = FindObjectsOfType<PlayerBehavior>().ToList();

        lifeUI.setBeing(being);

        InvokeRepeating("checkForPlayerInRange", 0.0f, 0.1f);
    }

    private void Update()
    {
        checkIfPLayerIsInAttackRange();
        if(being != null && being.currentLife <= 0)
        {
            die();
        }
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

    /// <summary>
    /// Check if the player is in attack range if so attack
    /// </summary>
    void checkIfPLayerIsInAttackRange()
    {
        if (closestPlayerDistance < attackRange)
        {
            useAbility(being.basicAttack, closestPlayerPosition, players[closestPlayerIndex]);
        }
    }
}
