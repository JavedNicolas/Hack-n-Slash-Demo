using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EffectAndValue
{
    [Header("Effect and values")]
    [SerializeField] public float value;
    [SerializeField] public Effect effect;

    [Header("Starting time")]
    [SerializeField] public EffectStartingTime startingTime;

    [Header("Start influencing the value")]
    [SerializeField] public List<StatType> statTypes;


    public EffectAndValue() { }

    public void addEffectAndValue(Effect effect, float value, List<StatType> statTypes)
    {
        this.effect = effect;
        this.value = value;
        this.statTypes = statTypes;
    }

    public void useEffect(BeingBehavior sender, GameObject target, DatabaseElement effectOrigin)
    {
        float valueBuffed = sender.being.stats.getBuffedValue(value, statTypes, GetType().Name);
        effect.use(sender, target, valueBuffed, effectOrigin);
    }

    public string getName()
    {
        throw new System.NotImplementedException();
    }

    public string getDescription(Being owner, string name)
    {
        return effect.description.Replace("{0}", owner.stats.getBuffedValue(value, statTypes, name).ToString() + "\n");
    }
}

