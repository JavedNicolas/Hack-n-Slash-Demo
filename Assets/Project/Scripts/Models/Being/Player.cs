using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Being
{
    public Player(string name, float currentLife, float baseLife, float shield, float aSPD, float attackRange, int strength, List<Skill> skills, float movementSpeedPercentage, GameObject prefab, float projectileSpeed) : base(name, currentLife, baseLife, shield, aSPD, attackRange, strength, skills, movementSpeedPercentage, prefab, projectileSpeed)
    {
    }
}
