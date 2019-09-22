using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemEffectAndValue
{
    [Header("Effect and values")]
    [SerializeField] public float value;
    [SerializeField] public Effect effect;

    [Header("Starting time")]
    [SerializeField] public EffectType startingTime;

    [Header("Start influencing the value")]
    [SerializeField] public List<StatType> statTypes;

    public ItemEffectAndValue() { }

    public ItemEffectAndValue(float value, Effect effect, EffectType startingTime, List<StatType> statTypes)
    {
        this.value = value;
        this.effect = effect;
        this.startingTime = startingTime;
        this.statTypes = statTypes;
    }

    /// <summary>
    /// use the effect with the buffed attributs
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="target"></param>
    /// <param name="effectOrigin"></param>
    public bool useEffect(BeingBehavior sender, GameObject target, DatabaseElement effectOrigin)
    {
        if (!canBeUsed(sender, target, effectOrigin))

        return false;
        float valueBuffed = sender.being.stats.getBuffedValue(value, statTypes, GetType().Name);
        effect.use(sender, target, valueBuffed, effectOrigin);
 
        return true;
    }

    /// <summary>
    /// Check if the effect can be used
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="target"></param>
    /// <param name="effectOrigin"></param>
    /// <returns></returns>
    public bool canBeUsed(BeingBehavior sender, GameObject target, DatabaseElement effectOrigin)
    {
        float valueBuffed = sender.being.stats.getBuffedValue(value, statTypes, GetType().Name);
        if (!effect.canBeUsed(sender, target, valueBuffed))
            return false;

        return true;
    }

    public string getName()
    {
        throw new System.NotImplementedException();
    }

    public string getDescription(Being owner, DatabaseElement effectOrigin)
    {
        float buffedValue = owner.stats.getBuffedValue(value, statTypes, effectOrigin.getName());

        return effect.description.Replace("{0}" , buffedValue.ToString()); ;
    }
}
