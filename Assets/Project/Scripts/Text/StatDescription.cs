using UnityEngine;
using System.Collections;

public static class StatDescription
{
    public static string getStatDescription(float value, StatType statType, StatBonusType bonusType = StatBonusType.Pure)
    {
        string description = "";
        switch (statType)
        {
            case StatType.CastSpeed: description = "cast speed"; break;
            case StatType.AttackSpeed: description = "attack speed"; break;
            case StatType.AttackRange: description = "attack range"; break;
            case StatType.ProjectileDamage: description = "projectile damage"; break;
            case StatType.ProjectileSpeed: description = "projectile speed"; break;
            case StatType.NumberOfProjectile: description = "projectile"; break;
            case StatType.AreaDamage: description = "area damage"; break;
            case StatType.AreaSize: description = "area size"; break;
            case StatType.MovementSpeed: description = "movement speed"; break;
            default: description = statType.ToString(); break;
        }

        description = getBonusTypeDescription(bonusType, value) + description;

        return description;
    }

    static string getBonusTypeDescription(StatBonusType bonusType, float value)
    {
        string description = "";
        switch (bonusType)
        {
            case StatBonusType.Pure: description = "{0} bonus "; break;
            case StatBonusType.additional: description = value > 0 ? "{0}% additionnal" : "{0}% reduced"; break;
            case StatBonusType.Multiplied: description = value > 0 ? "{0}% more" : "{0}% less"; break;
            default: description = ""; break;
        }

        return description.Replace("{0}", value.ToString());
    }
}
