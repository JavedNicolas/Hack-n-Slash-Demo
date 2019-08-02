using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : Being
{
    public Enemy(float currentLife, float baseLife, float shield, float aSPD, float attackRange, float strength, List<Skill> skills, float movementSpeedPercentage) :
        base(currentLife, baseLife, shield, aSPD, attackRange, strength, skills, movementSpeedPercentage){

    }

    public Enemy() { }

    public Enemy(Enemy enemy) : base(enemy.currentLife, enemy.baseLife, enemy.shield, enemy.aspd, enemy.attackRange, enemy.strength, enemy.skills, enemy.movementSpeedPercentage)
    {
        this.currentLife = this.baseLife;
    }

    public void setLife()
    {
        currentLife = baseLife;
    }
}
