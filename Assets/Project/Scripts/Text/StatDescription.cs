using UnityEngine;
using System.Collections;

public static class StatDescription
{
    public static string getStatDescription(float value, StatType statType, StatBonusType bonusType = StatBonusType.Pure)
    {
        string description = getBonusTypeDescription(bonusType, value);
        string statTypeDescription;
        switch (statType)
        {
            case StatType.LightningDamage: statTypeDescription = "LightningDamage_Description".localize(); break;
            case StatType.CastSpeed: statTypeDescription = "CastSpeed_Description".localize(); break;
            case StatType.AttackSpeed: statTypeDescription = "AttackSpeed_Description".localize(); break;
            case StatType.AttackRange: statTypeDescription = "AttackRange_Description".localize(); break;
            case StatType.ProjectileDamage: statTypeDescription = "ProjectileDamage_Description".localize(); break;
            case StatType.ProjectileSpeed: statTypeDescription = "ProjectileSpeed_Description".localize(); break;
            case StatType.NumberOfProjectile: statTypeDescription = "Projectile_Description".localize(); break;
            case StatType.AreaDamage: statTypeDescription = "AreaDamage_Description".localize(); break;
            case StatType.AreaSize: statTypeDescription = "AreaSize_Description".localize(); break;
            case StatType.MovementSpeed: statTypeDescription = "MovementSpeed_Description".localize(); break;
            default: statTypeDescription = statType.ToString(); break;
        }

        return description.Replace("{Description}", statTypeDescription).sentenceFormat();
    }

    static string getBonusTypeDescription(StatBonusType bonusType, float value)
    {
        string description;
        switch (bonusType)
        {
            case StatBonusType.Pure: description = "PureBonus_Description".localize(); break;
            case StatBonusType.additional: description = value > 0 ? "AdditionalBonus_Description".localize() : "ReducedBonus_Description".localize(); break;
            case StatBonusType.Multiplied: description = value > 0 ? "MoreBonus_Description".localize() : "LessBonus_Description".localize(); break;
            default: description = ""; break;
        }

        return description.Replace("{Value}", value.ToString());
    }
}
