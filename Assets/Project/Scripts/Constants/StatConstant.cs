using UnityEngine;
using System.Collections;

public enum StatType
{
    // Base start
    Intelligence,
    Strenght,
    Dexterity,

    // Life and mana
    Life,
    Mana,

    // ability speed
    CastSpeed,
    AttackSpeed,
    AttackRange,

    // Projectile
    ProjectileDamage,
    ProjectileSpeed,
    NumberOfProjectile,

    // Area
    AreaDamage,
    AreaSize,

    // Movement
    MovementSpeed
}


public enum StatBonusType
{
    Pure, // add pure stat (before every other bonus)
    additional, // add a percentage of the base damage as bonus
    Multiplied // add a percentage of the total damage (after pure and additional are added)
}