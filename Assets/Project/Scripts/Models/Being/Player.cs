using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Being
{
    public Player(float currentLife, float baseLife, float shield, float aSPD, float attackRange, float strength, List<Skill> skills, float movementSpeedPercentage) : 
        base(currentLife, baseLife, shield, aSPD, attackRange, strength, skills, movementSpeedPercentage)
    {
        
    }
}
