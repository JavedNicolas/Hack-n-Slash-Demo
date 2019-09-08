using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BeingStats : Stats
{
    [Header("Stats")]
    [SerializeField] protected int _intelligence;
    public int intelligence { get => getBuffedValue(_intelligence, StatType.Intelligence); }

    [SerializeField] protected int _strength;
    public int strength { get => getBuffedValue(_strength, StatType.Strength); }

    [SerializeField] protected int _dexterity;
    public int dexterity { get => getBuffedValue(_dexterity, StatType.Dexterity); }

    [Header("Base Stat")]
    [SerializeField] float _maxLife;
    public float maxLife { get { return Mathf.FloorToInt(getBuffedValue(_maxLife, StatType.Life)); } }

    [SerializeField] float _attackRange;
    public float attackRange { get => getBuffedValue(_attackRange, StatType.AttackRange); }

    [SerializeField] float _movementSpeed;
    public float movementSpeed { get => getBuffedValue(_movementSpeed, StatType.MovementSpeed); }

    [SerializeField] float _attackSpeed;
    public float attackSpeed { get => getBuffedValue(_attackSpeed, StatType.AttackSpeed); }

    [SerializeField] float _castSpeed;
    public float castSpeed { get => getBuffedValue(_castSpeed, StatType.CastSpeed); }

    [SerializeField] protected int _currentLevel;
    public int currentLevel { get => _currentLevel; }

    public BeingStats() { }

    public BeingStats(float maxLife, float attackSpeed, float castSpeed, float attackRange, float movementSpeed, int currentLevel)
    {
        this._maxLife = maxLife;
        this._attackSpeed = attackSpeed;
        this._castSpeed = castSpeed;
        this._attackRange = attackRange;
        this._movementSpeed = movementSpeed;
        this._currentLevel = currentLevel;
    }

    /// <summary>
    /// Return the value buffed by the player stats
    /// </summary>
    /// <param name="value">the value to buff</param>
    /// <param name="statType">The type of the value </param>
    /// <returns></returns>
    public override float getBuffedValue(float value, List<StatType> statTypes, string specificTo = "")
    {
        float pureBonus = 0;
        float additionalBonus = 0;
        float multipliedBonus = 0;

        foreach (StatType statType in statTypes)
            for (int i = 0; i < stats.Count; i++)
            {
                Stat currentStat = stats[i];
                // get the stat value if the stat is of the same type as asked and 
                // if the stat is no specific
                // or if it's specific and match the specific To paramater
                if (currentStat.statType == statType && (!currentStat.isSpecific || (currentStat.isSpecific && currentStat.isSpecificTo == specificTo)))
                {
                    float influenceFactor = getInflencedFactor(currentStat);
                    influenceFactor = Mathf.FloorToInt(influenceFactor / currentStat.influencedEvery);
                    switch (currentStat.bonusType)
                    {
                        case StatBonusType.Pure: pureBonus += currentStat.value * influenceFactor; break;
                        case StatBonusType.additional: additionalBonus += currentStat.value * influenceFactor; break;
                        case StatBonusType.Multiplied: multipliedBonus += currentStat.value * influenceFactor; break;
                    }
                }
            }

        float buffedValue = ((value + pureBonus) + ((value + pureBonus) * (additionalBonus / 100))) * (1 + (multipliedBonus / 100));

        return buffedValue;
    }

    protected virtual float getInflencedFactor(Stat stat)
    {
        switch (stat.isInfluencedBy)
        {
            case StatInfluencedBy.Nothing: return 1;
            case StatInfluencedBy.Intelligence: return intelligence;
            case StatInfluencedBy.Strength: return strength;
            case StatInfluencedBy.Dexterity: return dexterity;
            case StatInfluencedBy.MaxLife: return maxLife;
            case StatInfluencedBy.Level: return _currentLevel;
            default: return 0;
        }
    }
}
