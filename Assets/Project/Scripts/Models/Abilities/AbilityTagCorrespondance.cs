using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AbilityTagCorrespondance
{

    static public List<StatType> GetStatTypesFor(AbilityTag tag)
    {
        switch (tag)
        {
            case AbilityTag.Attack: return new List<StatType>() { StatType.AttackSpeed};
            case AbilityTag.Spell: return new List<StatType>() { StatType.CastSpeed };
            case AbilityTag.Projectile: return new List<StatType>() { StatType.ProjectileDamage, StatType.ProjectileSpeed, StatType.NumberOfProjectile };
            case AbilityTag.Area: return new List<StatType>() { StatType.AreaDamage, StatType.AreaSize};
            case AbilityTag.Heal: return new List<StatType>() { };
            case AbilityTag.Support: return new List<StatType>() { };
            case AbilityTag.Physic: return new List<StatType>() { StatType.Physical };
            case AbilityTag.Elemental: return new List<StatType>() { StatType.LightningDamage, StatType.Fire, StatType.Ice, StatType.Wind  };
            case AbilityTag.Lightning: return new List<StatType>() { StatType.LightningDamage};
            case AbilityTag.Fire: return new List<StatType>() { StatType.Fire };
            case AbilityTag.Wind: return new List<StatType>() { StatType.Wind };
            case AbilityTag.Ice: return new List<StatType>() { StatType.Ice };
            case AbilityTag.Movement: return new List<StatType>() { StatType.MovementSpeed };
            default: return new List<StatType>() { };
        }
    }

    static public List<StatType> getStatTypesToDescribeFor(AbilityTag tag)
    {
        switch (tag)
        {
            case AbilityTag.Attack: return new List<StatType>() { };
            case AbilityTag.Spell: return new List<StatType>() { };
            case AbilityTag.Projectile: return new List<StatType>() { StatType.ProjectileSpeed, StatType.NumberOfProjectile };
            case AbilityTag.Area: return new List<StatType>() { StatType.AreaSize, StatType.AreaDamage, StatType.AreaOverTimeDelay };
            case AbilityTag.Heal: return new List<StatType>() { };
            case AbilityTag.Support: return new List<StatType>() { };
            case AbilityTag.Physic: return new List<StatType>() {};
            case AbilityTag.Elemental: return new List<StatType>() { };
            case AbilityTag.Lightning: return new List<StatType>() { };
            case AbilityTag.Fire: return new List<StatType>() { };
            case AbilityTag.Wind: return new List<StatType>() {  };
            case AbilityTag.Ice: return new List<StatType>() {  };
            case AbilityTag.Movement: return new List<StatType>() {  };
            default: return new List<StatType>() { };
        }
    }
}
