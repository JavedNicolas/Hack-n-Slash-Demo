using UnityEngine;
using System.Collections;

[System.Serializable]
public class Stat
{
    [SerializeField] public StatType statType;
    [SerializeField] public StatBonusType bonusType;
    [SerializeField] public StatInfluencedBy isInfluencedBy;
    [SerializeField] public float influencedEvery; // is influence by 1 every x value
    [SerializeField] public float value;
    [SerializeField] public string sourceName;
    [SerializeField] public bool isSpecific;
    [SerializeField] public string isSpecificTo;

    public Stat() { }

    public Stat(StatType statType, StatBonusType bonusType,float value, string sourceName)
    {
        this.statType = statType;
        this.bonusType = bonusType;
        this.isInfluencedBy = StatInfluencedBy.Nothing;
        this.influencedEvery = 1;
        this.value = value;
        this.sourceName = sourceName;
        this.isSpecific = false;
        this.isSpecificTo = "";
    }

    public Stat(StatType statType, StatBonusType bonusType, float value, string sourceName, StatInfluencedBy statInfluencedBy, float influencedEvery = 1)
    {
        this.statType = statType;
        this.bonusType = bonusType;
        this.isInfluencedBy = statInfluencedBy;
        this.influencedEvery = influencedEvery;
        this.value = value;
        this.sourceName = sourceName;
        this.isSpecific = false;
        this.isSpecificTo = "";
    }

    public Stat(StatType statType, StatBonusType bonusType, float value, string sourceName, string isSpecificTo)
    {
        this.statType = statType;
        this.bonusType = bonusType;
        this.isInfluencedBy = StatInfluencedBy.Nothing;
        this.influencedEvery = 1;
        this.value = value;
        this.sourceName = sourceName;
        this.isSpecific = true;
        this.isSpecificTo = isSpecificTo;
    }

    public Stat(StatType statType, StatBonusType bonusType, float value, string sourceName, StatInfluencedBy statInfluencedBy, float influencedEvery, string isSpecificTo)
    {
        this.statType = statType;
        this.bonusType = bonusType;
        this.isInfluencedBy = statInfluencedBy;
        this.influencedEvery = influencedEvery;
        this.value = value;
        this.sourceName = sourceName;
        this.isSpecific = true;
        this.isSpecificTo = isSpecificTo;
    }
}
