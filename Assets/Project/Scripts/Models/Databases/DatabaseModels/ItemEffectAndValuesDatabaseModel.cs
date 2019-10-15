using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemEffectAndValuesDatabaseModel
{
    [Header("Effect and values")]
    [SerializeField] public float value;
    [SerializeField] public string effectName;

    [Header("Starting time")]
    [SerializeField] public EffectUseBy usedBy;

    [Header("Start influencing the value")]
    [SerializeField] public List<StatType> statTypes;

    public ItemEffectAndValuesDatabaseModel(ItemEffectAndValue effectAndValue, ResourcesList resourcesList)
    {
        effectName = effectAndValue.effect.getName();
        value = effectAndValue.value;
        usedBy = effectAndValue.usedBy;
        statTypes = effectAndValue.statTypes;
    }

    public ItemEffectAndValue dataBaseModelToItemEffectAndValue(ResourcesList resourcesList)
    {
        Effect effect = EffectList.effects.Find(x => x.getName() == effectName);
        ItemEffectAndValue itemEffectAndValue = new ItemEffectAndValue(value, effect, usedBy, statTypes);
        return itemEffectAndValue;
    }
}
