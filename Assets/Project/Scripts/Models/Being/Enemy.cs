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

    public Enemy(string name, float baseLife, float shield, float aSPD, float attackRange, int strength, List<Ability> skills, float movementSpeedPercentage, 
        GameObject prefab, float projectileSpeed, List<Loot> possibleLoot, float experience) : base(name, baseLife, shield, aSPD, attackRange, strength, skills, movementSpeedPercentage, prefab, projectileSpeed)
    {
        this._possibleLoot = possibleLoot;
        this._experience = experience;
    }

    public Enemy(){ }

    public Enemy(Enemy enemy) : base(enemy.name, enemy.baseLife, enemy.shield, enemy.bonusAttackSpeed, enemy.attackRange, enemy.strength, enemy.ability, enemy.movementSpeedBonus, enemy.prefab, enemy.projectileSpeed)
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

    Loot getLootAt(int i)
    {
        if (_possibleLoot[i].isRandom)
            _possibleLoot[i].getRandomItem();
        return _possibleLoot[i];
    }
}
