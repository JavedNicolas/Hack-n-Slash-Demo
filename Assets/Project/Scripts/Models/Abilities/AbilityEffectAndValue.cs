using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AbilityEffectAndValue
{
    [Header("Effect and values")]
    [SerializeField] public List<float> valuesByLevel = new List<float>();
    [SerializeField] public List<StatType> statTypes = new List<StatType>();

    [SerializeField] public int effectIndex;
    [SerializeField] public Element element;

    [Header("Starting time")]
    [SerializeField] public EffectUseBy effectType;

    /// <summary>
    /// use the effect with the buffed attributs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="target"></param>
    /// <param name="effectOrigin"></param>
    public bool useEffect(BeingBehavior sender, GameObject target, Ability effectOrigin)
    {
        float currentLevelValue = getValueForCurrentLevel(getCurrentLevel(effectOrigin));
        float valueBuffed = sender.being.stats.getBuffedValue(currentLevelValue, statTypes, GetType().Name, getStat(effectOrigin));
        getEffect().use(sender, target, valueBuffed, effectOrigin);

        return true;
    }
    /// <summary>
    /// get the currentLevel
    /// </summary>
    /// <param name="effectOrigin"></param>
    /// <returns></returns>
    int getCurrentLevel(Ability effectOrigin)
    {
        return effectOrigin.stats.currentLevel;
    }

    /// <summary>
    /// Return the list of the stat for the effectOrigin
    /// </summary>
    /// <param name="effectOrigin"></param>
    /// <returns></returns>
    List<Stat> getStat(Ability effectOrigin)
    {
        return effectOrigin.stats.statList;
    }

    public string getName()
    {
        throw new System.NotImplementedException();
    }

    public string getDescription(Being owner, Ability effectOrigin)
    {
        string description = "";
        float value = getValueForCurrentLevel(getCurrentLevel(effectOrigin));
        float buffedValue = owner.stats.getBuffedValue(value, statTypes, effectOrigin.getName(), getStat(effectOrigin));
        description += getEffect().getDescription(buffedValue, element);

        return description;
    }

    /// <summary>
    /// Get the value for the currentLevel
    /// </summary>
    /// <param name="currentLevel"></param>
    /// <returns></returns>
    float getValueForCurrentLevel(int currentLevel)
    {
        if(currentLevel <= valuesByLevel.Count)
            return valuesByLevel[currentLevel - 1];

        return valuesByLevel[valuesByLevel.Count - 1];
    }

    Effect getEffect()
    {
        return EffectList.effects[effectIndex];
    }
}

