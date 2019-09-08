using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : Being
{
    #region attributs
    [SerializeField] List<Loot> _possibleLoot = new List<Loot>();
    public List<Loot> possibleLoot { get => _possibleLoot; }

    [SerializeField] float _experience;
    public float experience { get => _experience; }
    #endregion

    public Enemy(string name, float baseLife, float baseASPD, float baseCastSpeed, float baseAttackRange, float baseMovementSpeed, 
        List<Ability> abilities, GameObject prefab, List<Loot> possibleLoot, float experience) :
        base(name, baseLife, baseASPD, baseCastSpeed, baseAttackRange, baseMovementSpeed, abilities, prefab)
    {
        this._possibleLoot = possibleLoot;
        this._experience = experience;
    }

    public Enemy(){ }

    public Enemy(Enemy enemy) :
        base(enemy.name, enemy._stats.maxLife, enemy._stats.attackSpeed, enemy._stats.castSpeed, enemy._stats.attackRange, enemy._stats.movementSpeed, enemy.abilities, enemy.prefab)
    {
        this._possibleLoot = enemy._possibleLoot;
        this._experience = enemy.experience;
    }

    /// <summary>
    /// Generate the list of item the enemy will drop
    /// </summary>
    /// <returns>The list of loot to drop</returns>
    public List<Loot> generateLoot()
    {
        List<Loot> loot = new List<Loot>();
        for(int i =0; i < _possibleLoot.Count; i++)
        {
            //Get a random number between 0 and 100 with a precision of 1
            float randomRoll = Random.Range(0.0f, 100.0f);
            if (randomRoll > 0 && _possibleLoot[i].changeToDrop >= randomRoll)
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
        if (_possibleLoot[i].isRandom)
            _possibleLoot[i].getRandomItem();
        return _possibleLoot[i];
    }
}
