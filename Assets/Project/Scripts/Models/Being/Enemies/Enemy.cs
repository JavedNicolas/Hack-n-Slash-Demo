using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : Being
{
    [SerializeField] List<Loot> _possibleLoot = new List<Loot>();
    public List<Loot> possibleLoot { get => _possibleLoot; }

    public Enemy(string name, float currentLife, float baseLife, float shield, float aSPD, float attackRange, int strength, List<Ability> skills, float movementSpeedPercentage, GameObject prefab, float projectileSpeed) : base(name, currentLife, baseLife, shield, aSPD, attackRange, strength, skills, movementSpeedPercentage, prefab, projectileSpeed)
    {
    }

    public Enemy()
    {
    }

    public Enemy(Enemy enemy) : base(enemy.name, enemy.baseLife, enemy.baseLife, enemy.shield, enemy.bonusAttackSpeed, enemy.attackRange, enemy.strength, enemy.skills, enemy.movementSpeedBonus, enemy.prefab, enemy.projectileSpeed)
    {
    }

}
