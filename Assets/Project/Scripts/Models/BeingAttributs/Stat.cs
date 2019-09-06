using UnityEngine;
using System.Collections;

public struct Stat
{
    [SerializeField] public StatType statType;
    [SerializeField] public StatBonusType bonusType;
    [SerializeField] public float value;
    [SerializeField] public string sourceName;

    public Stat(StatType statType, StatBonusType bonusType, float value, string sourceName)
    {
        this.statType = statType;
        this.bonusType = bonusType;
        this.value = value;
        this.sourceName = sourceName;
    }
}
