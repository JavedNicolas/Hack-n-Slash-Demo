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

    private List<EnemyAndAggro> enemies = new List<EnemyAndAggro>();

    // Enemy detection
    private EnemyAndAggro currentTarget = null;
    /// Out of range target are target that the enemy will follow when not in range enemy match the target criteria
    /// like in range, or have aggro
    private EnemyAndAggro outOfRangeAggroTarget = null;

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

        InvokeRepeating("checkForTarget", 0.0f, 0.1f);
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
        checkIfTargetIsInAttackRange();
    }

    /// <summary>
    /// Resh the list containing the enemy in the map
    /// </summary>
    void refreshEnemyList()
    {
        List<BeingBehavior> enemyList = FindObjectsOfType<BeingBehavior>().ToList();
        for(int i = 0; i < enemyList.Count; i++)
        {
            if(!enemies.Exists(x => x.beingBehavior == enemyList[i]) && enemyList[i] != null && 
                !enemyList[i].being.isDead() && enemyList[i].teamID != teamID)
            {
                enemies.Add(new EnemyAndAggro(enemyList[i], 0));
            }
        }
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
    /// Check if the closest enemy or enemy with the max aggro value is in the detection Range
    /// </summary>
    void checkForTarget()
    {
        int inRangeEnemy = 0;
        for(int i =0; i < enemies.Count; i ++)
        {
            // if the outOfRangeTarget gets in range
            if (outOfRangeAggroTarget == enemies[i] && currentTarget == null && isBeingInRange(outOfRangeAggroTarget.beingBehavior, dectetionRange))
            {
                setcurrentTarget(enemies[i]);
            }

            if (enemies[i].aggro > 0 && (outOfRangeAggroTarget == null || enemies[i].aggro > outOfRangeAggroTarget.aggro) && (currentTarget == null || enemies[i].aggro > currentTarget.aggro))
            {
                setcurrentTarget(enemies[i]);
            }
            else if(isBeingInRange(enemies[i].beingBehavior, dectetionRange) &&
                (outOfRangeAggroTarget == null || enemies[i].aggro > outOfRangeAggroTarget.aggro) && (currentTarget == null || currentTarget.aggro < enemies[i].aggro))
            {
                setcurrentTarget(enemies[i]);
            }

            if (isBeingInRange(enemies[i].beingBehavior, dectetionRange))
            {
                inRangeEnemy++;
                // add aggro based on the distance from the target
                float inRangeAggroValue = BeingConstant.distanceAggroFactor / (Vector3.Distance(enemies[i].beingBehavior.transform.position, transform.position) + 0.01f);
                enemies[i].addAggro(inRangeAggroValue);
            }
        }

        if (inRangeEnemy == 0)
        {
            currentTarget = null;
            if (outOfRangeAggroTarget == null)
            {
                resetAggro();
            }  
        }

        // move to the target
        if (outOfRangeAggroTarget != null && !isBeingInRange(outOfRangeAggroTarget.beingBehavior, attackRange))
            moveTo(outOfRangeAggroTarget.beingBehavior.transform.position, attackRange);
        else if (currentTarget != null && !isBeingInRange(currentTarget.beingBehavior, attackRange))
            moveTo(currentTarget.beingBehavior.transform.position, attackRange);
    }

    void setcurrentTarget(EnemyAndAggro enemy)
    {
        currentTarget = enemy;
        outOfRangeAggroTarget = null;
    }

    /// <summary>
    /// Check if the player is in attack range if so attack
    /// </summary>
    void checkIfTargetIsInAttackRange()
    {
        if (currentTarget == null)
            return;

        if(abilityToUse == null)
            pickAbilityToUseNext();

        float targetedEnemyDistance = Vector3.Distance(transform.position, currentTarget.beingBehavior.transform.position);
        if (targetedEnemyDistance < attackRange && abilityToUse != null)
        {
            if (useAbility(abilityToUse, currentTarget.beingBehavior.transform.position, currentTarget.beingBehavior))
            {
                abilityToUse = null;
                outOfRangeAggroTarget = null;
            }
        }
    }

    public override void takeDamage(float damage, DatabaseElement damageOrigin, BeingBehavior damageDealer)
    {
        base.takeDamage(damage, damageOrigin, damageDealer);
        addAggro(damage / BeingConstant.damageAggroFactor, damageDealer);
    }

    /// <summary>
    /// Add aggro to an being behavior in the enemy list
    /// </summary>
    /// <param name="value">The aggro value</param>
    /// <param name="aggroTarget">The being which generate the aggro</param>
    void addAggro(float value, BeingBehavior aggroTarget)
    {
        if (currentTarget == null)
        {
            if(outOfRangeAggroTarget == null || outOfRangeAggroTarget.aggro < enemies.Find(x => x.beingBehavior == aggroTarget).aggro)
            {
                outOfRangeAggroTarget = enemies.Find(x => x.beingBehavior == aggroTarget);
                CancelInvoke("removeOutOfRangeTarget");
                Invoke("removeOutOfRangeTarget", BeingConstant.outOfRangeTargetTimer);
            }
        }

        int index = enemies.FindIndex(x => x.beingBehavior == aggroTarget);
        enemies[index].addAggro(value);
    }

    void resetAggro()
    {
        for (int i = 0; i < enemies.Count; i++)
            enemies[i].aggro = 0;
    }

    /// <summary>
    /// Remove the out of range target
    /// </summary>
    void removeOutOfRangeTarget()
    {
        outOfRangeAggroTarget = null;
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
