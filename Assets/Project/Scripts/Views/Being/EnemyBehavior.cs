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
    public UILife lifeUI;

    [Header("Loot")]
    public static float lootExplosionStrength = 10f;
    public List<Loot> loot;

    private List<BeingBehavior> enemies = new List<BeingBehavior>();

    // Player detection
    private BeingBehavior closestEnemy;

    protected Ability abilityToUse = null;

    // override the being
    public new Enemy being
    {
        get { return (Enemy)_being; }
        set
        {
            _being = value;
            interactable = value;
        }
    }

    void Start()
    {
        loot = being.generateLoot();
        lifeUI.setBeing(being);

        InvokeRepeating("checkForEnemyInRange", 0.0f, 0.1f);
    }

    private void Update()
    {
        IABheavior();
        if (being != null && being.currentLife <= 0)
        {
            die();
        }
    }

    protected virtual void IABheavior()
    {
        refreshEnemyList();
        checkIfEnemyIsInAttackRange();
    }

    /// <summary>
    /// Resh the list containing the enemy in the map
    /// </summary>
    void refreshEnemyList()
    {
        enemies = FindObjectsOfType<BeingBehavior>().ToList();
    }

    void pickAbilityToUseNext()
    {
        int abilityFrenquency = Random.Range(0, 100);
        List<int> possibleAbilityIndex = new List<int>();

        for (int i = 0; i < being.abilities.Count; i++)
        {
            if ((int)being.abilityUsageFrequency[i] >= abilityFrenquency)
            {
                possibleAbilityIndex.Add(i);
            }   
        }

        if (possibleAbilityIndex.Count > 0)
        {
            int abilityToUseIndex = Random.Range(0, possibleAbilityIndex.Count);
            abilityToUse = being.abilities[abilityToUseIndex];
        }
        else
            abilityToUse = being.basicAttack;
    }

    /// <summary>
    /// Check if the closest Player is in the detection Range
    /// </summary>
    void checkForEnemyInRange()
    {
        if (enemies.Count > 0)
        {
            if(closestEnemy == null || closestEnemy.being.isDead())
            {
                stopMoving();
                int closestEnemyIndex = enemies.FindIndex(x => x.teamID != teamID && canBeAttacked);
                closestEnemy = (closestEnemyIndex < enemies.Count ? enemies[closestEnemyIndex] : null);
                return;
            }


            float currentClosestEnemyDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

            for (int i = 1; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                    continue;

                float distance = Vector3.Distance(transform.position, enemies[i].transform.position);

                if (currentClosestEnemyDistance > distance && enemies[i].teamID != teamID)
                {
                    closestEnemy = enemies[i];
                    currentClosestEnemyDistance = distance;
                }
            }

            if (currentClosestEnemyDistance < dectetionRange && currentClosestEnemyDistance > attackRange && closestEnemy != null)
            {
                moveTo(closestEnemy.transform.position, attackRange);
            }
        }
    }

    /// <summary>
    /// Check if the player is in attack range if so attack
    /// </summary>
    void checkIfEnemyIsInAttackRange()
    {
        if (closestEnemy == null)
            return;

        if(abilityToUse == null)
            pickAbilityToUseNext();

        float closestEnemyDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);
        if (closestEnemyDistance < attackRange && abilityToUse != null)
            if(useAbility(abilityToUse, closestEnemy.transform.position, closestEnemy))
            {
                abilityToUse = null;
            }

    }

    void dropLoot()
    {
        for(int i = 0; i < loot.Count; i++)
        {
            GameObject itemObject = Instantiate(GameManager.instance.itemObjetPrefab);
            itemObject.transform.position = transform.position;
            itemObject.GetComponent<ItemObject>().setLoot(loot[i]);
            Vector3 force = new Vector3(Random.Range(-lootExplosionStrength, lootExplosionStrength), 1, Random.Range(-lootExplosionStrength, lootExplosionStrength));
            itemObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }

    protected override void die()
    {
        dropLoot();
        base.die();
    }
}
