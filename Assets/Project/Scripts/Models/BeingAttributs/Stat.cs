using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Stat
{
    [SerializeField] public StatType statType;
    [SerializeField] public StatBonusType bonusType;
    [SerializeField] public StatInfluencedBy isInfluencedBy;
    [SerializeField] public float influencedEvery; // is influence by 1 every x value
    [SerializeField] public float value;
    [SerializeField] public string sourceName;


    /// <summary>
    /// Constructor for influenced stat
    /// </summary>
    /// <param name="statType">The type of stat</param>
    /// <param name="bonusType">The type of bonus</param>
    /// <param name="value">The value of the stat</param>
    /// <param name="sourceName">From where the stat come from</param>
    /// <param name="statInfluencedBy">The stat it's influenced by</param>    
    /// <param name="influencedEvery"></param>
    public Stat(StatType statType, StatBonusType bonusType,float value, string sourceName, StatInfluencedBy statInfluencedBy = StatInfluencedBy.Nothing, float influencedEvery = 1)
    {
        this.statType = statType;
        this.bonusType = bonusType;
        this.isInfluencedBy = statInfluencedBy;
        this.influencedEvery = influencedEvery;
        this.value = value;
        this.sourceName = sourceName;
    }
}
