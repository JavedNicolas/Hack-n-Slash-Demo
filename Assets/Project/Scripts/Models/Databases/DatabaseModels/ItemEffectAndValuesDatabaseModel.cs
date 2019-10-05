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
    [SerializeField] public EffectUseBy usedBy;

    [Header("Start influencing the value")]
    [SerializeField] public List<StatType> statTypes;

    public ItemEffectAndValuesDatabaseModel(ItemEffectAndValue effectAndValue, ResourcesList resourcesList)
    {
        effectGUID = resourcesList.getGUIDFor(effectAndValue.effect);
        value = effectAndValue.value;
        usedBy = effectAndValue.usedBy;
        statTypes = effectAndValue.statTypes;
    }

    public ItemEffectAndValue dataBaseModelToItemEffectAndValue(ResourcesList resourcesList)
    {
        Effect effect = (Effect)resourcesList.getObject<ScriptableObject>(effectGUID);
        ItemEffectAndValue itemEffectAndValue = new ItemEffectAndValue(value, effect, usedBy, statTypes);
        return itemEffectAndValue;
    }
}
