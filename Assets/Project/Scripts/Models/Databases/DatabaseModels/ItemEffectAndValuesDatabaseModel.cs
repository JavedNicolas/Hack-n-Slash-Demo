using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemEffectAndValuesDatabaseModel
{
    [Header("Effect and values")]
    [SerializeField] public float value;
    [SerializeField] public string effectGUID;

    [Header("Starting time")]
    [SerializeField] public EffectType startingTime;

    [Header("Start influencing the value")]
    [SerializeField] public List<StatType> statTypes;

    public ItemEffectAndValuesDatabaseModel(ItemEffectAndValue effectAndValue, DatabaseResourcesList resourcesList)
    {
        effectGUID = resourcesList.getGUIDFor(effectAndValue.effect);
        value = effectAndValue.value;
        startingTime = effectAndValue.startingTime;
        statTypes = effectAndValue.statTypes;
    }

    public ItemEffectAndValue dataBaseModelToItemEffectAndValue(DatabaseResourcesList resourcesList)
    {
        Effect effect = (Effect)resourcesList.getObject<ScriptableObject>(effectGUID);
        ItemEffectAndValue itemEffectAndValue = new ItemEffectAndValue(value, effect, startingTime, statTypes);
        return itemEffectAndValue;
    }
}
