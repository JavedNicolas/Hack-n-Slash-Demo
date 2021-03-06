﻿using UnityEngine;
using System.Collections;

public enum StatType
{
    // Base start
    Intelligence,
    Strength,
    Dexterity,

    // Life and mana
    Life,
    Mana,

    // Damage Types
    Physical,
    Ice,
    Fire,
    LightningDamage,
    Wind,

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
    AreaBurstDamage,
    AreaOverTimeDamage,
    AreaSize,
    AreaOverTimeDelay,

    // Degen
    Degen,

    // Movement
    MovementSpeed
}

public enum StatBonusType
{
    Pure, // add pure stat (before every other bonus)
    additional, // add a percentage of the base damage as bonus
    Multiplied // add a percentage of the total damage (after pure and additional are added)
}

public enum StatInfluencedBy
{
    // none
    Nothing,

    // basic statc
    Intelligence,
    Strength,
    Dexterity,

    // other
    MaxLife,
    MaxMana,
    Level,
    JobLevel

}