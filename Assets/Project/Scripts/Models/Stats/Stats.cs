using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Stats
{
    [SerializeField] protected List<Stat> _stats = new List<Stat>();
    public List<Stat> stats { get => _stats; }

    /// <summary>
    /// try to add a stat to the player
    /// </summary>
    /// <param name="stat">The stat to add</param>
    /// <returns>Return true if the stat has be added, false if not. \n
    /// Ex : It can fail if the player cannot get this stat, or if it already has it from the same source)</returns>
    public bool addStat(Stat stat)
    {
        if (_stats.Exists(x => x.sourceName == stat.sourceName && x.bonusType == stat.bonusType && x.statType == stat.statType &&
        x.isSpecificTo == stat.isSpecificTo && x.isInfluencedBy == stat.isInfluencedBy))
            return false;

        _stats.Add(stat);

        return true;
    }

    /// <summary>
    /// try to remove a stat to the player
    /// </summary>
    /// <param name="stat">The stat to remove</param>
    /// <returns>Return true if the stat has be removed, false if not. \n
    /// Ex : The player does not have this stat from this source</returns>
    public bool removeStat(string sourceName)
    {
        List<Stat> statsFound = _stats.FindAll(x => x.sourceName == sourceName);
        if (statsFound.Count == 0)
            return false;

        for (int i = 0; i < statsFound.Count; i++)
        {
            if (_stats.Contains(statsFound[i]))
            {
                _stats.Remove(statsFound[i]);
            }
        }

        return true;
    }

    /// <summary>
    /// Return the value buffed by the player stats as an int
    /// </summary>
    /// <param name="value">the value to buff</param>
    /// <param name="statType">The type of the value </param>
    /// <returns>An ceiled int</returns>
    public int getBuffedValue(float value, StatType statType, string specificTo = "")
    {
        return Mathf.FloorToInt(getBuffedValue(value, new List<StatType>() { statType }, specificTo));
    }

    /// <summary>
    /// Return the value buffed by the player stats as an int
    /// </summary>
    /// <param name="value">the value to buff</param>
    /// <param name="statType">The type of the value </param>
    /// <returns>An ceiled int</returns>
    public int getBuffedValue(int value, StatType statType, string specificTo = "")
    {
        return Mathf.FloorToInt(getBuffedValue((float)value, new List<StatType>() { statType }, specificTo));
    }

    /// <summary>
    /// Return the value buffed by the player stats as an int
    /// </summary>
    /// <param name="value">the value to buff</param>
    /// <param name="statType">The type of the value </param>
    /// <returns>An ceiled int</returns>
    public int getBuffedValue(int value, List<StatType> statTypes, string specificTo = "")
    {
        return Mathf.FloorToInt(getBuffedValue((float)value, statTypes, specificTo));
    }

    /// <summary>
    /// Return the value buffed by the player stats
    /// </summary>
    /// <param name="value">the value to buff</param>
    /// <param name="statType">The type of the value </param>
    /// <returns></returns>
    public virtual float getBuffedValue(float value, List<StatType> statTypes, string specificTo = "")
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
                    switch (currentStat.bonusType)
                    {
                        case StatBonusType.Pure: pureBonus += currentStat.value; break;
                        case StatBonusType.additional: additionalBonus += currentStat.value; break;
                        case StatBonusType.Multiplied: multipliedBonus += currentStat.value; break;
                    }
                }
            }

        float buffedValue = ((value + pureBonus) + ((value + pureBonus) * (additionalBonus / 100))) * (1 + (multipliedBonus / 100));

        return buffedValue;
    }

    
}
