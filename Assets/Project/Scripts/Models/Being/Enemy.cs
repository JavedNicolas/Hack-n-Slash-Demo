using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : Being
{
    #region attributs
    [SerializeField] public List<Loot> possibleLoot = new List<Loot>();

    [SerializeField] List<AbilityUsageFrequence> _abilityUsageFrequency = new List<AbilityUsageFrequence>();
    public List<AbilityUsageFrequence> abilityUsageFrequency { get => _abilityUsageFrequency; }

    [SerializeField] float _experience;
    public float experience { get => _experience; }
    #endregion

    public Enemy(){ }

    public Enemy(Enemy enemy) :
        base(enemy.name, enemy.stats.maxLife, enemy.stats.attackSpeed, enemy.stats.castSpeed, enemy.stats.attackRange, enemy.stats.movementSpeed, enemy.abilityIDs, enemy.prefab)
    {
        this.possibleLoot = enemy.possibleLoot;
        this._experience = enemy.experience;
        this._abilityUsageFrequency = enemy.abilityUsageFrequency;
    }

    public Enemy(string name, float baseLife, float baseASPD, float baseCastSpeed, float baseAttackRange, float baseMovementSpeed,
    List<int> abilityIDs, List<AbilityUsageFrequence> abilityUsageFrequences, GameObject prefab, List<Loot> possibleLoot, float experience) :
    base(name, baseLife, baseASPD, baseCastSpeed, baseAttackRange, baseMovementSpeed, abilityIDs, prefab)
    {
        this.possibleLoot = new List<Loot>(possibleLoot);
        this._experience = experience;
        this._abilityUsageFrequency = abilityUsageFrequences;
    }

    /// <summary>
    /// Generate the list of item the enemy will drop
    /// </summary>
    /// <returns>The list of loot to drop</returns>
    public List<Loot> generateLoot()
    {
        List<Loot> loot = new List<Loot>();
        for(int i =0; i < possibleLoot.Count; i++)
        {
            //Get a random number between 0 and 100 with a precision of 1
            float randomRoll = Random.Range(0.0f, 100.0f);
            if (randomRoll > 0 && possibleLoot[i].changeToDrop >= randomRoll)
                loot.Add(new Loot(getLootAt(i)));
        }
        return loot;
    }

    /// <summary>
    /// get A loot for the index, or a random loot if the loot is random
    /// </summary>
    /// <param name="i">index</param>
    /// <returns>a loot</returns>
    Loot getLootAt(int i)
    {
        return possibleLoot[i];
    }
}
